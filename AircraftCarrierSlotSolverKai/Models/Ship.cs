using AircraftCarrierSlotSolverKai.Models.Records;
using System.Linq;
using System.Text.RegularExpressions;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class Ship
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 艦名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 艦種
        /// </summary>
        public string Type => ShipTypeRecords.Instance.Records.First(x => x.ID == Shiptype).Name;
        /// <summary>
        /// 艦種ID
        /// </summary>
        public int Shiptype { get; set; }
        /// <summary>
        /// 火力
        /// </summary>
        public int FirePower { get; set; }
        /// <summary>
        /// スロット数
        /// </summary>
        public int SlotNum { get; set; }
        /// <summary>
        /// スロット数1
        /// </summary>
        public int Slot1Num { get; set; }
        /// <summary>
        /// スロット数2
        /// </summary>
        public int Slot2Num { get; set; }
        /// <summary>
        /// スロット数3
        /// </summary>
        public int Slot3Num { get; set; }
        /// <summary>
        /// スロット数4
        /// </summary>
        public int Slot4Num { get; set; }
        /// <summary>
        /// スロット数5
        /// </summary>
        public int Slot5Num { get; set; }
        /// <summary>
        /// スロット数リスト
        /// </summary>
        public int[] Slots => new int[] { Slot1Num, Slot2Num, Slot3Num, Slot4Num, Slot5Num };
        /// <summary>
        /// 改装前
        /// </summary>
        public string PrevRemodel { get; set; }
        /// <summary>
        /// 夜襲カットイン
        /// </summary>
        public bool NightCutin { get; set; }

        public bool IsSeaplaneEquipable => Regex.IsMatch(Type, ".*戦艦|.*巡洋艦|潜水空母|水上機母艦");

        public bool IsCV => Regex.IsMatch(Type, "(正規|装甲|軽)空母");

        public bool IsEtc => !IsSeaplaneEquipable && !IsCV;
    }

}
