using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models.Records
{
    public class AirCraftTypeRecords : SQLRecords<AirCraftType>
    {
        public static AirCraftTypeRecords Instance = new AirCraftTypeRecords();
    }
}
