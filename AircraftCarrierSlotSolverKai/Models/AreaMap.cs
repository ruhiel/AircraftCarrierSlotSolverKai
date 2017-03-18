using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class AreaMap : CsvClassMap<Area>
    {
        public AreaMap()
        {
            Map(m => m.Name).Index(0).Name("海域");
            Map(m => m.AirSuperiorityPotential).Index(1).Name("敵制空値");
        }
    }
}
