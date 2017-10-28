using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class AreaRecords : SQLRecords<Area>
    {
        public static AreaRecords Instance = new AreaRecords();
    }
}
