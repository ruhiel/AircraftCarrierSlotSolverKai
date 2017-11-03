using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models.Records
{
    public class WorldRecords : SQLRecords<World>
    {
        public static WorldRecords Instance = new WorldRecords();

        public IEnumerable<World> WithDummyList => new World[] { new World() { ID = -1 } }.Concat(Records);
    }
}
