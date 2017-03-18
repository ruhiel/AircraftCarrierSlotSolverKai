using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.ViewModels
{
    public class PresetRegisterViewModel
    {
        public ReactiveProperty<string> FleetName { get; private set; } = new ReactiveProperty<string>();
    }
}
