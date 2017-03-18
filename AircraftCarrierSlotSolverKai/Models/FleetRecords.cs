using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class FleetRecords : CSVRecords<Fleet, FleetMap>
    {
        public static FleetRecords Instance = new FleetRecords();
        
        private FleetRecords(string fileName = "fleets.csv") : base(fileName)
        {
        }
    }
}
