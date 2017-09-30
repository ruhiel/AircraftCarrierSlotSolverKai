using AircraftCarrierSlotSolverKai.Models;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Events;
using AircraftCarrierSlotSolverKai.Views;
using Reactive.Bindings.Extensions;
using System.Reactive.Linq;

namespace AircraftCarrierSlotSolverKai.ViewModels
{
    public class MainWindowViewModel
    {
        public List<ShipInfo> ShipList => ShipInfoRecords.Instance.Records;

        public List<Area> AreaList => AreaRecords.Instance.Records;

        public ShipInfo NowSelectShip { get; set; }

        public Area NowSelectArea { get; set; }

        public Fleet NowSelectFleet { get; set; }

        [Int("整数を入力してください")]
        public ReactiveProperty<string> TargetAirSuperiorityPotential { get; }

        public ObservableCollection<ShipSlotInfoViewModel> ShipSlotInfoList { get; set; }

        public ObservableCollection<Fleet> FleetList => Models.FleetList.Instance;

        public ReactiveCommand ShipAddCommand { get; } = new ReactiveCommand();

        public ReactiveCommand CalcCommand { get; }

        public ReactiveCommand AirCraftSettingCommand { get; } = new ReactiveCommand();

        public ReactiveCommand SuperiorityCommand { get; } = new ReactiveCommand();

        public ReactiveCommand EnsureCommand { get; } = new ReactiveCommand();

        public ReactiveCommand PresetCommand { get; } = new ReactiveCommand();

        public ReactiveCommand PresetViewCommand { get; } = new ReactiveCommand();

        public ReactiveCommand PresetDeleteCommand { get; } = new ReactiveCommand();

        public ReactiveProperty<bool> GridVisible { get; } = new ReactiveProperty<bool>(true);

        public ReactiveProperty<bool> ProgressVisible { get; }

        public MainWindowViewModel()
        {
            ShipSlotInfoList = new ObservableCollection<ShipSlotInfoViewModel>();

            ShipAddCommand.Subscribe(_ =>
            {
                if (NowSelectShip != null)
                {
                    ShipSlotInfoList.Add(new ShipSlotInfoViewModel(NowSelectShip));
                }
            });

            TargetAirSuperiorityPotential = new ReactiveProperty<string>(default(int).ToString()).SetValidateAttribute(() => TargetAirSuperiorityPotential);

            CalcCommand = new[] { TargetAirSuperiorityPotential.ObserveHasErrors}.CombineLatestValuesAreAllFalse().ToReactiveCommand();
            CalcCommand.Subscribe(_ => 
            {
                GridVisible.Value = false;
                Messenger.Instance.GetEvent<PubSubEvent<(bool result, string message)>>().Publish(Calculator.Calc(int.Parse(TargetAirSuperiorityPotential.Value), ShipSlotInfoList.Select(x => x.ShipSlotInfo)));
                GridVisible.Value = true;
            });

            AirCraftSettingCommand.Subscribe(_ =>
            {
                new AirCraftSettingView().ShowDialog();
            });

            SuperiorityCommand.Subscribe(_ => TargetAirSuperiorityPotential.Value = (NowSelectArea == null ? TargetAirSuperiorityPotential.Value : (int.Parse(Properties.Settings.Default.MarginAirSuperiorityPotential) + (int)(NowSelectArea.AirSuperiorityPotential * 1.5)).ToString()));

            EnsureCommand.Subscribe(_ => TargetAirSuperiorityPotential.Value = (NowSelectArea == null ? TargetAirSuperiorityPotential.Value : (int.Parse(Properties.Settings.Default.MarginAirSuperiorityPotential) + NowSelectArea.AirSuperiorityPotential * 3).ToString()));

            PresetCommand.Subscribe(_ =>
            {
                var dialog = new PresetRegisterView();

                dialog.ShowDialog();

                Models.FleetList.Instance.Add(dialog.FleetName, int.Parse(TargetAirSuperiorityPotential.Value), ShipSlotInfoList.Select(x => x.ShipSlotInfo));
            });

            PresetViewCommand.Subscribe(_ =>
            {
                if (NowSelectFleet == null)
                {
                    return;
                }

                ShipSlotInfoList.Clear();

                var ships = new[] 
                {
                    new { Name = NowSelectFleet.Ship1Name, Slot1 = new {Name = NowSelectFleet.Ship1Slot1, Improvement = NowSelectFleet.Ship1Slot1Improvement}, Slot2 = new {Name = NowSelectFleet.Ship1Slot2, Improvement = NowSelectFleet.Ship1Slot2Improvement}, Slot3 = new {Name = NowSelectFleet.Ship1Slot3, Improvement = NowSelectFleet.Ship1Slot3Improvement}, Slot4 = new {Name = NowSelectFleet.Ship1Slot4, Improvement = NowSelectFleet.Ship1Slot4Improvement} },
                    new { Name = NowSelectFleet.Ship2Name, Slot1 = new {Name = NowSelectFleet.Ship2Slot1, Improvement = NowSelectFleet.Ship2Slot1Improvement}, Slot2 = new {Name = NowSelectFleet.Ship2Slot2, Improvement = NowSelectFleet.Ship2Slot2Improvement}, Slot3 = new {Name = NowSelectFleet.Ship2Slot3, Improvement = NowSelectFleet.Ship2Slot3Improvement}, Slot4 = new {Name = NowSelectFleet.Ship2Slot4, Improvement = NowSelectFleet.Ship2Slot4Improvement} },
                    new { Name = NowSelectFleet.Ship3Name, Slot1 = new {Name = NowSelectFleet.Ship3Slot1, Improvement = NowSelectFleet.Ship3Slot1Improvement}, Slot2 = new {Name = NowSelectFleet.Ship3Slot2, Improvement = NowSelectFleet.Ship3Slot2Improvement}, Slot3 = new {Name = NowSelectFleet.Ship3Slot3, Improvement = NowSelectFleet.Ship3Slot3Improvement}, Slot4 = new {Name = NowSelectFleet.Ship3Slot4, Improvement = NowSelectFleet.Ship3Slot4Improvement} },
                    new { Name = NowSelectFleet.Ship4Name, Slot1 = new {Name = NowSelectFleet.Ship4Slot1, Improvement = NowSelectFleet.Ship4Slot1Improvement}, Slot2 = new {Name = NowSelectFleet.Ship4Slot2, Improvement = NowSelectFleet.Ship4Slot2Improvement}, Slot3 = new {Name = NowSelectFleet.Ship4Slot3, Improvement = NowSelectFleet.Ship4Slot3Improvement}, Slot4 = new {Name = NowSelectFleet.Ship4Slot4, Improvement = NowSelectFleet.Ship4Slot4Improvement} },
                    new { Name = NowSelectFleet.Ship5Name, Slot1 = new {Name = NowSelectFleet.Ship5Slot1, Improvement = NowSelectFleet.Ship5Slot1Improvement}, Slot2 = new {Name = NowSelectFleet.Ship5Slot2, Improvement = NowSelectFleet.Ship5Slot2Improvement}, Slot3 = new {Name = NowSelectFleet.Ship5Slot3, Improvement = NowSelectFleet.Ship5Slot3Improvement}, Slot4 = new {Name = NowSelectFleet.Ship5Slot4, Improvement = NowSelectFleet.Ship5Slot4Improvement} },
                    new { Name = NowSelectFleet.Ship6Name, Slot1 = new {Name = NowSelectFleet.Ship6Slot1, Improvement = NowSelectFleet.Ship6Slot1Improvement}, Slot2 = new {Name = NowSelectFleet.Ship6Slot2, Improvement = NowSelectFleet.Ship6Slot2Improvement}, Slot3 = new {Name = NowSelectFleet.Ship6Slot3, Improvement = NowSelectFleet.Ship6Slot3Improvement}, Slot4 = new {Name = NowSelectFleet.Ship6Slot4, Improvement = NowSelectFleet.Ship6Slot4Improvement} }
                };
                
                foreach(var ship in ships.Where(x => !string.IsNullOrEmpty(x.Name)))
                {
                    var slot1 = AirCraftSettingRecords.Instance.Records.FirstOrDefault(x => x.Name == ship.Slot1.Name && x.Improvement == ship.Slot1.Improvement);
                    var slot2 = AirCraftSettingRecords.Instance.Records.FirstOrDefault(x => x.Name == ship.Slot2.Name && x.Improvement == ship.Slot2.Improvement);
                    var slot3 = AirCraftSettingRecords.Instance.Records.FirstOrDefault(x => x.Name == ship.Slot3.Name && x.Improvement == ship.Slot3.Improvement);
                    var slot4 = AirCraftSettingRecords.Instance.Records.FirstOrDefault(x => x.Name == ship.Slot4.Name && x.Improvement == ship.Slot4.Improvement);

                    var vm = new ShipSlotInfoViewModel(ShipInfoRecords.Instance.Records.First(x => x.Name == ship.Name), new AirCraft(slot1), new AirCraft(slot2), new AirCraft(slot3), new AirCraft(slot4));

                    ShipSlotInfoList.Add(vm);
                }

                TargetAirSuperiorityPotential.Value = NowSelectFleet.AirSuperiorityPotential.ToString();
            });

            PresetDeleteCommand.Subscribe(_ =>
            {
                Models.FleetList.Instance.Remove(NowSelectFleet);

                Models.FleetList.Instance.Save();

                NowSelectFleet = null;
            });

            ProgressVisible = GridVisible.Select(x => !x).ToReactiveProperty();
        }

        ~MainWindowViewModel()
        {
            Properties.Settings.Default.Save();
        }
    }
}
