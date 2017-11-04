using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AircraftCarrierSlotSolverKai.Models.Records.CVCIRecords;

namespace AircraftCarrierSlotSolverKai.Models
{
    [JsonObject]
    public class CVCI : BindableBase
    {
        [JsonProperty]
        public CIType Type { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        public bool _IsSelected;
        [JsonProperty]
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }

        public CVCI(CIType type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
