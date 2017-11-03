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
                fleet.ShipSlotInfo = JsonConvert.DeserializeObject<IEnumerable<ShipSlotInfo>>(fleet.Organization);
            }
        }

        public void Add(string fleetName, int airSuperiorityPotential, IEnumerable<ShipSlotInfo> shipSlotInfos, long? worldId)
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
                            Organization = JsonConvert.SerializeObject(shipSlotInfos),
                            ShipSlotInfo = shipSlotInfos,
                            World = worldId
                        };
                        connection.Execute($"insert into {TableName} (name, air_superiority_potential, organization, world) values (@{nameof(fleet.Name)}, @{nameof(fleet.AirSuperiorityPotential)}, @{nameof(fleet.Organization)}, @{nameof(fleet.World)})", fleet, tran);
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
