using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using ZeroFormatter;

namespace AircraftCarrierSlotSolverKai.Models
{
    [ZeroFormattable]
    public class ShipSlotInfo : BindableBase
    {
        [Index(0)]
        public virtual ShipInfo ShipInfo { get; set; }

        /// <summary>
        /// 艦名
        /// </summary>
        [IgnoreFormat]
        public string ShipName
        {
            get { return ShipInfo.Name; }
        }

        private AirCraft _Slot1;
        /// <summary>
        /// 第一スロット艦載機
        /// </summary>
        [Index(1)]
        public virtual AirCraft Slot1
        {
            get { return _Slot1; }
            set { SetProperty(ref _Slot1, value); }
        }

        private int _Slot1Num;
        /// <summary>
        /// 第一スロット数
        /// </summary>
        [Index(2)]
        public virtual int Slot1Num
        {
            get { return _Slot1Num; }
            set { SetProperty(ref _Slot1Num, value); }
        }

        private AirCraft _Slot2;
        /// <summary>
        /// 第二スロット艦載機
        /// </summary>
        [Index(3)]
        public virtual AirCraft Slot2
        {
            get { return _Slot2; }
            set { SetProperty(ref _Slot2, value); }
        }

        private int _Slot2Num;
        /// <summary>
        /// 第二スロット数
        /// </summary>
        [Index(4)]
        public virtual int Slot2Num
        {
            get { return _Slot2Num; }
            set { SetProperty(ref _Slot2Num, value); }
        }

        private AirCraft _Slot3;
        /// <summary>
        /// 第三スロット艦載機
        /// </summary>
        [Index(5)]
        public virtual AirCraft Slot3
        {
            get { return _Slot3; }
            set { SetProperty(ref _Slot3, value); }
        }

        private int _Slot3Num;
        /// <summary>
        /// 第三スロット数
        /// </summary>
        [Index(6)]
        public virtual int Slot3Num
        {
            get { return _Slot3Num; }
            set { SetProperty(ref _Slot3Num, value); }
        }

        private AirCraft _Slot4;
        /// <summary>
        /// 第四スロット艦載機
        /// </summary>
        [Index(7)]
        public virtual AirCraft Slot4
        {
            get { return _Slot4; }
            set { SetProperty(ref _Slot4, value); }
        }

        private int _Slot4Num;
        /// <summary>
        /// 第四スロット数
        /// </summary>
        [Index(8)]
        public virtual int Slot4Num
        {
            get { return _Slot4Num; }
            set { SetProperty(ref _Slot4Num, value); }
        }

        private bool _Attack;
        [Index(9)]
        public virtual bool Attack
        {
            get { return _Attack; }
            set { SetProperty(ref _Attack, value); }
        }

        private bool _Saiun;
        [Index(10)]
        public virtual bool Saiun
        {
            get { return _Saiun; }
            set { SetProperty(ref _Saiun, value); }
        }

        /// <summary>
        /// 熟練艦載機整備員
        /// </summary>
        private bool _MaintenancePersonnel;
        [Index(11)]
        public virtual bool MaintenancePersonnel
        {
            get { return _MaintenancePersonnel; }
            set { SetProperty(ref _MaintenancePersonnel, value); }
        }

        private bool _MinimumSlot;
        [Index(12)]
        public virtual bool MinimumSlot
        {
            get { return _MinimumSlot; }
            set { SetProperty(ref _MinimumSlot, value); }
        }
        private bool _FirstSlotAttack;
        [Index(13)]
        public virtual bool FirstSlotAttack
        {
            get { return _FirstSlotAttack; }
            set { SetProperty(ref _FirstSlotAttack, value); }
        }
        private bool _OnlyAttacker;
        [Index(14)]
        public virtual bool OnlyAttacker
        {
            get { return _OnlyAttacker; }
            set { SetProperty(ref _OnlyAttacker, value); }
        }

        private AirCraft _SlotSetting1;
        [Index(15)]
        public virtual AirCraft SlotSetting1
        {
            get { return _SlotSetting1; }
            set { SetProperty(ref _SlotSetting1, value); }
        }

        private AirCraft _SlotSetting2;
        [Index(16)]
        public virtual AirCraft SlotSetting2
        {
            get { return _SlotSetting2; }
            set { SetProperty(ref _SlotSetting2, value); }
        }

        private AirCraft _SlotSetting3;
        [Index(17)]
        public virtual AirCraft SlotSetting3
        {
            get { return _SlotSetting3; }
            set { SetProperty(ref _SlotSetting3, value); }
        }

        private AirCraft _SlotSetting4;
        [Index(18)]
        public virtual AirCraft SlotSetting4
        {
            get { return _SlotSetting4; }
            set { SetProperty(ref _SlotSetting4, value); }
        }
        private bool _SeaplaneFighterNumEnable = true;
        [Index(19)]
        public virtual bool SeaplaneFighterNumEnable
        {
            get { return _SeaplaneFighterNumEnable; }
            set { SetProperty(ref _SeaplaneFighterNumEnable, value); }
        }
        private int _SeaplaneFighterNum = 0;
        [Index(20)]
        public virtual int SeaplaneFighterNum
        {
            get { return _SeaplaneFighterNum; }
            set { SetProperty(ref _SeaplaneFighterNum, value); }
        }

        private bool _SeaplaneBomberNumEnable = true;
        [Index(21)]
        public virtual bool SeaplaneBomberNumEnable
        {
            get => _SeaplaneBomberNumEnable;
            set => SetProperty(ref _SeaplaneBomberNumEnable, value);
        }

        private int _SeaplaneBomberNum = 1;
        [Index(22)]
        public virtual int SeaplaneBomberNum
        {
            get { return _SeaplaneBomberNum; }
            set { SetProperty(ref _SeaplaneBomberNum, value); }
        }
        private bool _EquipSlotNumEnable;

        [Index(23)] public virtual bool EquipSlotNumEnable { get => _EquipSlotNumEnable; set => SetProperty(ref _EquipSlotNumEnable, value); }

        private int _EquipSlotNum;

        [Index(24)] public virtual int EquipSlotNum { get => _EquipSlotNum; set => SetProperty(ref _EquipSlotNum, value); }

        private bool _AutoMaintenancePersonnel = true;

        [Index(25)] public virtual bool AutoMaintenancePersonnel { get => _AutoMaintenancePersonnel; set => SetProperty(ref _AutoMaintenancePersonnel, value); }

        [IgnoreFormat]
        public int MinSlotNum => new[] { Slot1Num, Slot2Num, Slot3Num, Slot4Num }.Min();

        [IgnoreFormat]
        public int MinSlotIndex => new[] { Slot1Num, Slot2Num, Slot3Num, Slot4Num }.Select((slot, index) => (slot, index)).OrderByDescending(x => x.index).First(y => y.slot == MinSlotNum).index + 1;

        [IgnoreFormat]
        public IEnumerable<(AirCraft airCraft, int index)> SlotSettings => new AirCraft[] { SlotSetting1, SlotSetting2, SlotSetting3, SlotSetting4 }.Select((airCraft, index) => { (AirCraft airCraft, int index) tuple = (airCraft, index + 1); return tuple; });

        [IgnoreFormat]
        public Dictionary<int, string> AirCraftSetting
        {
            get
            {
                return new Dictionary<int, string> { { 0, SlotSetting1?.AirCraftName ?? "未指定" }, { 1, SlotSetting2?.AirCraftName ?? "未指定" }, { 2, SlotSetting3?.AirCraftName ?? "未指定" }, { 3, SlotSetting4?.AirCraftName ?? "未指定" } };
            }
        }

        public ShipSlotInfo()
        {

        }

        public ShipSlotInfo(ShipInfo shipInfo)
        {
            ShipInfo = shipInfo;
            Slot1Num = shipInfo.Slot1Num;
            Slot2Num = shipInfo.Slot2Num;
            Slot3Num = shipInfo.Slot3Num;
            Slot4Num = shipInfo.Slot4Num;
        }
    }
}
