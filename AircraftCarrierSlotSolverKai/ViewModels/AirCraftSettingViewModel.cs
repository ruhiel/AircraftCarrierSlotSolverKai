using AircraftCarrierSlotSolverKai.Models;
using Reactive.Bindings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using AircraftCarrierSlotSolverKai.Models.Records;

namespace AircraftCarrierSlotSolverKai.ViewModels
{
    public class AirCraftSettingViewModel
    {
        public IEnumerable<AirCraft> AirCraftList => AirCraftRecords.Instance.Records;

        public AirCraft NowSelectAirCraft { get; set; }

        public ObservableCollection<AirCraftSetting> AirCraftSettings => AirCraftSettingRecords.Instance.Records;

        public ReactiveCommand AddCommand { get; private set; }

        public ReactiveCommand AirCraftSettingSaveCommand { get; private set; }

        public AirCraftSettingViewModel()
        {
            AddCommand = new ReactiveCommand();

            AddCommand.Subscribe(_ => AirCraftSettingRecords.Instance.Add(NowSelectAirCraft));

            AirCraftSettingSaveCommand = new ReactiveCommand();

            AirCraftSettingSaveCommand.Subscribe(_ => AirCraftSettingRecords.Instance.Save());
        }
    }
}
