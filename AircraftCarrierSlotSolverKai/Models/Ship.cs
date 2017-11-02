using AircraftCarrierSlotSolverKai.Models.Records;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    [JsonObject]
    public class Ship
    {
        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty] public int ID { get; set; }
        /// <summary>
        /// 艦名
        /// </summary>
        [JsonProperty] public string Name { get; set; }
        /// <summary>
        /// 艦種
        /// </summary>
        [JsonIgnore] public string Type => ShipTypeRecords.Instance.Records.First(x => x.ID == Shiptype).Name;
        /// <summary>
        /// 艦種ID
        /// </summary>
        [JsonProperty] public int Shiptype { get; set; }
        /// <summary>
        /// 火力
        /// </summary>
        [JsonProperty] public int FirePower { get; set; }
        /// <summary>
        /// スロット数
        /// </summary>
        [JsonProperty] public int SlotNum { get; set; }
        /// <summary>
        /// スロット数1
        /// </summary>
        [JsonProperty] public int Slot1Num { get; set; }
        /// <summary>
        /// スロット数2
        /// </summary>
        [JsonProperty] public int Slot2Num { get; set; }
        /// <summary>
        /// スロット数3
        /// </summary>
        [JsonProperty] public int Slot3Num { get; set; }
        /// <summary>
        /// スロット数4
        /// </summary>
        [JsonProperty] public int Slot4Num { get; set; }
        /// <summary>
        /// スロット数リスト
        /// </summary>
        [JsonIgnore] public int[] Slots => new int[] { Slot1Num, Slot2Num, Slot3Num, Slot4Num };

        /// <summary>
        /// 改装前
        /// </summary>
        [JsonProperty] public string PrevRemodel { get; set; }
    }

}
