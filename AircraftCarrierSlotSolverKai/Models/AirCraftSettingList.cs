using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class AirCraftSettingList : ObservableCollection<AirCraftSetting>
    {
        public static AirCraftSettingList Instance = new AirCraftSettingList();

        private AirCraftSettingList()
        {
            AirCraftSettingRecords.Instance.Load();

            foreach (var record in AirCraftSettingRecords.Instance.Records.Where(x => x.AirCraft != null).OrderBy(y => y.AirCraft.TypeOrder))
            {
                Add(record);
            }
        }

        public void Add(AirCraft airCraft) => Add(new AirCraftSetting() { Name = airCraft.Name, Improvement = 0, Value = 1 });

        public void Save()
        {
            AirCraftSettingRecords.Instance.Records.Clear();

            AirCraftSettingRecords.Instance.Records.AddRange(this);

            AirCraftSettingRecords.Instance.Save();
        }
    }
}
