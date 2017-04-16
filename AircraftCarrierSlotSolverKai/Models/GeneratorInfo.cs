using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class GeneratorInfo
    {
        public (ShipInfo, int) Ship { get; set; }
        /// <summary>
        /// Item1:機数、Item2:スロット番号
        /// </summary>
        public (int, int) Slot { get; set; }

        public (AirCraft, int) AirCraft { get; set; }

        public string SlotName => string.Format("slot_{0}_{1}_{2}", Ship.Item2, Slot.Item2, AirCraft.Item2);

        public int Power
        {
            get
            {
                if (AirCraft.Item1.Type == Consts.DiveBomber)
                {
                    var pow = 1 * (25 + AirCraft.Item1.Bomber * Math.Sqrt(Slot.Item1));
                    return (int)Math.Floor(pow);
                }
                else if (AirCraft.Item1.Type == Consts.TorpedoBomber)
                {
                    var pow = 1.15 * (25 + AirCraft.Item1.Torpedo * Math.Sqrt(Slot.Item1));
                    return (int)Math.Floor(pow);
                }
                else
                {
                    return 0;
                }
            }
        }

        public int AirSuperiorityPotential
        {
            get
            {
                if (AirCraft.Item1.Name == "装備なし")
                {
                    return 0;
                }

                var air = AirCraft.Item1.AA * Math.Sqrt(Slot.Item1);
                double bonus = 0;
                switch (AirCraft.Item1.Type)
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
}
