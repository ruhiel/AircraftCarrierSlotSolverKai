using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models.Records
{
    public class EquipableSlotRecord : SQLRecords<EquipableSlot>
    {
        public static EquipableSlotRecord Instance = new EquipableSlotRecord();
    }
}
