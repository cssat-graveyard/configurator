using MySql.Data.MySqlClient;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Framework
{
	public static class Database
	{
		private static string hostName = "pocweb.cac.washington.edu";
		private static int sshPort = 22;
		private static string dbHost = "localhost";
		private static int dbPort = 3306;
		private static string dbUid = "test_annie";
		private static string dbPassword = "b4Rxx:pW";
		private static string connectionString = String.Format("HOST={0};PORT={1};UID={2};PASSWORD={3}", dbHost, dbPort, dbUid, dbPassword);

		private static string _userName = String.Empty;
		private static string UserName
		{
			get
			{
				if (String.IsNullOrEmpty(_userName))
				{
#if DEBUG
					_userName = "schmitzr";
					_password = "sara.leia.schmitz";
#else
					CredentialsPrompt prompt = new CredentialsPrompt();
					
					if (prompt.ShowDialog() == DialogResult.OK)
					{
						_userName = prompt.UserName;
						_password = prompt.Password;
					}
					else
					{
						throw new DatabaseException("Required credentials not provided.");
					}
#endif
				}

				return _userName;
			}
		}

		private static string _password = String.Empty;
		private static string Password
		{
			get
			{
				if (String.IsNullOrEmpty(_password))
				{
					CredentialsPrompt prompt = new CredentialsPrompt();

					if (!String.IsNullOrEmpty(_userName))
						prompt.UserName = _userName;

					if (prompt.ShowDialog() == DialogResult.OK)
					{
						_userName = prompt.UserName;
						_password = prompt.Password;
					}
					else
					{
						throw new DatabaseException("Required credentials not provided.");
					}
				}

				return _password;
			}
		}

		private static Dictionary<string, Table> _tables;
		public static Dictionary<string, Table> Tables
		{
			get
			{
				if (_tables == null)
					_tables = new Dictionary<string, Table>();

				return _tables;
			}
			private set { _tables = value; }
		}

		private static Dictionary<string, StoredProcedure> _storedProcedures;
		public static Dictionary<string, StoredProcedure> StoredProcedures
		{
			get
			{
				if (_storedProcedures == null)
					_storedProcedures = new Dictionary<string, StoredProcedure>();

				return _storedProcedures;
			}
			private set { _storedProcedures = value; }
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public static void RefreshFromDatabase()
		{
			Tables.Clear();
			StoredProcedures.Clear();

			using (SshClient client = new SshClient(hostName, sshPort, UserName, Password))
			{
				try
				{
					client.Connect();

					ForwardedPort forwardedPort = new ForwardedPortLocal(dbHost, (uint)dbPort, hostName, (uint)dbPort);
					client.AddForwardedPort(forwardedPort);
					forwardedPort.Start();

					MySqlConnection db = new MySqlConnection(connectionString);
					db.Open();
					RetrieveTables(ref db);
					RetrieveStoredProcedures(ref db);
					db.Close();
				}
				catch (DatabaseException)
				{
					throw;
				}
				catch (SshAuthenticationException e)
				{
					string message = String.Format("A problem occurred while connecting to the database:\n{0}", e.Message);
					_password = String.Empty;
					throw new DatabaseException(message, e);
				}
				catch (Exception e)
				{
					string message = String.Format("A problem occured while connecting to the database:\n{0}", e.Message);
					throw new DatabaseException(message, e);
				}
				finally
				{
					client.Disconnect();
				}
			}
		}

		private static void RetrieveTables(ref MySqlConnection db)
		{
			try
			{
				List<string> IgnoreTables = new List<string>();
				IgnoreTables.Add("ref_last_dw_transfer");
				IgnoreTables.Add("ref_leading_digit_int_param_key");

				string query = @"select TABLE_NAME from information_schema.TABLES where TABLE_SCHEMA = 'test_annie' and (TABLE_NAME like 'ref_%' or TABLE_NAME like 'vw_%');";
				MySqlCommand command = new MySqlCommand(query, db);
				MySqlDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					string tableName = reader.GetString(0);

					if (!IgnoreTables.Contains(tableName))
						Tables.Add(tableName, new Table());
				}

				IgnoreTables.Clear();
				reader.Close();

				foreach (var item in Tables)
				{
					try
					{
						query = String.Format("SELECT * FROM test_annie.{0};", item.Key);
						command.CommandText = query;
						command.ExecuteNonQuery();
						MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
						dataAdapter.Fill(item.Value.Values);
						item.Value.Values.TableName = item.Key;

						for (int i = 0; i < item.Value.Values.Columns.Count; i++)
							item.Value.Columns.Add(item.Value.Values.Columns[i].Ordinal, item.Value.Values.Columns[i].ColumnName);
					}
					catch (MySqlException)
					{
						IgnoreTables.Add(item.Key);
					}
				}

				foreach (var item in IgnoreTables)
					Tables.Remove(item);
			}
			catch (MySqlException e)
			{
				string message = String.Format("A problem occurred while reading from the database:\n{0}", e.Message);
				throw new DatabaseException(message, e);
			}
		}

		private static void RetrieveStoredProcedures(ref MySqlConnection db)
		{
			try
			{
				string query = "show procedure status;";
				MySqlCommand command = new MySqlCommand(query, db);
				MySqlDataReader reader = command.ExecuteReader();
				List<string> IgnoreProcedures = new List<string>();

				while (reader.Read())
				{
					string database = reader.GetString(0);
					string procedureName = reader.GetString(1);

					if (database == "test_annie" && procedureName.StartsWith("sp_") &&
						!(procedureName.Contains("_assign") || procedureName.Contains("_create") || procedureName.Contains("_load")))
						StoredProcedures.Add(procedureName, new StoredProcedure());
				}

				reader.Close();

				foreach (var item in StoredProcedures)
				{
					try
					{
						query = String.Format("select * from information_schema.parameters where specific_schema = \'{0}\' and specific_name = \'{1}\';",
						"test_annie", item.Key);

						command.CommandText = query;
						reader = command.ExecuteReader();

						while (reader.Read())
						{
							StoredProcedureParameter parameter = new StoredProcedureParameter();
							parameter.Mode = reader.GetString(4);
							parameter.Name = reader.GetString(5);
							parameter.DataType = reader.GetString(6);
							item.Value.Parameters.Add(parameter);
						}

						reader.Close();
						string parameterValues = String.Empty;

						for (int i = 0; i < item.Value.Parameters.Count; i++)
							parameterValues = String.Concat(parameterValues, "\"0\", ");

						parameterValues = parameterValues.TrimEnd(new char[] { ',', ' ' });

						query = String.Format("call test_annie.{0}({1});", item.Key, parameterValues);

						command.CommandText = query;
						reader = command.ExecuteReader();

						for (int i = 0; i < reader.FieldCount; i++)
							item.Value.Columns.Add(i, reader.GetName(i));

						reader.Close();

					}
					catch (MySqlException)
					{
						IgnoreProcedures.Add(item.Key);
					}
				}

				foreach (var item in IgnoreProcedures)
					StoredProcedures.Remove(item);

			}
			catch (MySqlException e)
			{
				string message = String.Format("A problem occurred while reading from the database:\n{0}", e.Message);
				throw new DatabaseException(message, e);
			}
		}
	}

	public class Table
	{
		public Dictionary<int, string> Columns { get; set; }
		public DataTable Values { get; set; }

		public Table()
			: this(new Dictionary<int, string>(), new DataTable())
		{ }

		public Table(Dictionary<int, string> columns, DataTable values)
		{
			this.Columns = columns;
			this.Values = values;
		}
	}

	public class StoredProcedure
	{
		public List<StoredProcedureParameter> Parameters { get; set; }
		public Dictionary<int, string> Columns { get; set; }

		public StoredProcedure()
			: this(new List<StoredProcedureParameter>(), new Dictionary<int, string>())
		{ }

		public StoredProcedure(List<StoredProcedureParameter> parameters, Dictionary<int, string> columns)
		{
			this.Parameters = parameters;
			this.Columns = columns;
		}
	}

	public class StoredProcedureParameter
	{
		public string Mode { get; set; }
		public string Name { get; set; }
		public string DataType { get; set; }

		public StoredProcedureParameter()
			:this(String.Empty, String.Empty, String.Empty)
		{ }

		public StoredProcedureParameter(string mode, string name, string dataType)
		{
			this.Mode = mode;
			this.Name = name;
			this.DataType = dataType;
		}
	}

	[Serializable]
	public class DatabaseException : Exception
	{
		public DatabaseException()
			: base()
		{ }

		public DatabaseException(string message)
			: base(message)
		{ }

		public DatabaseException(string message, Exception inner)
			: base(message, inner)
		{ }
	}

	[Serializable]
	public class TableNotFoundException : DatabaseException
	{
		public TableNotFoundException()
			: base()
		{ }

		public TableNotFoundException(string message)
			: base(message)
		{ }

		public TableNotFoundException(string message, Exception inner)
			: base(message, inner)
		{ }
	}

	[Serializable]
	public class ColumnNotFoundException : DatabaseException
	{
		public ColumnNotFoundException()
			: base()
		{ }

		public ColumnNotFoundException(string message)
			: base(message)
		{ }

		public ColumnNotFoundException(string message, Exception inner)
			: base(message, inner)
		{ }
	}
}
