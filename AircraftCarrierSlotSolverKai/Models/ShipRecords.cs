using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class ShipRecords : CSVRecords<Ship, ShipInfoMap>
    {
        public static ShipRecords Instance = new ShipRecords();

        public override List<Ship> GetList(List<Ship> list)
        {
            var types = list.Select(x => x.Type).Distinct().Select((type, idx) => new { type, idx });

            return list.Where(x => x.Prev != "-").OrderBy(y => types.First(z => z.type == y.Type).idx).ToList();
        }

        private ShipRecords(string fileName = "slots.csv") : base(fileName)
        {
        }
    }
}
