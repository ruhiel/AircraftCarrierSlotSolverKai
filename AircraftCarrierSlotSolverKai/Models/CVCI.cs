using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AircraftCarrierSlotSolverKai.Models.Records.CVCIRecords;

namespace AircraftCarrierSlotSolverKai.Models
{
    [JsonObject]
    public class CVCI
    {
        [JsonProperty]
        public CIType Type { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        public CVCI(CIType type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
