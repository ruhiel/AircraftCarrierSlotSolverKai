using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models.Records
{
    public class CVCIRecords
    {
        [Flags]
        public enum CIType
        {
            DIVE_BOMBER = 1 << 1,
            TORPEDO_BOMBER = 1 << 2,
            FIGHTER = 1 << 3,
            DIVE_BOMBER2 = 1 << 4,
            DIVE_BOMBER_TORPEDO_BOMBER_FIGHTER = DIVE_BOMBER | TORPEDO_BOMBER | FIGHTER,
            DIVE_BOMBER_TIVE_BOMBER_FIGHTER = DIVE_BOMBER2 | FIGHTER,
            DIVE_BOMBER_TORPEDO_BOMBER = DIVE_BOMBER | TORPEDO_BOMBER,
        }

        public static CVCIRecords Instance = new CVCIRecords();

        public IEnumerable<CVCI> Records => new CVCI[]
        {
            new CVCI(CIType.DIVE_BOMBER_TORPEDO_BOMBER_FIGHTER, "艦爆・艦攻・艦戦"),
            new CVCI(CIType.DIVE_BOMBER_TIVE_BOMBER_FIGHTER, "艦爆・艦爆・艦戦"),
            new CVCI(CIType.DIVE_BOMBER_TORPEDO_BOMBER, "艦爆・艦攻"),
        };
    }
}
