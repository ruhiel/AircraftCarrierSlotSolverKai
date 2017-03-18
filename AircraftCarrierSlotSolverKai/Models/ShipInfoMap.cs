using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class ShipInfoMap : CsvClassMap<ShipInfo>
    {
        public ShipInfoMap()
        {
            Map(m => m.Name).Index(0).Name("艦娘");
            Map(m => m.Type).Index(1).Name("艦種");
            Map(m => m.FirePower).Index(2).Name("火力");
            Map(m => m.SlotNum).Index(3).Name("スロット数");
            Map(m => m.Slot1Num).Index(4).Name("搭載数1");
            Map(m => m.Slot2Num).Index(5).Name("搭載数2");
            Map(m => m.Slot3Num).Index(6).Name("搭載数3");
            Map(m => m.Slot4Num).Index(7).Name("搭載数4");
        }
    }
}
