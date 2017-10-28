using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class ShipRecords : SQLRecords<Ship>
    {
        public static ShipRecords Instance = new ShipRecords();

        private ShipRecords() : base(x => x
                    .OrderBy(y => string.IsNullOrEmpty(y.PrevRemodel))
                    .ThenBy(z => z.ID))
        {

        }
    }
}
