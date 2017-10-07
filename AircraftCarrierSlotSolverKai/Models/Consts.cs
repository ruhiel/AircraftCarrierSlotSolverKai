using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class Consts
    {
        public const string Fighter = "艦上戦闘機";

        public const string DiveBomber = "艦上爆撃機";

        public const string TorpedoBomber = "艦上攻撃機";

        public const string JetBomber = "噴式戦闘爆撃機";

        public const string ReconAircraft = "艦上偵察機";

        public const string SeaplaneBomber = "水上爆撃機";

        public const string ReconnaissanceSeaplane = "水上偵察機";

        public const string SeaplaneFighter = "水上戦闘機";

        public const string AviationPersonnel = "航空要員";

        public static readonly Dictionary<string, int> ShipTypeOrder = new Dictionary<string, int>()
        {
            {"正規空母", 0},
            {"装甲空母", 1},
            {"軽空母", 2},
            {"水上機母艦", 3},
            {"航空巡洋艦", 4},
            {"航空戦艦", 5},
            {"潜水空母", 6},
            {"揚陸艦", 7},
            {"補給艦", 8},
        };
    }
}
