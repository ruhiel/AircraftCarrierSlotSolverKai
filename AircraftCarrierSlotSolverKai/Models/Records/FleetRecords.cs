using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models.Records
{
    public class FleetRecords : SQLRecords<Fleet>
    {
        public static FleetRecords Instance = new FleetRecords();

        private FleetRecords() : base()
        {
            foreach(var fleet in Records)
            {
                fleet.ShipSlotInfo = JsonConvert.DeserializeObject<Fleets>(fleet.Organization).List;
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
                            Organization = JsonConvert.SerializeObject(new Fleets()
                            {
                                List = shipSlotInfos.ToList(),
                            }),
                            ShipSlotInfo = shipSlotInfos
                        };
                        connection.Execute($"insert into {TableName} (name, air_superiority_potential, organization) values (@{nameof(fleet.Name)}, @{nameof(fleet.AirSuperiorityPotential)}, @{nameof(fleet.Organization)})", fleet, tran);
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
