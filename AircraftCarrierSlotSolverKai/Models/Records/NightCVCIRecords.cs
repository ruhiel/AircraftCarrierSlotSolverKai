using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AircraftCarrierSlotSolverKai.Models.NightCVCI;

namespace AircraftCarrierSlotSolverKai.Models.Records
{
    class NightCVCIRecords
    {
        public static NightCVCIRecords Instance = new NightCVCIRecords();

        public IEnumerable<NightCVCI> Records => new NightCVCI[]
        {
            new NightCVCI(NightCVCIType.NIGHT_FIGHTER_NIGHT_FIGHTER_NIGHT_BOMBER, "夜戦・夜戦・夜攻"),
            new NightCVCI(NightCVCIType.NIGHT_FIGHTER_NIGHT_BOMBER, "夜戦・夜攻"),
            new NightCVCI(NightCVCIType.NIGHT_FIGHTER_NIGHT_FIGHTER_NIGHT_FIGHTER, "夜戦・夜戦・夜戦"),
            new NightCVCI(NightCVCIType.NIGHT_FIGHTER_NIGHT_FIGHTER_BOMBER, "夜戦・夜戦・その他"),
            new NightCVCI(NightCVCIType.NIGHT_FIGHTER_NIGHT_BOMBER_BOMBER, "夜戦・夜攻・その他"),
            new NightCVCI(NightCVCIType.NIGHT_FIGHTER_BOMBER_BOMBER, "夜戦・その他・その他"),
        };
    }
}
