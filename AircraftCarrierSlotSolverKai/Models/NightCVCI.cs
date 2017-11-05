using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class NightCVCI : BindableBase
    {
        [Flags]
        public enum NightCVCIType
        {
            NIGHT_FIGHTER = 1 << 1,
            NIGHT_BOMBER = 1 << 2,
            BOMBER = 1 << 3,
            NIGHT_FIGHTER2 = 1 << 4,
            NIGHT_FIGHTER3 = 1 << 5,
            BOMBER2 = 1 << 6,
            NIGHT_FIGHTER_NIGHT_FIGHTER_NIGHT_BOMBER = NIGHT_FIGHTER2 | NIGHT_BOMBER,
            NIGHT_FIGHTER_NIGHT_BOMBER = NIGHT_FIGHTER | NIGHT_BOMBER,
            NIGHT_FIGHTER_NIGHT_FIGHTER_NIGHT_FIGHTER = NIGHT_FIGHTER3,
            NIGHT_FIGHTER_NIGHT_FIGHTER_BOMBER = NIGHT_FIGHTER2 | BOMBER,
            NIGHT_FIGHTER_NIGHT_BOMBER_BOMBER = NIGHT_FIGHTER | NIGHT_BOMBER | BOMBER,
            NIGHT_FIGHTER_BOMBER_BOMBER = NIGHT_FIGHTER | BOMBER2,
        }

        [JsonProperty]
        public NightCVCIType Type { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        public bool _IsSelected;
        [JsonProperty]
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }

        public NightCVCI(NightCVCIType type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
