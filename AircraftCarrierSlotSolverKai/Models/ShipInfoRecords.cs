using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class ShipInfoRecords : CSVRecords<ShipInfo, ShipInfoMap>
    {
        public static ShipInfoRecords Instance = new ShipInfoRecords();

        public override List<ShipInfo> GetList(List<ShipInfo> list)
        {
            var types = list.Select(x => x.Type).Distinct().Select((type, idx) => new { type, idx });

            return list.Where(x => x.Prev != "-").OrderBy(y => types.First(z => z.type == y.Type).idx).ToList();
        }

        private ShipInfoRecords(string fileName = "slots.csv") : base(fileName)
        {
        }
    }
}
