using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class EquipableSlot
    {
        /// <summary>
        /// 艦船ID
        /// </summary>
        public int Shipid { get; set; }

        public int AircraftType { get; set; }

        public int SlotIndex { get; set; }
    }
}
