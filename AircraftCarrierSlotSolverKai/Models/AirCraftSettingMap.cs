using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class AirCraftSettingMap : CsvClassMap<AirCraftSetting>
    {
        public AirCraftSettingMap()
        {
            Map(m => m.Name).Index(0).Name("艦載機");
            Map(m => m.Improvement).Index(1).Name("改修値");
            Map(m => m.Value).Index(2).Name("所持数");
        }
    }
}
