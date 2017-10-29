using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models.Records
{
    public class AirCraftRecords : SQLRecords<AirCraft>
    {
        public static AirCraftRecords Instance = new AirCraftRecords();
    }
}
