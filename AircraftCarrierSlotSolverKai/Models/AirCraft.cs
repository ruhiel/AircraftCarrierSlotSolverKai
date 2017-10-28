using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;

namespace AircraftCarrierSlotSolverKai.Models
{
    [ZeroFormattable]
    public class AirCraft
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Index(0)] public virtual string Name { get; set; }

        /// <summary>
        /// 種別
        /// </summary>
        [IgnoreFormat] public virtual string Type => AirCraftTypeRecords.Instance.Records.First(x => x.Id == AircraftType).Name;

        /// <summary>
        /// 種別ID
        /// </summary>
        [Index(1)] public virtual int AircraftType { get; set; }

        /// <summary>
        /// 火力
        /// </summary>
        [Index(2)] public virtual int FirePower { get; set; }

        /// <summary>
        /// 改修値
        /// </summary>
        [Index(3)] public virtual int Improvement { get; set; } = 0;

        /// <summary>
        /// 対空
        /// </summary>
        [Index(4)] public virtual int AAValue { get; set; }

        /// <summary>
        /// 爆装
        /// </summary>
        [Index(5)] public virtual int Bomber { get; set; }
        /// <summary>
        /// 雷装
        /// </summary>
        [Index(6)] public virtual int Torpedo { get; set; }
        /// <summary>
        /// 命中
        /// </summary>
        [Index(7)] public virtual int Accuracy { get; set; }
        /// <summary>
        /// 回避
        /// </summary>
        [Index(8)] public virtual int Evasion { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        [Index(9)] public virtual int Id { get; set; }
        /// <summary>
        /// 装甲
        /// </summary>
        [Index(10)] public virtual int Armor { get; set; }
        /// <summary>
        /// 対潜
        /// </summary>
        [Index(11)] public virtual int ASW { get; set; }
        /// <summary>
        /// 索敵
        /// </summary>
        [Index(12)] public virtual int ViewRange { get; set; }
        /// <summary>
        /// 運
        /// </summary>
        [Index(13)] public virtual int Luck { get; set; }

        /// <summary>
        /// 艦載機名称(改修値付き)
        /// </summary>
        [IgnoreFormat]
        public virtual string AirCraftName
        {
            get
            {
                return Name + (Improvement == 0 ? string.Empty : string.Format("(★{0})", Improvement));
            }
        }

        [IgnoreFormat]
        public virtual int TypeOrder
        {
            get
            {
                switch (Type)
                {
                    case Consts.Fighter:
                        return 0;
                    case Consts.TorpedoBomber:
                        return 1;
                    case Consts.DiveBomber:
                        return 2;
                    default:
                        return 3;
                }
            }
        }
        /// <summary>
        /// 攻撃可能か
        /// </summary>
        [IgnoreFormat] public virtual bool Attackable => Type == Consts.TorpedoBomber || Type == Consts.DiveBomber;


        /// <summary>
        /// 対空値
        /// </summary>
        [IgnoreFormat]
        public virtual int AA
        {
            get
            {
                switch (Type)
                {
                    case "艦戦":
                        return (int)(AAValue + 0.2 * Improvement);
                    case "艦爆":
                        return (int)(AAValue + 0.25 * Improvement);
                    default:
                        return AAValue;
                }
            }
        }

        public AirCraft()
        {
        }

        public AirCraft(string name, int type, int firePower = 0, int aa = 0, int bomber = 0, int torpedo = 0, int accuracy = 0, int evasion = 0, int improvement = 0)
        {
            Name = name;
            AircraftType = type;
            AAValue = aa;
            Bomber = bomber;
            Torpedo = torpedo;
            Accuracy = accuracy;
            Evasion = evasion;
            Improvement = improvement;
        }

        public AirCraft(AirCraft source)
        {
            Id = source?.Id ?? 0;
            Name = source?.Name ?? "装備なし";
            AircraftType = source?.AircraftType ?? 0;
            AAValue = source?.AAValue ?? default(int);
            Bomber = source?.Bomber ?? default(int);
            Torpedo = source?.Torpedo ?? default(int);
            Accuracy = source?.Accuracy ?? default(int);
            Evasion = source?.Evasion ?? default(int);
            Improvement = source?.Improvement ?? default(int);
        }

        public AirCraft(AirCraftSetting setting) : this(setting?.AirCraft)
        {
        }

        public int Power(int slotNum)
        {
            if (Type == Consts.DiveBomber)
            {
                var pow = 1 * (25 + Bomber * Math.Sqrt(slotNum));
                return (int)Math.Floor(pow);
            }
            else if (Type == Consts.TorpedoBomber)
            {
                var pow = 1.15 * (25 + Torpedo * Math.Sqrt(slotNum));
                return (int)Math.Floor(pow);
            }
            else
            {
                return 0;
            }
        }

        public int AirSuperiorityPotential(int slotNum)
        {
            if (Name == "装備なし")
            {
                return 0;
            }

            var air = AA * Math.Sqrt(slotNum);
            double bonus = 0;
            switch (Type)
            {
                case Consts.TorpedoBomber:
                case Consts.DiveBomber:
                case Consts.SeaplaneBomber:
                    bonus = 3;
                    break;
                case Consts.Fighter:
                case Consts.SeaplaneFighter:
                    bonus = 25;
                    break;
                default:
                    break;
            }

            return (int)Math.Floor(air + bonus);
        }
    }

}
