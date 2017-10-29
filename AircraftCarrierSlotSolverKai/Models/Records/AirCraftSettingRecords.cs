using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models.Records
{
    public class AirCraftSettingRecords : SQLRecords<AirCraftSetting>
    {
        public static AirCraftSettingRecords Instance = new AirCraftSettingRecords();

        private AirCraftSettingRecords() : base(x => x.OrderBy(y => y.AirCraft.Type))
        {
        }

        public void Add(AirCraft airCraft) => Records.Add(new AirCraftSetting() { AircraftId = airCraft.Id, Improvement = airCraft.Improvement, Value = 0 });
        public void Save()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute($"delete from {TableName}", tran);

                        foreach(var record in Records)
                        {
                            connection.Execute($"insert into {TableName} (aircraft_id, improvement, value) values (@{nameof(record.AircraftId)}, @{nameof(record.Improvement)}, @{nameof(record.Value)})", record, tran);
                        }
                        tran.Commit();
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
