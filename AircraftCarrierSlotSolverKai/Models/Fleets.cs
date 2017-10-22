using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;

namespace AircraftCarrierSlotSolverKai.Models
{
    [ZeroFormattable]
    public class Fleets
    {
        [Index(0)] public virtual IList<ShipSlotInfo> List { get; set; }
    }
}
