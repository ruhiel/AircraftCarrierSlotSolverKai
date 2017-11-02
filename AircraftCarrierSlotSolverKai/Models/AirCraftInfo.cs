using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    [JsonObject]
    public class AirCraftInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty] public int AircraftId { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty] public int Improvement { get; set; }

        /// <summary>
        /// 艦載機
        /// </summary>
        [JsonIgnore] public AirCraft AirCraft => new AirCraft(AircraftId, Improvement);

        public AirCraftInfo()
        {

        }

        public AirCraftInfo(int id, int improvement)
        {
            AircraftId = id;
            Improvement = improvement;
        }
    }
}
