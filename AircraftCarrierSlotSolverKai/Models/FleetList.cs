using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class FleetList : SQLRecords
    {
        public static FleetList Instance = new FleetList();

        public ObservableCollection<Fleet> Records { get; set; }

        private FleetList()
        {
            Records = new ObservableCollection<Fleet>();
            using (var con = new SQLiteConnection(ConnectionString))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = @"SELECT ID, 艦隊名, 制空値, 編成 FROM fleet ORDER BY ID";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var fleet = new Fleet()
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                AirSuperiorityPotential = reader.GetInt32(2)
                            };

                            var bin = (byte[])reader.GetValue(3);

                            fleet.FleetBin = bin;

                            fleet.ShipSlotInfo = ZeroFormatterSerializer.Deserialize<Fleets>(bin).List;

                            Records.Add(fleet);
                        }
                    }
                }
            }
        }

        public void Add(string fleetName, int airSuperiorityPotential, IEnumerable<ShipSlotInfo> shipSlotInfos)
        {
            using (var con = new SQLiteConnection(ConnectionString))
            {
                var fleet = new Fleet()
                {
                    Name = fleetName,
                    AirSuperiorityPotential = airSuperiorityPotential,
                    FleetBin = ZeroFormatterSerializer.Serialize(new Fleets()
                    {
                        List = shipSlotInfos.ToList(),
                    }),
                    ShipSlotInfo = shipSlotInfos
                };

                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO fleet (艦隊名, 制空値, 編成) VALUES (@name, @airSuperiorityPotential, @bin)";
                    cmd.Parameters.Add(new SQLiteParameter("@name", fleet.Name));
                    cmd.Parameters.Add(new SQLiteParameter("@airSuperiorityPotential", fleet.AirSuperiorityPotential));
                    cmd.Parameters.Add(new SQLiteParameter("@bin", fleet.FleetBin));

                    cmd.ExecuteNonQuery();
                }

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT last_insert_rowid() FROM fleet";
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        fleet.ID = reader.GetInt32(0);
                    }
                }

                Records.Add(fleet);
            }
        }

        public void Remove(Fleet nowSelectFleet)
        {
            Records.Remove(nowSelectFleet);

            using (var con = new SQLiteConnection(ConnectionString))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"DELETE FROM fleet WHERE ID = @id";
                    cmd.Parameters.Add(new SQLiteParameter("@id", nowSelectFleet.ID));

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
