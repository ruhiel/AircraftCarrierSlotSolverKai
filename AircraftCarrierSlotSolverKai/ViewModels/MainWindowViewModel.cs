﻿using AircraftCarrierSlotSolverKai.Models;
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
        public IEnumerable<string> ShipTypeList => ShipInfoRecords.Instance.Records.Select(x => x.Type).Distinct().OrderBy(y => Consts.ShipTypeOrder[y]);

        public ReactiveProperty<string> NowSelectShipType { get; set; } = new ReactiveProperty<string>("正規空母");

        public ObservableCollection<ShipInfo> ShipList { get; set; } = new ObservableCollection<ShipInfo>();

        public IEnumerable<Area> AreaList => AreaRecords.Instance.Records;

        public ShipInfo NowSelectShip { get; set; }

        public Area NowSelectArea { get; set; }

        public Fleet NowSelectFleet { get; set; }

        [Int("整数を入力してください")]
        public ReactiveProperty<string> TargetAirSuperiorityPotential { get; }

        public ObservableCollection<ShipSlotInfoViewModel> ShipSlotInfoList { get; set; }

        public ObservableCollection<Fleet> FleetList => Models.FleetRecords.Instance.Records;

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
            CalcCommand.Subscribe(async _ => 
            {
                GridVisible.Value = false;
                Messenger.Instance.GetEvent<PubSubEvent<(bool result, string message)>>().Publish(await Calculator.Calc(int.Parse(TargetAirSuperiorityPotential.Value), ShipSlotInfoList.Select(x => x.ShipSlotInfo)));
                GridVisible.Value = true;
            });

            AirCraftSettingCommand.Subscribe(_ =>
            {
                new AirCraftSettingView().ShowDialog();
            });

            SuperiorityCommand.Subscribe(_ => TargetAirSuperiorityPotential.Value = (NowSelectArea == null ? TargetAirSuperiorityPotential.Value : (int.Parse(Properties.Settings.Default.MarginAirSuperiorityPotential) + (int)(NowSelectArea.AirSuperiorityPotential * 1.5)).ToString()));

            EnsureCommand.Subscribe(_ => TargetAirSuperiorityPotential.Value = (NowSelectArea == null ? TargetAirSuperiorityPotential.Value : (int.Parse(Properties.Settings.Default.MarginAirSuperiorityPotential) + NowSelectArea.AirSuperiorityPotential * 3).ToString()));

            // 編成追加
            PresetCommand.Subscribe(_ =>
            {
                var dialog = new PresetRegisterView();

                dialog.ShowDialog();

                Models.FleetRecords.Instance.Add(dialog.FleetName, int.Parse(TargetAirSuperiorityPotential.Value), ShipSlotInfoList.Select(x => x.ShipSlotInfo));
            });

            // 編成展開
            PresetViewCommand.Subscribe(_ =>
            {
                if (NowSelectFleet == null)
                {
                    return;
                }

                ShipSlotInfoList.Clear();

                foreach(var ship in NowSelectFleet.ShipSlotInfo)
                {
                    var vm = new ShipSlotInfoViewModel(ship);

                    ShipSlotInfoList.Add(vm);
                }

                TargetAirSuperiorityPotential.Value = NowSelectFleet.AirSuperiorityPotential.ToString();
            });

            PresetDeleteCommand.Subscribe(_ =>
            {
                Models.FleetRecords.Instance.Remove(NowSelectFleet);

                /// TODO:
                NowSelectFleet = null;
            });

            ProgressVisible = GridVisible.Select(x => !x).ToReactiveProperty();

            NowSelectShipType.Subscribe(type =>
            {
                ShipList.Clear();

                foreach (var item in ShipInfoRecords.Instance.Records.Where(x => string.IsNullOrEmpty(type) ? true : x.Type == type))
                {
                    ShipList.Add(item);
                }
            });
        }

        ~MainWindowViewModel()
        {
            Properties.Settings.Default.Save();
        }
    }
}
