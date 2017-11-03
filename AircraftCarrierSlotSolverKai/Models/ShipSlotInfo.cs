using AircraftCarrierSlotSolverKai.Models.Records;
using Newtonsoft.Json;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace AircraftCarrierSlotSolverKai.Models
{
    [JsonObject]
    public class ShipSlotInfo : BindableBase
    {
        [JsonProperty]
        public int ShipId { get; set; }

        [JsonIgnore]
        public Ship ShipInfo => ShipRecords.Instance.Records.First(x => x.ID == ShipId);

        /// <summary>
        /// 艦名
        /// </summary>
        [JsonIgnore]
        public string ShipName => ShipInfo.Name;

        private AirCraftInfo _Slot1;
        /// <summary>
        /// 第一スロット艦載機
        /// </summary>
        [JsonProperty]
        public AirCraftInfo Slot1
        {
            get { return _Slot1; }
            set { SetProperty(ref _Slot1, value); }
        }

        /// <summary>
        /// 第一スロット数
        /// </summary>
        [JsonIgnore]
        public int Slot1Num => ShipInfo.Slot1Num;

        private AirCraftInfo _Slot2;
        /// <summary>
        /// 第二スロット艦載機
        /// </summary>
        [JsonProperty]
        public AirCraftInfo Slot2
        {
            get { return _Slot2; }
            set { SetProperty(ref _Slot2, value); }
        }

        /// <summary>
        /// 第二スロット数
        /// </summary>
        [JsonIgnore]
        public int Slot2Num => ShipInfo.Slot2Num;

        private AirCraftInfo _Slot3;
        /// <summary>
        /// 第三スロット艦載機
        /// </summary>
        [JsonProperty]
        public AirCraftInfo Slot3
        {
            get { return _Slot3; }
            set { SetProperty(ref _Slot3, value); }
        }

        /// <summary>
        /// 第三スロット数
        /// </summary>
        [JsonIgnore]
        public int Slot3Num => ShipInfo.Slot3Num;

        private AirCraftInfo _Slot4;
        /// <summary>
        /// 第四スロット艦載機
        /// </summary>
        [JsonProperty]
        public AirCraftInfo Slot4
        {
            get { return _Slot4; }
            set { SetProperty(ref _Slot4, value); }
        }

        /// <summary>
        /// 第四スロット数
        /// </summary>
        [JsonIgnore]
        public int Slot4Num => ShipInfo.Slot4Num;

        private bool _Attack;
        [JsonProperty]
        public bool Attack
        {
            get { return _Attack; }
            set { SetProperty(ref _Attack, value); }
        }

        private bool _Saiun;
        [JsonProperty]
        public bool Saiun
        {
            get { return _Saiun; }
            set { SetProperty(ref _Saiun, value); }
        }

        /// <summary>
        /// 熟練艦載機整備員
        /// </summary>
        private bool _MaintenancePersonnel;
        [JsonProperty]
        public bool MaintenancePersonnel
        {
            get { return _MaintenancePersonnel; }
            set { SetProperty(ref _MaintenancePersonnel, value); }
        }

        private bool _MinimumSlot;
        [JsonProperty]
        public bool MinimumSlot
        {
            get { return _MinimumSlot; }
            set { SetProperty(ref _MinimumSlot, value); }
        }
        private bool _FirstSlotAttack;
        [JsonProperty]
        public bool FirstSlotAttack
        {
            get { return _FirstSlotAttack; }
            set { SetProperty(ref _FirstSlotAttack, value); }
        }
        private bool _OnlyAttacker;
        [JsonProperty]
        public bool OnlyAttacker
        {
            get { return _OnlyAttacker; }
            set { SetProperty(ref _OnlyAttacker, value); }
        }

        private AirCraftInfo _SlotSetting1;
        [JsonProperty]
        public AirCraftInfo SlotSetting1
        {
            get { return _SlotSetting1; }
            set { SetProperty(ref _SlotSetting1, value); }
        }

        private AirCraftInfo _SlotSetting2;
        [JsonProperty]
        public AirCraftInfo SlotSetting2
        {
            get { return _SlotSetting2; }
            set { SetProperty(ref _SlotSetting2, value); }
        }

        private AirCraftInfo _SlotSetting3;
        [JsonProperty]
        public AirCraftInfo SlotSetting3
        {
            get { return _SlotSetting3; }
            set { SetProperty(ref _SlotSetting3, value); }
        }

        private AirCraftInfo _SlotSetting4;
        [JsonProperty]
        public AirCraftInfo SlotSetting4
        {
            get { return _SlotSetting4; }
            set { SetProperty(ref _SlotSetting4, value); }
        }
        private bool _SeaplaneFighterNumEnable = true;
        [JsonProperty]
        public bool SeaplaneFighterNumEnable
        {
            get { return _SeaplaneFighterNumEnable; }
            set { SetProperty(ref _SeaplaneFighterNumEnable, value); }
        }
        private int _SeaplaneFighterNum = 0;
        [JsonProperty]
        public int SeaplaneFighterNum
        {
            get { return _SeaplaneFighterNum; }
            set { SetProperty(ref _SeaplaneFighterNum, value); }
        }

        private bool _SeaplaneBomberNumEnable = true;
        [JsonProperty]
        public bool SeaplaneBomberNumEnable
        {
            get => _SeaplaneBomberNumEnable;
            set => SetProperty(ref _SeaplaneBomberNumEnable, value);
        }

        private int _SeaplaneBomberNum = 1;
        [JsonProperty]
        public int SeaplaneBomberNum
        {
            get { return _SeaplaneBomberNum; }
            set { SetProperty(ref _SeaplaneBomberNum, value); }
        }
        private bool _EquipSlotNumEnable;

        [JsonProperty] public bool EquipSlotNumEnable { get => _EquipSlotNumEnable; set => SetProperty(ref _EquipSlotNumEnable, value); }

        private int _EquipSlotNum;

        [JsonProperty] public int EquipSlotNum { get => _EquipSlotNum; set => SetProperty(ref _EquipSlotNum, value); }

        private bool _AutoMaintenancePersonnel = true;

        [JsonProperty] public bool AutoMaintenancePersonnel { get => _AutoMaintenancePersonnel; set => SetProperty(ref _AutoMaintenancePersonnel, value); }

        [JsonIgnore]
        public int MinSlotNum => new[] { Slot1Num, Slot2Num, Slot3Num, Slot4Num }.Min();

        [JsonIgnore]
        public int MinSlotIndex => new[] { Slot1Num, Slot2Num, Slot3Num, Slot4Num }.Select((slot, index) => (slot, index)).OrderByDescending(x => x.index).First(y => y.slot == MinSlotNum).index + 1;

        [JsonIgnore]
        public IEnumerable<(AirCraft airCraft, int index)> SlotSettings => new AirCraft[] { SlotSetting1?.AirCraft, SlotSetting2?.AirCraft, SlotSetting3?.AirCraft, SlotSetting4?.AirCraft }.Select((airCraft, index) => { (AirCraft airCraft, int index) tuple = (airCraft, index + 1); return tuple; });

        [JsonIgnore]
        public Dictionary<int, string> AirCraftSetting
        {
            get
            {
                return new Dictionary<int, string> { { 0, SlotSetting1?.AirCraft.FullName ?? "未指定" }, { 1, SlotSetting2?.AirCraft.FullName ?? "未指定" }, { 2, SlotSetting3?.AirCraft.FullName ?? "未指定" }, { 3, SlotSetting4?.AirCraft.FullName ?? "未指定" } };
            }
        }

        public ShipSlotInfo(int shipId)
        {
            ShipId = shipId;
        }
    }
}
