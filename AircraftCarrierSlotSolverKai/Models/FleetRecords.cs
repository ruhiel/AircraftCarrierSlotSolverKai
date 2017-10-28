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
    public class FleetRecords : SQLRecords<Fleet>
    {
        public static FleetRecords Instance = new FleetRecords();

        private FleetRecords() : base()
        {
            foreach(var fleet in Records)
            {
                fleet.ShipSlotInfo = ZeroFormatterSerializer.Deserialize<Fleets>(fleet.Organization).List;
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
                        connection.Execute($"insert into fleet (name, air_superiority_potential, organization) values (@{nameof(fleet.Name)}, @{nameof(fleet.AirSuperiorityPotential)}, @{nameof(fleet.Organization)})", fleet, tran);
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
                        connection.Execute($"delete from fleet where id = @{nameof(nowSelectFleet.ID)}", nowSelectFleet, tran);
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
