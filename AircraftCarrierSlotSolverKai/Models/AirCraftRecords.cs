using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class AirCraftRecords : CSVRecords<AirCraft, AirCraftMap>
    {
        public static AirCraftRecords Instance = new AirCraftRecords();

        private AirCraftRecords(string fileName = "air.csv") : base(fileName)
        {
        }
    }
}
