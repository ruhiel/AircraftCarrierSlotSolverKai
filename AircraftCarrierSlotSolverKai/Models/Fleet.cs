using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class Fleet
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int AirSuperiorityPotential { get; set; }
        public byte[] FleetBin { get; set; }
        public IEnumerable<ShipSlotInfo> ShipSlotInfo { get; set; }
    }
}
