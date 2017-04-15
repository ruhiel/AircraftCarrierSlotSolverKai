using CsvHelper.Configuration;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class AirCraftMap : CsvClassMap<AirCraft>
    {
        public AirCraftMap()
        {
            Map(m => m.Id).Index(0).Name("装備ID");
            Map(m => m.Type).Index(1).Name("装備種");
            Map(m => m.Name).Index(2).Name("装備名");
            Map(m => m.FirePower).Index(3).Name("火力");
            Map(m => m.Torpedo).Index(4).Name("雷装");
            Map(m => m.AAValue).Index(5).Name("対空");
            Map(m => m.Armor).Index(6).Name("装甲");
            Map(m => m.ASW).Index(7).Name("対潜");
            Map(m => m.Evasion).Index(8).Name("回避");
            Map(m => m.ViewRange).Index(9).Name("索敵");
            Map(m => m.Luck).Index(10).Name("運");
            Map(m => m.Accuracy).Index(11).Name("命中");
            Map(m => m.Bomber).Index(12).Name("爆装");
        }
    }
}
