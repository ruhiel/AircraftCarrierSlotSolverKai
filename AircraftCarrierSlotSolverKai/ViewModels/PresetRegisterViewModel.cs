using AircraftCarrierSlotSolverKai.Models;
using AircraftCarrierSlotSolverKai.Models.Records;
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
        public IEnumerable<World> WorldList => WorldRecords.Instance.Records;

        public World NowSelectWorld { get; set; }

        public ReactiveProperty<string> FleetName { get; private set; } = new ReactiveProperty<string>();
    }
}
