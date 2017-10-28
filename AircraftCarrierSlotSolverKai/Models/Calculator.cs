using Google.OrTools.LinearSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public static class Calculator
    {
        /// <summary>
        /// 計算処理
        /// </summary>
        /// <param name="airSuperiority"></param>
        /// <param name="shipSlotInfos"></param>
        /// <returns></returns>
        public static async Task<(bool result, string message)> Calc(int airSuperiority, IEnumerable<ShipSlotInfo> shipSlotInfos) => await Task.Run(() =>
        {
            try
            {
                if (!shipSlotInfos.Any())
                {
                    return (false, "艦娘が追加されていないため計算実行できません。");
                }

                // 表示初期化
                ResetViewProcess(shipSlotInfos);

                // ソルバー生成
                var solver = Solver.CreateSolver("IntegerProgramming", "CBC_MIXED_INTEGER_PROGRAMMING");

                // 変数作成
                var variables = CreateVariable(solver, shipSlotInfos);

                // 制約条件(制空値)
                AirConstraints(solver, variables, airSuperiority);

                // 制約条件(スロット)
                SlotConstraints(solver, variables);

                // 制約条件(所持数)
                StockConstraints(solver, variables);

                // 制約条件(艦載機設定)
                AirCraftSettingConstraints(solver, variables, shipSlotInfos);

                // 制約条件(装備種)
                AirCraftTypeConstraints(solver, variables, shipSlotInfos);

                // 目的関数
                SetGoal(solver, variables);

                // ソルバー実行
                var resultStatus = solver.Solve();

                if (resultStatus != Solver.OPTIMAL)
                {
                    return (false, "制空値を満たす解がありませんでした。");
                }

                // 結果表示
                CalcResultViewProcess(solver, variables, shipSlotInfos);

                return (true, string.Empty);
            }
            catch (Exception e)
            {
                return (false, $"計算に失敗しました。{e.Message}");
            }
        });

        /// <summary>
        /// 制約条件(艦載機設定)
        /// </summary>
        /// <param name="solver"></param>
        /// <param name="variables"></param>
        /// <param name="shipSlotInfos"></param>
        private static void AirCraftSettingConstraints(Solver solver, List<Variable> variables, IEnumerable<ShipSlotInfo> shipSlotInfos)
        {
            // 攻撃機を必ず積む
            AirCraftSettingConstraintsDetail(solver, variables, shipSlotInfos, x => x.Attack, AttackFilter);

            // 彩雲を最小スロットに積む
            AirCraftSettingConstraintsDetail(solver, variables, shipSlotInfos, x => x.Saiun, SaiunFilter);

            // 熟練艦載機整備員を最小スロットに積む
            AirCraftSettingConstraintsDetail(solver, variables, shipSlotInfos, x => x.MaintenancePersonnel, SaiunFilter);

            // 攻撃機を最小スロットに積まない
            AirCraftSettingConstraintsDetail(solver, variables, shipSlotInfos, x => x.MinimumSlot, MinimumSlotFilter, double.NegativeInfinity, 0);

            // 攻撃機を第一スロットに積む
            AirCraftSettingConstraintsDetail(solver, variables, shipSlotInfos, x => x.FirstSlotAttack, FirstAttackFilter);

            // 攻撃機のみ積む
            AirCraftSettingConstraintsDetail(solver, variables, shipSlotInfos, x => x.OnlyAttacker, OnlyAttackerFilter, double.NegativeInfinity, 0);

            // 艦載機指定
            foreach (var shipSlotInfo in shipSlotInfos)
            {
                foreach (var airCraftInfos in shipSlotInfo.SlotSettings.Where(x => x.airCraft != null))
                {
                    var constraint = solver.MakeConstraint(1, 1);

                    var info = GetInfoListFromVariables(variables).First(x =>
                                                                    x.shipId == shipSlotInfo.ShipInfo.ID &&
                                                                    x.airCraftId == airCraftInfos.airCraft.Id &&
                                                                    x.improvement == airCraftInfos.airCraft.Improvement &&
                                                                    x.slotIndex == airCraftInfos.index);

                    constraint.SetCoefficient(info.variable, 1);
                }
            }

            // 水上戦闘機
            foreach (var shipSlotInfo in shipSlotInfos.Where(x => x.SeaplaneFighterNumEnable))
            {
                var constraint = solver.MakeConstraint(double.NegativeInfinity, shipSlotInfo.SeaplaneFighterNum);

                foreach (var info in GetInfoListFromVariables(variables).Where(x => x.shipId == shipSlotInfo.ShipInfo.ID)
                                                                            .Select(y => (y.variable, AirCraftRecords.Instance.Records.First(z => z.Id == y.airCraftId)))
                                                                            .Where(i => i.Item2.Type.Equals("水上戦闘機")))
                {
                    constraint.SetCoefficient(info.variable, 1);
                }
            }

            // 水上爆撃機
            foreach (var shipSlotInfo in shipSlotInfos.Where(x => x.SeaplaneBomberNumEnable))
            {
                var constraint = solver.MakeConstraint(double.NegativeInfinity, shipSlotInfo.SeaplaneBomberNum);

                foreach (var info in GetInfoListFromVariables(variables).Where(x => x.shipId == shipSlotInfo.ShipInfo.ID)
                                                                            .Select(y => (y.variable, AirCraftRecords.Instance.Records.First(z => z.Id == y.airCraftId)))
                                                                            .Where(i => i.Item2.Type.Equals("水上爆撃機")))
                {
                    constraint.SetCoefficient(info.variable, 1);
                }
            }

            // その他艦種用スロット装備数
            foreach (var shipSlotInfo in shipSlotInfos.Where(x => x.EquipSlotNumEnable))
            {
                var constraint = solver.MakeConstraint(double.NegativeInfinity, shipSlotInfo.EquipSlotNum);

                foreach (var info in GetInfoListFromVariables(variables).Where(x => x.shipId == shipSlotInfo.ShipInfo.ID))
                {
                    constraint.SetCoefficient(info.variable, 1);
                }
            }

            // 航空要員自動設定
            foreach (var shipSlotInfo in shipSlotInfos.Where(x => x.AutoMaintenancePersonnel))
            {
                var settings = shipSlotInfo.SlotSettings.Where(x => x.airCraft != null);

                var count = settings.Any() ? settings.Select(x => AirCraftRecords.Instance.Records.First(y => y.Id == x.airCraft.Id)).Count(z => z.Type.Equals("航空要員")) : 0;

                var constraint = solver.MakeConstraint(double.NegativeInfinity, count);

                foreach (var info in GetInfoListFromVariables(variables).Where(x => x.shipId == shipSlotInfo.ShipInfo.ID)
                                                                           .Select(y => (y.variable, AirCraftRecords.Instance.Records.First(z => z.Id == y.airCraftId)))
                                                                           .Where(i => i.Item2.Type.Equals("航空要員")))
                {
                    constraint.SetCoefficient(info.variable, 1);
                }
            }
        }

        /// <summary>
        /// 制約条件(艦載機設定)共通処理
        /// </summary>
        /// <param name="solver"></param>
        /// <param name="variables"></param>
        /// <param name="shipSlotInfos"></param>
        /// <param name="filter"></param>
        /// <param name="predicate"></param>
        /// <param name="lb"></param>
        /// <param name="ub"></param>
        private static void AirCraftSettingConstraintsDetail(Solver solver,
                                                                List<Variable> variables,
                                                                IEnumerable<ShipSlotInfo> shipSlotInfos,
                                                                Func<ShipSlotInfo, bool> filter,
                                                                Func<ShipSlotInfo, Func<(Variable variable, int shipId, int airCraftId, int improvement, int slotIndex), bool>> predicate,
                                                                double lb = 1,
                                                                double ub = double.PositiveInfinity)
        {
            foreach (var ship in shipSlotInfos.Where(filter))
            {
                var constraint = solver.MakeConstraint(lb, ub);

                foreach (var info in GetInfoListFromVariables(variables).Where(predicate(ship)))
                {
                    constraint.SetCoefficient(info.variable, 1);
                }
            }
        }

        /// <summary>
        /// 制約条件(艦載機設定)フィルタ(攻撃機を第一スロットに積む)
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static Func<(Variable variable, int shipId, int airCraftId, int improvement, int slotIndex), bool> FirstAttackFilter(ShipSlotInfo ship) =>
                                v => v.shipId == ship.ShipInfo.ID &&
                                GetAirCraft(v.airCraftId, v.improvement).Attackable &&
                                v.slotIndex == 1;

        /// <summary>
        /// 制約条件(艦載機設定)フィルタ(攻撃機を最小スロットに積まない)
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static Func<(Variable variable, int shipId, int airCraftId, int improvement, int slotIndex), bool> MinimumSlotFilter(ShipSlotInfo ship) =>
                                v => v.shipId == ship.ShipInfo.ID &&
                                GetAirCraft(v.airCraftId, v.improvement).Attackable &&
                                v.slotIndex == ship.MinSlotIndex;

        /// <summary>
        /// 制約条件(艦載機設定)フィルタ(熟練艦載機整備員を最小スロットに積む)
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static Func<(Variable variable, int shipId, int airCraftId, int improvement, int slotIndex), bool> MaintenancePersonnelFilter(ShipSlotInfo ship) =>
                                        v => v.shipId == ship.ShipInfo.ID &&
                                        GetAirCraft(v.airCraftId, v.improvement).Name.Contains("熟練艦載機整備員") &&
                                        v.slotIndex == ship.MinSlotIndex;

        /// <summary>
        /// 制約条件(艦載機設定)フィルタ(彩雲を最小スロットに積む)
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static Func<(Variable variable, int shipId, int airCraftId, int improvement, int slotIndex), bool> SaiunFilter(ShipSlotInfo ship) => 
                                        v => v.shipId == ship.ShipInfo.ID &&
                                        GetAirCraft(v.airCraftId, v.improvement).Name.Contains("彩雲") &&
                                        v.slotIndex == ship.MinSlotIndex;

        /// <summary>
        /// 制約条件(艦載機設定)フィルタ(攻撃機を必ず積む)
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static Func<(Variable variable, int shipId, int airCraftId, int improvement, int slotIndex), bool> AttackFilter(ShipSlotInfo ship) => 
                                        v => v.shipId == ship.ShipInfo.ID &&
                                        GetAirCraft(v.airCraftId, v.improvement).Attackable;

        /// <summary>
        /// 制約条件(艦載機設定)フィルタ(攻撃機のみ積む)
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static Func<(Variable variable, int shipId, int airCraftId, int improvement, int slotIndex), bool> OnlyAttackerFilter(ShipSlotInfo ship) =>
                        v => v.shipId == ship.ShipInfo.ID &&
                        !GetAirCraft(v.airCraftId, v.improvement).Attackable;

        /// <summary>
        /// 変数リスト取得
        /// </summary>
        /// <param name="variables"></param>
        /// <returns></returns>
        private static IEnumerable<(Variable variable, int shipId, int airCraftId, int improvement, int slotIndex)> GetInfoListFromVariables(IEnumerable<Variable> variables) => variables.Select(variable =>
        {
            var info = Parse(variable.Name());
            return (variable, info.shipId, info.airCraftId, info.improvement, info.slotIndex);
        });


        /// <summary>
        /// 結果表示
        /// </summary>
        /// <param name="solver"></param>
        /// <param name="variables"></param>
        /// <param name="shipSlotInfos"></param>
        private static void CalcResultViewProcess(Solver solver, List<Variable> variables, IEnumerable<ShipSlotInfo> shipSlotInfos)
        {
            foreach (var answer in GetInfoListFromVariables(variables.Where(x => x.SolutionValue() > 0)))
            {
                var airCraft = GetAirCraft(answer.airCraftId, answer.improvement);

                var shipSlotInfo = shipSlotInfos.First(x => x.ShipInfo.ID == answer.shipId);

                if (answer.slotIndex == 1)
                {
                    shipSlotInfo.Slot1 = airCraft;
                }
                else if (answer.slotIndex == 2)
                {
                    shipSlotInfo.Slot2 = airCraft;
                }
                else if (answer.slotIndex == 3)
                {
                    shipSlotInfo.Slot3 = airCraft;
                }
                else if (answer.slotIndex == 4)
                {
                    shipSlotInfo.Slot4 = airCraft;
                }
            }
        }

        /// <summary>
        /// 表示初期化
        /// </summary>
        /// <param name="shipSlotInfos"></param>
        private static void ResetViewProcess(IEnumerable<ShipSlotInfo> shipSlotInfos)
        {
            foreach (var shipSlotInfo in shipSlotInfos)
            {
                shipSlotInfo.Slot1 = null;
                shipSlotInfo.Slot2 = null;
                shipSlotInfo.Slot3 = null;
                shipSlotInfo.Slot4 = null;
            }
        }

        /// <summary>
        /// 制約条件(所持数)
        /// </summary>
        /// <param name="solver"></param>
        /// <param name="variables"></param>
        private static void StockConstraints(Solver solver, List<Variable> variables)
        {
            foreach (var setting in AirCraftSettingRecords.Instance.Records)
            {
                var constraint = solver.MakeConstraint(double.NegativeInfinity, setting.Value);

                foreach (var info in GetInfoListFromVariables(variables)
                    .Where(x => x.airCraftId == setting.AirCraft.Id && x.improvement == setting.Improvement))
                {
                    constraint.SetCoefficient(info.variable, 1);
                }
            }
        }

        /// <summary>
        /// 目的関数
        /// </summary>
        /// <param name="solver"></param>
        /// <param name="variables"></param>
        private static void SetGoal(Solver solver, List<Variable> variables)
        {
            var objective = solver.Objective();
            // 火力+命中+回避
            foreach (var info in GetInfoListFromVariables(variables))
            {
                var airCraft = GetAirCraft(info.airCraftId, info.improvement);
                var slotNum = GetSlotNum(info.shipId, info.slotIndex);

                objective.SetCoefficient(info.variable, airCraft.Accuracy + airCraft.Evasion + airCraft.Power(slotNum));
            }
            objective.SetMaximization();
        }

        /// <summary>
        /// 制約条件(スロット)
        /// </summary>
        /// <param name="solver"></param>
        /// <param name="variables"></param>
        private static void SlotConstraints(Solver solver, List<Variable> variables)
        {
            foreach (var slotGroups in GetInfoListFromVariables(variables).GroupBy(x => (x.shipId, x.slotIndex)))
            {
                var constraint = solver.MakeConstraint(double.NegativeInfinity, 1);
                foreach (var slot in slotGroups)
                {
                    constraint.SetCoefficient(slot.variable, 1);
                }
            }
        }

        /// <summary>
        /// 制約条件(制空値)
        /// </summary>
        private static void AirConstraints(Solver solver, List<Variable> variables, int airSuperiority)
        {
            var constraint = solver.MakeConstraint(airSuperiority, double.PositiveInfinity);

            foreach (var info in GetInfoListFromVariables(variables))
            {
                var airCraft = GetAirCraft(info.airCraftId, info.improvement);
                var slotNum = GetSlotNum(info.shipId, info.slotIndex);

                constraint.SetCoefficient(info.variable, airCraft.AirSuperiorityPotential(slotNum));
            }
        }

        /// <summary>
        /// 制約条件(装備種)
        /// </summary>
        /// <param name="solver"></param>
        /// <param name="variables"></param>
        /// <param name="shipSlotInfos"></param>
        private static void AirCraftTypeConstraints(Solver solver, List<Variable> variables, IEnumerable<ShipSlotInfo> shipSlotInfos)
        {
            var dic = new Dictionary<string, Func<ShipSlotInfo, bool>>()
            {
                { "艦上攻撃機" , x => x.ShipInfo.Type.Contains("空母") || x.ShipInfo.Name.Contains("速吸") },
                { "艦上戦闘機" , x => x.ShipInfo.Type.Contains("空母") || x.ShipInfo.Type.Equals("揚陸艦") },
                { "艦上偵察機" , x => x.ShipInfo.Type.Contains("空母") },
                { "艦上爆撃機" , x => x.ShipInfo.Type.Contains("空母") },
                { "航空要員" , x => x.ShipInfo.Type.Contains("空母") || new []{"航空戦艦", "航空巡洋艦"}.Contains(x.ShipInfo.Type) || new[]{ "由良改二", "Zara due", "速吸", "Commandant Teste", "秋津洲改" }.Any(y => x.ShipInfo.Name.Contains(y)) },
                { "水上偵察機" , x => x.ShipInfo.Type.Contains("戦艦") || x.ShipInfo.Type.Contains("巡洋艦")},
                { "水上戦闘機" , x => x.ShipInfo.Type.Contains("空母") || new []{"水上機母艦", "航空戦艦", "航空巡洋艦", "潜水空母"}.Contains(x.ShipInfo.Type) || new [] {"Zara due", "Italia", "Roma改", "長門", "陸奥", "大和", "武蔵"}.Any(y => x.ShipInfo.Name.Contains(y)) },
                { "水上爆撃機" , x => new []{"水上機母艦", "航空戦艦", "航空巡洋艦", "潜水空母", "補給艦"}.Contains(x.ShipInfo.Type) || new [] {"Zara due", "Italia", "Roma改"}.Any(y => x.ShipInfo.Name.Contains(y)) },
                { "噴式戦闘爆撃機" , x => Regex.IsMatch(x.ShipInfo.Name, "(翔鶴|瑞鶴)改二甲") },
            };

            var constraint = solver.MakeConstraint(double.NegativeInfinity, 0);

            foreach (var info in GetInfoListFromVariables(variables).Select(x => (x.variable,
                                                                                    shipSlotInfos.First(y => y.ShipInfo.ID == x.shipId),
                                                                                    AirCraftRecords.Instance.Records.First(y => y.Id == x.airCraftId)))
                                                                    .Where(z => !dic[z.Item3.Type](z.Item2)))
            {
                constraint.SetCoefficient(info.variable, 1);
            }
        }

        /// <summary>
        /// 艦載機取得
        /// </summary>
        /// <param name="airCraftId"></param>
        /// <param name="improvement"></param>
        /// <returns></returns>
        private static AirCraft GetAirCraft(int airCraftId, int improvement) => AirCraftSettingRecords.Instance.Records.ToList().Find(x => x.AirCraft.Id == airCraftId && x.AirCraft.Improvement == improvement).AirCraft;

        /// <summary>
        /// スロット数取得
        /// </summary>
        /// <param name="shipId"></param>
        /// <param name="slotIndex"></param>
        /// <returns></returns>
        private static int GetSlotNum(int shipId, int slotIndex)
        {
            var ship = ShipRecords.Instance.Records.ToList().Find(x => x.ID == shipId);
            switch(slotIndex)
            {
                case 1:
                    return ship.Slot1Num;
                case 2:
                    return ship.Slot2Num;
                case 3:
                    return ship.Slot3Num;
                case 4:
                    return ship.Slot4Num;
                default:
                    throw new ArgumentException(slotIndex.ToString());
            }
        }

        /// <summary>
        /// 変数名から各種情報取り出し
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns></returns>
        private static (int shipId, int airCraftId, int improvement, int slotIndex) Parse(string variableName)
        {
            var val = variableName.Split('_');

            return (int.Parse(val[1]), int.Parse(val[2]), int.Parse(val[3]), int.Parse(val[4]));
        }

        /// <summary>
        /// 変数作成
        /// </summary>
        /// <param name="solver"></param>
        /// <param name="shipSlotInfos"></param>
        /// <returns></returns>
        private static List<Variable> CreateVariable(Solver solver, IEnumerable<ShipSlotInfo> shipSlotInfos) => 
                                shipSlotInfos.SelectMany(shipSlotInfo =>
                                    AirCraftSettingRecords.Instance.Records
                                        .Select(y => (shipSlotInfo, y)))
                                        .SelectMany(z => Enumerable.Range(1, z.shipSlotInfo.ShipInfo.SlotNum)
                                        .Select(slotIndex => (z.shipSlotInfo, z.y, slotIndex)))
                                        .Select(i => solver.MakeBoolVar($"_{i.shipSlotInfo.ShipInfo.ID}_{i.y.AirCraft.Id}_{i.y.AirCraft.Improvement}_{i.slotIndex}"))
                                        .ToList();
    }
}
