using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class AirCraftSettingRecords : CSVRecords<AirCraftSetting, AirCraftSettingMap>
    {
        public static AirCraftSettingRecords Instance = new AirCraftSettingRecords();

        public AirCraftSettingRecords(string fileName = "aircraftsetting.csv") : base(fileName)
        {
        }
    }
}
