using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework;

namespace Tests
{
	[TestClass]
	public class DatabaseTest
	{
		[TestMethod]
		public void MeasureDatabaseTest()
		{
			string table = "sp_ooh_pit_rates";

			Database.PopulateMeasure(table);
		}
	}
}
