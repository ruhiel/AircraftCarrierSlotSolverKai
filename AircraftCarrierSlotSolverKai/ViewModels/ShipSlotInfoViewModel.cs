using AircraftCarrierSlotSolverKai.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AircraftCarrierSlotSolverKai.ViewModels
{
    public class ShipSlotInfoViewModel
    {
        public ShipSlotInfo ShipSlotInfo { get; private set; }

        public ReadOnlyReactiveProperty<string> ShipName { get; private set; }

        public ReactiveProperty<AirCraft> Slot1 { get; private set; }

        public ReactiveProperty<AirCraft> Slot2 { get; private set; }

        public ReactiveProperty<AirCraft> Slot3 { get; private set; }

        public ReactiveProperty<AirCraft> Slot4 { get; private set; }

        public ReactiveProperty<int> Slot1Num { get; private set; }

        public ReactiveProperty<int> Slot2Num { get; private set; }

        public ReactiveProperty<int> Slot3Num { get; private set; }

        public ReactiveProperty<int> Slot4Num { get; private set; }

        public ReactiveProperty<bool> Attack { get; private set; }

        public ReactiveProperty<bool> Saiun { get; private set; }

        public ReactiveProperty<bool> MaintenancePersonnel { get; private set; }

        public ReactiveProperty<bool> MinimumSlot { get; private set; }

        public ReactiveProperty<bool> FirstSlotAttack { get; private set; }

        public ReactiveProperty<bool> OnlyAttacker { get; private set; }

        public ReactiveProperty<AirCraft> SlotSetting1 { get; private set; }

        public ReactiveProperty<AirCraft> SlotSetting2 { get; private set; }

        public ReactiveProperty<AirCraft> SlotSetting3 { get; private set; }

        public ReactiveProperty<AirCraft> SlotSetting4 { get; private set; }

        public AirCraftSetting NowSelectSlotSetting { get; set; }

        public ObservableCollection<AirCraftSetting> AirCraftList { get; private set; } = new ObservableCollection<AirCraftSetting>();

        public IEnumerable<AirCraftType> AirCraftTypeList => AirCraftTypeRecords.Instance.Records;

        public ReactiveProperty<AirCraftType> NowSelectAirCraftType { get; set; } = new ReactiveProperty<AirCraftType>();

        public ReactiveCommand SlotSetting1SetCommand { get; } = new ReactiveCommand();

        public ReactiveCommand SlotSetting1ReSetCommand { get; } = new ReactiveCommand();

        public ReactiveCommand SlotSetting2SetCommand { get; } = new ReactiveCommand();

        public ReactiveCommand SlotSetting2ReSetCommand { get; } = new ReactiveCommand();

        public ReactiveCommand SlotSetting3SetCommand { get; } = new ReactiveCommand();

        public ReactiveCommand SlotSetting3ReSetCommand { get; } = new ReactiveCommand();

        public ReactiveCommand SlotSetting4SetCommand { get; } = new ReactiveCommand();

        public ReactiveCommand SlotSetting4ReSetCommand { get; } = new ReactiveCommand();

        public ReactiveProperty<bool> SeaplaneFighterNumEnable { get; private set; }

        public ReactiveProperty<int> SeaplaneFighterNum { get; private set; }

        public ReactiveProperty<bool> SeaplaneBomberNumEnable { get; private set; }

        public ReactiveProperty<int> SeaplaneBomberNum { get; private set; }

        public ReactiveProperty<bool> EquipSlotNumEnable { get; private set; }

        public ReactiveProperty<int> EquipSlotNum { get; private set; }

        public ReactiveProperty<bool> AutoMaintenancePersonnel { get; private set; }

        public bool IsSpecialAttacks => ShipSlotInfo.ShipInfo.Type.Contains("戦艦") || ShipSlotInfo.ShipInfo.Type.Contains("巡洋艦");

        public bool IsCV => ShipSlotInfo.ShipInfo.Type.Contains("空母");

        public bool IsEtc => !IsSpecialAttacks && !IsCV;

        public ShipSlotInfoViewModel(ShipSlotInfo info)
        {
            ShipSlotInfo = info;

            ShipName = info.ObserveProperty(x => x.ShipName).ToReadOnlyReactiveProperty();

            Slot1 = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.Slot1);

            Slot2 = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.Slot2);

            Slot3 = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.Slot3);

            Slot4 = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.Slot4);

            Slot1Num = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.Slot1Num);

            Slot2Num = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.Slot2Num);

            Slot3Num = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.Slot3Num);

            Slot4Num = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.Slot4Num);

            Attack = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.Attack);

            Saiun = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.Saiun);

            MaintenancePersonnel = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.MaintenancePersonnel);

            MinimumSlot = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.MinimumSlot);

            FirstSlotAttack = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.FirstSlotAttack);

            OnlyAttacker = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.OnlyAttacker);

            SlotSetting1 = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.SlotSetting1);

            SlotSetting2 = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.SlotSetting2);

            SlotSetting3 = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.SlotSetting3);

            SlotSetting4 = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.SlotSetting4);

            SeaplaneFighterNumEnable = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.SeaplaneFighterNumEnable);

            SeaplaneFighterNum = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.SeaplaneFighterNum);

            SeaplaneBomberNumEnable = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.SeaplaneBomberNumEnable);

            SeaplaneBomberNum = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.SeaplaneBomberNum);

            EquipSlotNumEnable = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.EquipSlotNumEnable);

            EquipSlotNum = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.EquipSlotNum);

            AutoMaintenancePersonnel = ShipSlotInfo.ToReactivePropertyAsSynchronized(x => x.AutoMaintenancePersonnel);

            SlotSetting1SetCommand.Subscribe(_ => 
            {
                if(NowSelectSlotSetting != null)
                {
                    SlotSetting1.Value = new AirCraft(NowSelectSlotSetting.AirCraft);
                }
            });

            SlotSetting1ReSetCommand.Subscribe(_ => SlotSetting1.Value = null);

            SlotSetting2SetCommand.Subscribe(_ =>
            {
                if (NowSelectSlotSetting != null)
                {
                    SlotSetting2.Value = new AirCraft(NowSelectSlotSetting.AirCraft);
                }
            });

            SlotSetting2ReSetCommand.Subscribe(_ => SlotSetting2.Value = null);

            SlotSetting3SetCommand.Subscribe(_ =>
            {
                if (NowSelectSlotSetting != null)
                {
                    SlotSetting3.Value = new AirCraft(NowSelectSlotSetting.AirCraft);
                }
            });

            SlotSetting3ReSetCommand.Subscribe(_ => SlotSetting3.Value = null);

            SlotSetting4SetCommand.Subscribe(_ =>
            {
                if (NowSelectSlotSetting != null)
                {
                    SlotSetting4.Value = new AirCraft(NowSelectSlotSetting.AirCraft);
                }
            });

            SlotSetting4ReSetCommand.Subscribe(_ => SlotSetting4.Value = null);

            NowSelectAirCraftType.Subscribe(type =>
            {
                AirCraftList.Clear();
                foreach (var item in AirCraftSettingRecords.Instance.Records.Where(x => null == type?.Id ? true : x.AirCraft.AircraftType == type.Id))
                {
                    AirCraftList.Add(item);
                }
            });
        }

        public ShipSlotInfoViewModel(Ship shipInfo) : this(new ShipSlotInfo(shipInfo)) {}

        public ShipSlotInfoViewModel(Ship shipInfo, AirCraft Slot1, AirCraft Slot2, AirCraft Slot3, AirCraft Slot4) : this(new ShipSlotInfo(shipInfo))
        {
            ShipSlotInfo.Slot1 = Slot1;
            ShipSlotInfo.Slot2 = Slot2;
            ShipSlotInfo.Slot3 = Slot3;
            ShipSlotInfo.Slot4 = Slot4;
        }
    }
}
