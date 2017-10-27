using Dapper;
using System.Data.SQLite;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class SQLRecords
    {
        protected const string DataSource = @"solver.db";

        public SQLRecords()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        protected static SQLiteConnection GetConnection()
        {
            var config = new SQLiteConnectionStringBuilder()
            {
                DataSource = DataSource
            };
            return new SQLiteConnection(config.ToString());
        }
    }
}