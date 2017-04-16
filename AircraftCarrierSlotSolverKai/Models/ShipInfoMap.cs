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
            Map(m => m.ID).Index(0).Name("艦船ID");
            Map(m => m.Type).Index(1).Name("艦種");
            Map(m => m.Name).Index(2).Name("艦名");
            Map(m => m.Prev).Index(3).Name("改装前");
            Map(m => m.FirePower).Index(4).Name("火力最大");
            Map(m => m.SlotNum).Index(5).Name("スロット数");
            Map(m => m.Slot1Num).Index(6).Name("搭載機数1");
            Map(m => m.Slot2Num).Index(7).Name("搭載機数2");
            Map(m => m.Slot3Num).Index(8).Name("搭載機数3");
            Map(m => m.Slot4Num).Index(9).Name("搭載機数4");
        }
    }
}
