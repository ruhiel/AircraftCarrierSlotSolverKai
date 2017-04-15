using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class Calculator
    {
        private static Dictionary<AirCraft, int> _AirCraftLimits;

        public static (bool result, string message) Calc(int airSuperiority, IEnumerable<ShipSlotInfo> shipSlotInfos)
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.SolverPath))
            {
                return (false, "SCIPソルバーが指定されていないため計算実行できません。");
            }

            if (!File.Exists(Properties.Settings.Default.SolverPath))
            {
                return (false, "SCIPソルバーが存在しないため計算実行できません。");
            }

            if (!shipSlotInfos.Any())
            {
                return (false, "艦娘が追加されていないため計算実行できません。");
            }

            _AirCraftLimits = new Dictionary<AirCraft, int>();

            AirCraftRecords.Instance.Load();

            foreach (var aircraft in AirCraftSettingRecords.Instance.Records)
            {
                var air = new AirCraft(AirCraftRecords.Instance.Records.First(x => x.Name == aircraft.Name));
                air.Improvement = aircraft.Improvement;
                _AirCraftLimits.Add(air, aircraft.Value);
            }

            try
            {
                GenerateLPFile(shipSlotInfos, airSuperiority);

                var dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                GenerateSolveFile(dir);

                var slotStringList = CalcProcess(dir);

                if (!slotStringList.Any())
                {
                    return (false, "制空値を満たす解がありませんでした。");
                }

                CalcResultViewProcess(slotStringList, shipSlotInfos);
            }
            catch (Exception ex)
            {
                return (false, $"SCIPソルバーの実行に失敗しました。:{ex.Message}");
            }

            return (true, string.Empty);
        }

        private static void GenerateLPFile(IEnumerable<ShipSlotInfo> shipSlotList, int airSuperiority)
        {
            using (StreamWriter writer = new StreamWriter(@"slot.lp", false, new UTF8Encoding(false)))
            {
                OutputTarget(writer, shipSlotList);

                OutputAirCondition(writer, shipSlotList, airSuperiority);

                OutputSlotCondition(writer, shipSlotList);

                OutputStockCondition(writer, shipSlotList);

                OutputShipTypeCondition(writer, shipSlotList);

                OutputModeCondition(writer, shipSlotList);

                OutputBinary(writer, shipSlotList);

                writer.WriteLine("end");
            }
        }

        private static IEnumerable<GeneratorInfo> GetIEnumerable(IEnumerable<ShipSlotInfo> shipSlotList)
        {
            var noEquip = new List<AirCraft>()
            {
				// 所持数制限を受けないダミー装備
				new AirCraft("装備なし", "その他")
            };

            foreach (var ship in ShipInfoRecords.Instance.Records
                .Select((item, index) => Tuple.Create(item, index))
                .Where(x => shipSlotList.Select(y => y.ShipName).Contains(x.Item1.Name)))
            {
                foreach (var slot in ship.Item1.Slots.Select((item, index) => Tuple.Create(item, index)))
                {
                    foreach (var aircraft in GetAircraft(ship.Item1).Concat(noEquip).Select((item, index) => Tuple.Create(item, index)))
                    {
                        if (ship.Item1.SlotNum > slot.Item2)
                        {
                            yield return new GeneratorInfo() { Ship = ship, Slot = slot, AirCraft = aircraft };
                        }
                        else if (aircraft.Item1.AirCraftName == "装備なし")
                        {
                            yield return new GeneratorInfo() { Ship = ship, Slot = slot, AirCraft = aircraft };
                        }
                    }
                }
            }
        }

        private static IEnumerable<AirCraft> GetAircraft(ShipInfo ship)
        {
            Func<AirCraft, bool> predicate = null;
            var airCrafts = new List<AirCraft>();

            switch (ship.Type)
            {
                case "揚陸":
                    predicate = (x) => x.Type == Consts.Fighter || x.Type == "その他";
                    break;
                case "補給":
                    predicate = (x) => x.Type == Consts.TorpedoBomber || x.Type == "その他";
                    break;
                case "巡洋艦":
                case "潜母":
                    predicate = (x) => x.Type == Consts.SeaplaneBomber || x.Type == Consts.SeaplaneFighter || x.Type == "その他";
                    break;
                default:
                    predicate = (x) => x.Type == Consts.TorpedoBomber || x.Type == Consts.DiveBomber || x.Type == Consts.Fighter || x.Type == Consts.JetBomber || x.Type == "その他";
                    break;
            }

            return _AirCraftLimits.Select(y => y.Key).Where(predicate);
        }

        private static void OutputTarget(StreamWriter writer, IEnumerable<ShipSlotInfo> shipSlotList)
        {
            writer.WriteLine("maximize");

            foreach (var record in GetIEnumerable(shipSlotList))
            {
                var text = "+ " + record.Power + " " + record.SlotName + @" \ " + record.Ship.Item1.Name + " " + record.Slot.Item1 + " " + record.AirCraft.Item1.AirCraftName + " 火力";
                writer.WriteLine(text);
                text = "+ " + record.AirCraft.Item1.Accuracy + " " + record.SlotName + @" \ " + record.Ship.Item1.Name + " " + record.Slot.Item1 + " " + record.AirCraft.Item1.AirCraftName + " 命中";
                writer.WriteLine(text);
                text = "+ " + record.AirCraft.Item1.Evasion + " " + record.SlotName + @" \ " + record.Ship.Item1.Name + " " + record.Slot.Item1 + " " + record.AirCraft.Item1.AirCraftName + " 回避";
                writer.WriteLine(text);
            }

            writer.WriteLine();
        }

        private static void OutputAirCondition(StreamWriter writer, IEnumerable<ShipSlotInfo> shipSlotList, int airSuperiority)
        {
            writer.WriteLine("subject to");

            foreach (var record in GetIEnumerable(shipSlotList))
            {
                var text = "+ " + record.AirSuperiorityPotential + " " + record.SlotName + @" \ " + record.Ship.Item1.Name + " " + record.Slot.Item1 + " " + record.AirCraft.Item1.AirCraftName;
                writer.WriteLine(text);
            }
            writer.WriteLine(">= " + airSuperiority);
            writer.WriteLine();
        }

        private static void OutputSlotCondition(StreamWriter writer, IEnumerable<ShipSlotInfo> shipSlotList)
        {
            foreach (var group in GetIEnumerable(shipSlotList).GroupBy(x => new { Ship = x.Ship.Item2, Slot = x.Slot.Item2 }))
            {
                foreach (var g in group)
                {
                    var text = "+ " + g.SlotName;
                    writer.WriteLine(text);
                }
                writer.WriteLine("= 1");
                writer.WriteLine();
            }
        }

        private static void OutputStockCondition(StreamWriter writer, IEnumerable<ShipSlotInfo> shipSlotList)
        {
            foreach (var dic in _AirCraftLimits)
            {
                var list = GetIEnumerable(shipSlotList).Where(x => x.AirCraft.Item1 == dic.Key);
                if (list.Any())
                {
                    foreach (var record in list)
                    {
                        writer.WriteLine("+ " + record.SlotName);
                    }
                    writer.WriteLine("<= " + dic.Value + @" \ 所持制限 " + dic.Key.Name);
                    writer.WriteLine();
                }
            }
        }

        private static void OutputShipTypeCondition(StreamWriter writer, IEnumerable<ShipSlotInfo> shipSlotList)
        {
            if (GetIEnumerable(shipSlotList).Any(x => x.Ship.Item1.Type == "巡洋艦"))
            {
                // 水上機制限数
                foreach (var noEquipShipList in GetIEnumerable(shipSlotList)
                    .Where(x => x.Ship.Item1.Type == "巡洋艦" && x.AirCraft.Item1.AirCraftName != "装備なし")
                    .GroupBy(y => y.Ship.Item2))
                {
                    foreach (var noEquipList in noEquipShipList)
                    {
                        writer.WriteLine("+ " + noEquipList.SlotName);
                    }

                    writer.WriteLine("<= " + Properties.Settings.Default.CruiserSlotNum);
                    writer.WriteLine();
                }
            }
        }

        private static void OutputModeCondition(StreamWriter writer, IEnumerable<ShipSlotInfo> shipSlotList)
        {
            var infoList = GetIEnumerable(shipSlotList);
            foreach (var info in shipSlotList.Where(x => x.Attack))
            {
                var list = infoList.Where(x => x.Ship.Item1.Name == info.ShipName && x.AirCraft.Item1.Attackable);
                if (list.Any())
                {
                    foreach (var i in list)
                    {
                        var text = "+ " + i.SlotName + @" \ 攻撃機";
                        writer.WriteLine(text);
                    }
                    writer.WriteLine(">= 1");
                    writer.WriteLine();
                }
            }

            foreach (var info in shipSlotList.Where(x => x.FirstSlotAttack))
            {
                var list = infoList.Where(x => x.Ship.Item1.Name == info.ShipName &&
                    x.Slot.Item2 == 0 &&
                    x.AirCraft.Item1.Attackable);

                if (list.Any())
                {
                    foreach (var i in list)
                    {
                        var text = "+ " + i.SlotName + @" \ 1スロ目攻撃機";
                        writer.WriteLine(text);
                    }
                    writer.WriteLine(">= 1");
                    writer.WriteLine();
                }
            }

            foreach (var info in shipSlotList.Where(x => x.OnlyAttacker))
            {
                var list = infoList.Where(x => x.Ship.Item1.Name == info.ShipName && x.AirCraft.Item1.Type == Consts.Fighter);
                foreach (var i in list)
                {
                    var text = "+ " + i.SlotName + @" \ 攻撃機のみ";
                    writer.WriteLine(text);
                }
                writer.WriteLine("= 0");
                writer.WriteLine();
            }

            foreach (var info in shipSlotList.Where(x => x.AirCraftSetting.Any(y => y.Value != "未指定")))
            {
                foreach (var dic in info.AirCraftSetting.Where(z => z.Value != "未指定"))
                {
                    var v = infoList.Single(j => j.Ship.Item1.Name == info.ShipName && j.AirCraft.Item1.AirCraftName == dic.Value && j.Slot.Item2 == dic.Key);

                    var text = "+ " + v.SlotName + @" = 1 \ 艦載機指定";
                    writer.WriteLine(text);
                    writer.WriteLine();
                }
            }

            OutputEquipCondition(writer, shipSlotList);
        }

        private static void OutputEquipCondition(StreamWriter writer, IEnumerable<ShipSlotInfo> shipSlotList)
        {
            var infoList = GetIEnumerable(shipSlotList);
            foreach (var info in shipSlotList.Where(x => x.Saiun))
            {
                var min = info.MinSlotNum;

                var saiun = infoList.Where(x => x.Slot.Item1 == min && x.AirCraft.Item1.AirCraftName == "彩雲").First();
                writer.WriteLine("+ " + saiun.SlotName + @" = 1 \ " + "彩雲");
                writer.WriteLine();
            }

            foreach (var info in shipSlotList.Where(x => x.MaintenancePersonnel))
            {
                var min = info.MinSlotNum;

                var saiun = infoList.Where(x => x.Slot.Item1 == min && x.AirCraft.Item1.AirCraftName == "熟練艦載機整備員").First();
                writer.WriteLine("+ " + saiun.SlotName + @" = 1 \ " + "熟練艦載機整備員");
                writer.WriteLine();
            }

            foreach (var info in shipSlotList.Where(x => x.MinimumSlot))
            {
                var min = info.MinSlotNum;
                var list = infoList.Where(x => x.Slot.Item1 == min && (x.AirCraft.Item1.Type == Consts.TorpedoBomber || x.AirCraft.Item1.Type == Consts.DiveBomber));
                if (list.Any())
                {
                    foreach (var i in list)
                    {
                        writer.WriteLine("+ " + i.SlotName + @" \ " + "最小スロット攻撃機");
                    }
                    writer.WriteLine("= 0");
                    writer.WriteLine();
                }
            }
        }

        private static void OutputBinary(StreamWriter writer, IEnumerable<ShipSlotInfo> shipSlotList)
        {
            writer.WriteLine("binary");
            foreach (var record in GetIEnumerable(shipSlotList))
            {
                writer.WriteLine(record.SlotName + @" \ " + record.Ship.Item1.Name + " " + record.Slot.Item1 + " " + record.AirCraft.Item1.AirCraftName);
            }
            writer.WriteLine();

        }

        private static void GenerateSolveFile(string dir)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "solve.txt"), false, new UTF8Encoding(false)))
            {
                writer.WriteLine("read slot.lp");
                writer.WriteLine("optimize");
                writer.WriteLine("display solution");
                writer.WriteLine("quit");
            }
        }

        private static List<string> CalcProcess(string dir)
        {
            var slotStringList = new List<string>();

            var logFile = Path.Combine(dir, "result.log");

            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }

            var psi = new ProcessStartInfo();

            psi.FileName = Properties.Settings.Default.SolverPath;
            psi.Arguments = "-b solve.txt" + " -l result.log";
            psi.WorkingDirectory = dir;

            var process = Process.Start(psi);

            process.WaitForExit();

            var log = Path.Combine(dir, "result.log");
            var regex = new Regex(@"(?<slot>slot_\d+_\d_\d+).+");
            using (StreamReader r = new StreamReader(log))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    var matches = regex.Matches(line);
                    if (matches.Count > 0)
                    {
                        slotStringList.Add(matches[0].Groups["slot"].Value);
                    }
                }
            }

            return slotStringList;
        }

        private static void CalcResultViewProcess(List<string> slotStringList, IEnumerable<ShipSlotInfo> shipSlotList)
        {
            foreach (var generatorInfo in slotStringList.Select(x => GetIEnumerable(shipSlotList).First(y => y.SlotName == x)))
            {
                var ship = shipSlotList.First(x => x.ShipName == generatorInfo.Ship.Item1.Name);
                if (generatorInfo.Slot.Item2 == 0)
                {
                    ship.Slot1 = generatorInfo.AirCraft.Item1;
                }
                else if (generatorInfo.Slot.Item2 == 1)
                {
                    ship.Slot2 = generatorInfo.AirCraft.Item1;
                }
                else if (generatorInfo.Slot.Item2 == 2)
                {
                    ship.Slot3 = generatorInfo.AirCraft.Item1;
                }
                else if (generatorInfo.Slot.Item2 == 3)
                {
                    ship.Slot4 = generatorInfo.AirCraft.Item1;
                }
            }
        }
    }
}
