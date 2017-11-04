using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models.Records
{
    public class CVCIRecords
    {
        public static CVCIRecords Instance = new CVCIRecords();

        public IEnumerable<CVCI> Records => new CVCI[] {new CVCI(0, "艦爆・艦攻・艦戦"), new CVCI(1, "艦爆・艦爆・艦戦"), new CVCI(2, "艦爆・艦攻"), };
    }
}
