using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class AirCraftMap : CsvClassMap<AirCraft>
    {
        public AirCraftMap()
        {
            Map(m => m.Name).Index(0).Name("名称");
            Map(m => m.Type).Index(1).Name("種類");
            Map(m => m.FirePower).Index(2).Name("火力");
            Map(m => m.AAValue).Index(3).Name("対空");
            Map(m => m.Bomber).Index(4).Name("爆装");
            Map(m => m.Torpedo).Index(5).Name("雷装");
            Map(m => m.Accuracy).Index(6).Name("命中");
            Map(m => m.Evasion).Index(7).Name("回避");
        }
    }
}
