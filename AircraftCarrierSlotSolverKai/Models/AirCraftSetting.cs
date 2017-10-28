using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class AirCraftSetting
    {
        /// <summary>
        /// ID
        /// </summary>
        public int AircraftId { get; set; }

        /// <summary>
        /// 艦載機名
        /// </summary>
        public string Name => AirCraft.Name;

        /// <summary>
        /// 艦載機
        /// </summary>
        public AirCraft AirCraft
        {
            get
            {
                var aircraft = new AirCraft(AirCraftRecords.Instance.Records.SingleOrDefault(x => x.Id == AircraftId));
                if(aircraft == null)
                {
                    return null;
                }

                aircraft.Improvement = Improvement;
                return aircraft;
            }
        }
        

        /// <summary>
        /// 改修値
        /// </summary>
        public int Improvement { get; set; }

        /// <summary>
        /// 所持数
        /// </summary>
        public int Value { get; set; }
    }
}
