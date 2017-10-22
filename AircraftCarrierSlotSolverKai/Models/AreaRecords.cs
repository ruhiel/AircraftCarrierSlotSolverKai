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
                var list = new List<Area>();
                using (var con = new SQLiteConnection(ConnectionString))
                {
                    con.Open();

                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT 海域, 敵制空値 FROM area";

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new Area()
                                {
                                    Name = reader.GetString(0),
                                    AirSuperiorityPotential = reader.GetInt32(1)
                                });
                            }
                        }
                    }
                }

                return list;
            }
        }
    }
}
