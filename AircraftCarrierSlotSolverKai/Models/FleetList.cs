using Dapper;
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

            using (var connection = GetConnection())
            {
                connection.Open();
                foreach(var fleet in connection.Query<Fleet>(@"select * from fleet"))
                {
                    fleet.ShipSlotInfo = ZeroFormatterSerializer.Deserialize<Fleets>(fleet.Organization).List;

                    Records.Add(fleet);
                }
            }
        }

        public void Add(string fleetName, int airSuperiorityPotential, IEnumerable<ShipSlotInfo> shipSlotInfos)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        var fleet = new Fleet()
                        {
                            Name = fleetName,
                            AirSuperiorityPotential = airSuperiorityPotential,
                            Organization = ZeroFormatterSerializer.Serialize(new Fleets()
                            {
                                List = shipSlotInfos.ToList(),
                            }),
                            ShipSlotInfo = shipSlotInfos
                        };
                        connection.Execute("insert into fleet(name, air_superiority_potential, organization) values (@Name, @AirSuperiorityPotential, @Organization)", fleet, tran);
                        tran.Commit();

                        Records.Add(fleet);
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                    }
                }
            }
        }

        public void Remove(Fleet nowSelectFleet)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute("delete from fleet where id=@id", nowSelectFleet, tran);
                        tran.Commit();

                        Records.Remove(nowSelectFleet);
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                    }
                }
            }
        }
    }
}
