using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models.Records
{
    public class ShipTypeRecords : SQLRecords<ShipType>
    {
        public static ShipTypeRecords Instance = new ShipTypeRecords();
    }
}
