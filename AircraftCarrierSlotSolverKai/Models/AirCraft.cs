using AircraftCarrierSlotSolverKai.Models.Records;
using System;
using System.Linq;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class AirCraft
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 種別
        /// </summary>
        public string Type => AirCraftTypeRecords.Instance.Records.First(x => x.Id == AircraftType).Name;

        /// <summary>
        /// 種別ID
        /// </summary>
        public int AircraftType { get; set; }

        /// <summary>
        /// 火力
        /// </summary>
        public int FirePower { get; set; }

        /// <summary>
        /// 改修値
        /// </summary>
        public int Improvement { get; set; } = 0;

        /// <summary>
        /// 対空
        /// </summary>
        public int AAValue { get; set; }

        /// <summary>
        /// 爆装
        /// </summary>
        public int Bomber { get; set; }
        /// <summary>
        /// 雷装
        /// </summary>
        public int Torpedo { get; set; }
        /// <summary>
        /// 命中
        /// </summary>
        public int Accuracy { get; set; }
        /// <summary>
        /// 回避
        /// </summary>
        public int Evasion { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 装甲
        /// </summary>
        public int Armor { get; set; }
        /// <summary>
        /// 対潜
        /// </summary>
        public int ASW { get; set; }
        /// <summary>
        /// 索敵
        /// </summary>
        public int ViewRange { get; set; }
        /// <summary>
        /// 運
        /// </summary>
        public int Luck { get; set; }
        /// <summary>
        /// 夜戦型
        /// </summary>
        public bool NightType { get; set; }
        /// <summary>
        /// 夜襲カットイン
        /// </summary>
        public bool NightCutin { get; set; }
        /// <summary>
        /// 艦載機名称(改修値付き)
        /// </summary>
        public virtual string FullName
        {
            get
            {
                return Name + (Improvement == 0 ? string.Empty : $"(★{Improvement})");
            }
        }

        /// <summary>
        /// 攻撃可能か
        /// </summary>
        public virtual bool Attackable => Type == Consts.TorpedoBomber || Type == Consts.DiveBomber;

        /// <summary>
        /// 対空値
        /// </summary>
        public virtual int AA
        {
            get
            {
                switch (Type)
                {
                    case Consts.Fighter:
                    case Consts.SeaplaneFighter:
                        return (int)(AAValue + 0.2 * Improvement);
                    case Consts.DiveBomber:
                        return (int)(AAValue + 0.25 * Improvement);
                    case Consts.AviationPersonnel:
                        return 0;
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
            Name = source?.Name;
            AircraftType = source?.AircraftType ?? 0;
            AAValue = source?.AAValue ?? default(int);
            Bomber = source?.Bomber ?? default(int);
            Torpedo = source?.Torpedo ?? default(int);
            Accuracy = source?.Accuracy ?? default(int);
            Evasion = source?.Evasion ?? default(int);
            Improvement = source?.Improvement ?? default(int);
        }

        public AirCraft(int id, int improvement) : this(AirCraftRecords.Instance.Records.First(x => x.Id == id))
        {
            Improvement = improvement;
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
            var air = AA * Math.Sqrt(slotNum);
            var bonus = 0;
            switch (Type)
            {
                case Consts.TorpedoBomber:
                case Consts.DiveBomber:
                case Consts.SeaplaneBomber:
                case Consts.JetBomber:
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
