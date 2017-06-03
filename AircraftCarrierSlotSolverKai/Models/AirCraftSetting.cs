using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class AirCraftSetting
    {
        public string Name { get; set; }

        public AirCraft AirCraft
        {
            get
            {
                var aircraft = new AirCraft(AirCraftRecords.Instance.Records.SingleOrDefault(x => x.Name == Name));
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

        public int Value { get; set; }
    }
}
