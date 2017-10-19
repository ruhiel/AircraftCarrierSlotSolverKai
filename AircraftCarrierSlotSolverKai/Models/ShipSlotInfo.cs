using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class ShipSlotInfo : BindableBase
    {
        private string _ShipName;

        public ShipInfo ShipInfo { get; private set; }

        /// <summary>
        /// 艦名
        /// </summary>
        public string ShipName
        {
            get { return _ShipName; }
            set { SetProperty(ref _ShipName, value); }
        }

        private AirCraft _Slot1;
        /// <summary>
        /// 第一スロット艦載機
        /// </summary>
        public AirCraft Slot1
        {
            get { return _Slot1; }
            set { SetProperty(ref _Slot1, value); }
        }

        private int _Slot1Num;
        /// <summary>
        /// 第一スロット数
        /// </summary>
        public int Slot1Num
        {
            get { return _Slot1Num; }
            set { SetProperty(ref _Slot1Num, value); }
        }

        private AirCraft _Slot2;
        /// <summary>
        /// 第二スロット艦載機
        /// </summary>
        public AirCraft Slot2
        {
            get { return _Slot2; }
            set { SetProperty(ref _Slot2, value); }
        }

        private int _Slot2Num;
        /// <summary>
        /// 第二スロット数
        /// </summary>
        public int Slot2Num
        {
            get { return _Slot2Num; }
            set { SetProperty(ref _Slot2Num, value); }
        }

        private AirCraft _Slot3;
        /// <summary>
        /// 第三スロット艦載機
        /// </summary>
        public AirCraft Slot3
        {
            get { return _Slot3; }
            set { SetProperty(ref _Slot3, value); }
        }

        private int _Slot3Num;
        /// <summary>
        /// 第三スロット数
        /// </summary>
        public int Slot3Num
        {
            get { return _Slot3Num; }
            set { SetProperty(ref _Slot3Num, value); }
        }

        private AirCraft _Slot4;
        /// <summary>
        /// 第四スロット艦載機
        /// </summary>
        public AirCraft Slot4
        {
            get { return _Slot4; }
            set { SetProperty(ref _Slot4, value); }
        }

        private int _Slot4Num;
        /// <summary>
        /// 第四スロット数
        /// </summary>
        public int Slot4Num
        {
            get { return _Slot4Num; }
            set { SetProperty(ref _Slot4Num, value); }
        }

        public int MinSlotNum => new[] { Slot1Num, Slot2Num, Slot3Num, Slot4Num }.Min();

        public int MinSlotIndex => new[] { Slot1Num, Slot2Num, Slot3Num, Slot4Num }.Select((slot, index) => (slot, index)).OrderByDescending(x => x.index).First(y => y.slot == MinSlotNum).index + 1;

        private bool _Attack;
        public bool Attack
        {
            get { return _Attack; }
            set { SetProperty(ref _Attack, value); }
        }

        private bool _Saiun;
        public bool Saiun
        {
            get { return _Saiun; }
            set { SetProperty(ref _Saiun, value); }
        }

        /// <summary>
        /// 熟練艦載機整備員
        /// </summary>
        private bool _MaintenancePersonnel;
        public bool MaintenancePersonnel
        {
            get { return _MaintenancePersonnel; }
            set { SetProperty(ref _MaintenancePersonnel, value); }
        }

        private bool _MinimumSlot;
        public bool MinimumSlot
        {
            get { return _MinimumSlot; }
            set { SetProperty(ref _MinimumSlot, value); }
        }
        private bool _FirstSlotAttack;
        public bool FirstSlotAttack
        {
            get { return _FirstSlotAttack; }
            set { SetProperty(ref _FirstSlotAttack, value); }
        }
        private bool _OnlyAttacker;
        public bool OnlyAttacker
        {
            get { return _OnlyAttacker; }
            set { SetProperty(ref _OnlyAttacker, value); }
        }

        private AirCraft _SlotSetting1;
        public AirCraft SlotSetting1
        {
            get { return _SlotSetting1; }
            set { SetProperty(ref _SlotSetting1, value); }
        }

        private AirCraft _SlotSetting2;
        public AirCraft SlotSetting2
        {
            get { return _SlotSetting2; }
            set { SetProperty(ref _SlotSetting2, value); }
        }

        private AirCraft _SlotSetting3;
        public AirCraft SlotSetting3
        {
            get { return _SlotSetting3; }
            set { SetProperty(ref _SlotSetting3, value); }
        }

        private AirCraft _SlotSetting4;
        public AirCraft SlotSetting4
        {
            get { return _SlotSetting4; }
            set { SetProperty(ref _SlotSetting4, value); }
        }

        public IEnumerable<(AirCraft airCraft, int index)> SlotSettings => new AirCraft[] { SlotSetting1, SlotSetting2, SlotSetting3, SlotSetting4 }.Select((airCraft, index) => { (AirCraft airCraft, int index) tuple = (airCraft, index + 1); return tuple; });

        public Dictionary<int, string> AirCraftSetting
        {
            get
            {
                return new Dictionary<int, string> { { 0, SlotSetting1?.AirCraftName ?? "未指定" }, { 1, SlotSetting2?.AirCraftName ?? "未指定" }, { 2, SlotSetting3?.AirCraftName ?? "未指定" }, { 3, SlotSetting4?.AirCraftName ?? "未指定" } };
            }
        }
        private bool _SeaplaneFighterNumEnable;
        public bool SeaplaneFighterNumEnable
        {
            get { return _SeaplaneFighterNumEnable; }
            set { SetProperty(ref _SeaplaneFighterNumEnable, value); }
        }
        private int _SeaplaneFighterNum;
        public int SeaplaneFighterNum
        {
            get { return _SeaplaneFighterNum; }
            set { SetProperty(ref _SeaplaneFighterNum, value); }
        }

        public ShipSlotInfo(ShipInfo shipInfo)
        {
            ShipInfo = shipInfo;
            ShipName = shipInfo.Name;
            Slot1Num = shipInfo.Slot1Num;
            Slot2Num = shipInfo.Slot2Num;
            Slot3Num = shipInfo.Slot3Num;
            Slot4Num = shipInfo.Slot4Num;
        }
    }
}
