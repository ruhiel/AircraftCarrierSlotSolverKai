using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class AreaRecords : SQLRecords
    {
        public static AreaRecords Instance = new AreaRecords();

        public IEnumerable<Area> Records
        {
            get
            {
                var config = new SQLiteConnectionStringBuilder()
                {
                    DataSource = DataSource
                };
                using (var connection = new SQLiteConnection(config.ToString()))
                {
                    connection.Open();
                    return connection.Query<Area>(@"select * from area");
                }
            }
        }
    }
}
