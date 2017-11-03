using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class Fleet
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public int AirSuperiorityPotential { get; set; }
        public string Organization { get; set; }
        public IEnumerable<ShipSlotInfo> ShipSlotInfo { get; set; }
        public long? World { get; set; }
    }
}
