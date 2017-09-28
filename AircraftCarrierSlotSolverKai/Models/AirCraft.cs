using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class AirCraft
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 艦載機名称(改修値付き)
        /// </summary>
        public string AirCraftName
        {
            get
            {
                return Name + (Improvement == 0 ? string.Empty : string.Format("(★{0})", Improvement));
            }
        }

        /// <summary>
        /// 種別
        /// </summary>
        public string Type { get; set; }

        public int TypeOrder
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
        public bool Attackable => Type == Consts.TorpedoBomber || Type == Consts.DiveBomber;

        /// <summary>
        /// 火力
        /// </summary>
        public int FirePower { get; set; }

        /// <summary>
        /// 改修値
        /// </summary>
        public int Improvement { get; set; }

        /// <summary>
        /// 対空
        /// </summary>
        public int AAValue { get; set; }

        /// <summary>
        /// 対空値
        /// </summary>
        public int AA
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
        public int Id { get; set; }
        public int Armor { get; set; }
        public int ASW { get; set; }
        public int ViewRange { get; set; }
        public int Luck { get; set; }

        public AirCraft()
        {
            Improvement = 0;
        }

        public AirCraft(string name, string type, int firePower = 0, int aa = 0, int bomber = 0, int torpedo = 0, int accuracy = 0, int evasion = 0, int improvement = 0)
        {
            Name = name;
            Type = type;
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
            Type = source?.Type ?? "その他";
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
