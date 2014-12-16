using MySql.Data.MySqlClient;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Framework
{
	public static class Database
	{
		private static string hostName = "pocweb.cac.washington.edu";
		private static int sshPort = 22;
		private static string _userName = String.Empty;
		private static string _password = String.Empty;
		private static string dbHost = "localhost";
		private static int dbPort = 3306;
		private static string dbUid = "test_annie";
		private static string dbPassword = "b4Rxx:pW";
		private static string connectionString = String.Format("HOST={0};PORT={1};UID={2};PASSWORD={3}", dbHost, dbPort, dbUid, dbPassword);

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

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public static void PopulateFilterParameter(string table, ref Dictionary<int, string> columns, ref Dictionary<int, string> keys)
		{
			using (SshClient client = new SshClient(hostName, sshPort, UserName, Password))
			{
				try
				{
					string keysQuery = String.Format("SELECT * FROM test_annie.{0} ORDER BY 1;", table);
					string columnsQuery = String.Format(
						"SELECT ORDINAL_POSITION, COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_NAME = \'{0}\' ORDER BY ORDINAL_POSITION;", table);

					client.Connect();

					ForwardedPort forwardedPort = new ForwardedPortLocal(dbHost, (uint)dbPort, hostName, (uint)dbPort);
					client.AddForwardedPort(forwardedPort);
					forwardedPort.Start();

					MySqlConnection db = new MySqlConnection(connectionString);
					db.Open();

					MySqlCommand command = new MySqlCommand(columnsQuery, db);
					MySqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						int key = int.Parse(reader.GetValue(0).ToString());
						string value = reader.GetValue(1).ToString();
						columns.Add(key, value);
					}

					reader.Close();

					command = new MySqlCommand(keysQuery, db);
					reader = command.ExecuteReader();

					while (reader.Read())
					{
						int key = reader.GetInt32(0);
						string value = reader.GetString(1);
						keys.Add(key, value);
					}

					reader.Close();
					db.Close();
				}
				catch (MySqlException e)
				{
					string message = String.Format("A problem occurred while reading from the database:\n{0}", e.Message);
					throw new DatabaseException(message, e);
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public static Dictionary<int, string> PopulateMeasure(string table)
		{
			Dictionary<int, StoredProcedureParameter> parameters = new Dictionary<int, StoredProcedureParameter>();
			Dictionary<int, string> columns = new Dictionary<int, string>();
			string selectQuery = String.Empty;

			using (SshClient client = new SshClient(hostName, sshPort, UserName, Password))
			{
				try
				{
					string procedureQuery = String.Format("select * from information_schema.parameters where specific_schema = \'{0}\' and specific_name = \'{1}\';",
						"test_annie", table);

					client.Connect();

					ForwardedPort forwardedPort = new ForwardedPortLocal(dbHost, (uint)dbPort, hostName, (uint)dbPort);
					client.AddForwardedPort(forwardedPort);
					forwardedPort.Start();

					MySqlConnection db = new MySqlConnection(connectionString);
					db.Open();

					MySqlCommand command = new MySqlCommand(procedureQuery, db);
					MySqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						StoredProcedureParameter parameter = new StoredProcedureParameter();
						int ordinal = reader.GetInt32(3);
						parameter.Mode = reader.GetString(4);
						parameter.Name = reader.GetString(5);
						parameter.DataType = reader.GetString(6);
						parameters.Add(ordinal, parameter);
					}

					reader.Close();
					string parameterValues = String.Empty;

					foreach(var item in parameters)
						parameterValues = String.Concat(parameterValues, "\"0\", ");

					parameterValues = parameterValues.TrimEnd(new char[] { ',', ' ' });

					string procedureCall = String.Format("call test_annie.{0}({1});", table, parameterValues);

					command = new MySqlCommand(procedureCall, db);
					reader = command.ExecuteReader();

					for (int i = 0; i < reader.FieldCount; i++)
						columns.Add(i, reader.GetName(i));

					reader.Close();
					db.Close();
				}
				catch (MySqlException e)
				{
					string message = String.Format("A problem occurred while reading from the database:\n{0}", e.Message);
					throw new DatabaseException(message, e);
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

			return columns;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public static List<string> GetStoredProcedureList()
		{
			List<string> storedProcedures = new List<string>();

			using (SshClient client = new SshClient(hostName, sshPort, UserName, Password))
			{
				try
				{
					string proceduresQuery = String.Format("show procedure status;");

					client.Connect();

					ForwardedPort forwardedPort = new ForwardedPortLocal(dbHost, (uint)dbPort, hostName, (uint)dbPort);
					client.AddForwardedPort(forwardedPort);
					forwardedPort.Start();

					MySqlConnection db = new MySqlConnection(connectionString);
					db.Open();

					MySqlCommand command = new MySqlCommand(proceduresQuery, db);
					MySqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						string database = reader.GetString(0);
						string procedureName = reader.GetString(1);

						if (database == "test_annie" && procedureName.StartsWith("sp_"))
							storedProcedures.Add(procedureName);
					}

					reader.Close();
					db.Close();
				}
				catch (MySqlException e)
				{
					string message = String.Format("A problem occurred while reading from the database:\n{0}", e.Message);
					throw new DatabaseException(message, e);
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

			return storedProcedures;
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
}
