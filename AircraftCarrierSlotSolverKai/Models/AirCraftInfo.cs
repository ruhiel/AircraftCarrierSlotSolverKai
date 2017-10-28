using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;

namespace AircraftCarrierSlotSolverKai.Models
{
    [ZeroFormattable]
    public class AirCraftInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        [Index(0)] public virtual int AircraftId { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        [Index(1)] public virtual int Improvement { get; set; }

        /// <summary>
        /// 艦載機
        /// </summary>
        [IgnoreFormat] public AirCraft AirCraft => new AirCraft(AircraftId, Improvement);

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
