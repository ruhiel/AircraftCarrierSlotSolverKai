using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class AreaRecords : CSVRecords<Area, AreaMap>
    {
        public static AreaRecords Instance = new AreaRecords();
        private AreaRecords(string fileName = "area.csv") : base(fileName)
        {
        }
    }
}
