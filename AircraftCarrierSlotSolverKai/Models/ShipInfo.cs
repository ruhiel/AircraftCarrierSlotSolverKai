using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;

namespace AircraftCarrierSlotSolverKai.Models
{
    [ZeroFormattable]
    public class ShipInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        [Index(0)] public virtual int ID { get; set; }
        /// <summary>
        /// 艦名
        /// </summary>
        [Index(1)] public virtual string Name { get; set; }
        /// <summary>
        /// 艦種
        /// </summary>
        [Index(2)] public virtual string Type { get; set; }
        /// <summary>
        /// 火力
        /// </summary>
        [Index(3)] public virtual int FirePower { get; set; }
        /// <summary>
        /// スロット数
        /// </summary>
        [Index(4)] public virtual int SlotNum { get; set; }
        /// <summary>
        /// スロット数1
        /// </summary>
        [Index(5)] public virtual int Slot1Num { get; set; }
        /// <summary>
        /// スロット数2
        /// </summary>
        [Index(6)] public virtual int Slot2Num { get; set; }
        /// <summary>
        /// スロット数3
        /// </summary>
        [Index(7)] public virtual int Slot3Num { get; set; }
        /// <summary>
        /// スロット数4
        /// </summary>
        [Index(8)] public virtual int Slot4Num { get; set; }
        /// <summary>
        /// スロット数リスト
        /// </summary>
        [IgnoreFormat] public virtual int[] Slots => new int[] { Slot1Num, Slot2Num, Slot3Num, Slot4Num };

        /// <summary>
        /// 改装前
        /// </summary>
        [Index(10)] public virtual string Prev { get; set; }
    }

}
