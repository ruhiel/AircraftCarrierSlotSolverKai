using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class ShipInfoRecords : CSVRecords<ShipInfo, ShipInfoMap>
    {
        public static ShipInfoRecords Instance = new ShipInfoRecords();

        private ShipInfoRecords(string fileName = "slots.csv") : base(fileName)
        {
        }
    }
}
