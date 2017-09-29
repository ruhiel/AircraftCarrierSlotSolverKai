using Google.OrTools.LinearSolver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AircraftCarrierSlotSolverKai.Models
{
    public static class Calculator
    {
        public static (bool result, string message) Calc(int airSuperiority, IEnumerable<ShipSlotInfo> shipSlotInfos)
        {
            if (!shipSlotInfos.Any())
            {
                return (false, "艦娘が追加されていないため計算実行できません。");
            }

            var solver = Solver.CreateSolver("IntegerProgramming", "CBC_MIXED_INTEGER_PROGRAMMING");

            var variables = CreateVariable(solver, shipSlotInfos);

            // 制約条件(制空値)
            AirConstraints(solver, variables, airSuperiority);

            // 制約条件(スロット)
            SlotConstraints(solver, variables);

            // 制約条件(所持数)
            StockConstraints(solver, variables);

            // 制約条件(艦載機設定)
            AirCraftSettingConstraints(solver, variables, shipSlotInfos);

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
        }

        private static void AirCraftSettingConstraintsDetail(Solver solver,
                                                                List<Variable> variables,
                                                                IEnumerable<ShipSlotInfo> shipSlotInfos,
                                                                Func<ShipSlotInfo, bool> filter,
                                                                Func<ShipSlotInfo, Func<(Variable variable, (int shipId, int airCraftId, int improvement, int slotIndex)), bool>> predicate,
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

        private static Func<(Variable variable, (int shipId, int airCraftId, int improvement, int slotIndex)), bool> FirstAttackFilter(ShipSlotInfo ship) =>
                                v => v.Item2.shipId == ship.ShipInfo.ID &&
                                GetAirCraft(v.Item2.airCraftId, v.Item2.improvement).Attackable &&
                                v.Item2.slotIndex == 1;

        private static Func<(Variable variable, (int shipId, int airCraftId, int improvement, int slotIndex)), bool> MinimumSlotFilter(ShipSlotInfo ship) =>
                                v => v.Item2.shipId == ship.ShipInfo.ID &&
                                GetAirCraft(v.Item2.airCraftId, v.Item2.improvement).Attackable &&
                                v.Item2.slotIndex == ship.MinSlotIndex;

        private static Func<(Variable variable, (int shipId, int airCraftId, int improvement, int slotIndex)), bool> MaintenancePersonnelFilter(ShipSlotInfo ship) =>
                                        v => v.Item2.shipId == ship.ShipInfo.ID &&
                                        GetAirCraft(v.Item2.airCraftId, v.Item2.improvement).Name.Contains("熟練艦載機整備員") &&
                                        v.Item2.slotIndex == ship.MinSlotIndex;

        private static Func<(Variable variable, (int shipId, int airCraftId, int improvement, int slotIndex)), bool> SaiunFilter(ShipSlotInfo ship) => 
                                        v => v.Item2.shipId == ship.ShipInfo.ID &&
                                        GetAirCraft(v.Item2.airCraftId, v.Item2.improvement).Name.Contains("彩雲") &&
                                        v.Item2.slotIndex == ship.MinSlotIndex;

        private static Func<(Variable variable, (int shipId, int airCraftId, int improvement, int slotIndex)), bool> AttackFilter(ShipSlotInfo ship) => 
                                        v => v.Item2.shipId == ship.ShipInfo.ID &&
                                        GetAirCraft(v.Item2.airCraftId, v.Item2.improvement).Attackable;

        private static Func<(Variable variable, (int shipId, int airCraftId, int improvement, int slotIndex)), bool> OnlyAttackerFilter(ShipSlotInfo ship) =>
                        v => v.Item2.shipId == ship.ShipInfo.ID &&
                        !GetAirCraft(v.Item2.airCraftId, v.Item2.improvement).Attackable;

        /// <summary>
        /// 変数リスト取得
        /// </summary>
        /// <param name="variables"></param>
        /// <returns></returns>
        private static IEnumerable<(Variable variable, (int shipId, int airCraftId, int improvement, int slotIndex))> GetInfoListFromVariables(IEnumerable<Variable> variables) => variables.Select(variable => (variable, Parse(variable.Name())));


        /// <summary>
        /// 結果表示
        /// </summary>
        /// <param name="solver"></param>
        /// <param name="variables"></param>
        /// <param name="shipSlotInfos"></param>
        private static void CalcResultViewProcess(Solver solver, List<Variable> variables, IEnumerable<ShipSlotInfo> shipSlotInfos)
        {
            foreach(var shipSlotInfo in shipSlotInfos)
            {
                shipSlotInfo.Slot1 = null;
                shipSlotInfo.Slot2 = null;
                shipSlotInfo.Slot3 = null;
                shipSlotInfo.Slot4 = null;
            }

            foreach (var answer in GetInfoListFromVariables(variables.Where(x => x.SolutionValue() > 0)))
            {
                var airCraft = GetAirCraft(answer.Item2.airCraftId, answer.Item2.improvement);

                var shipSlotInfo = shipSlotInfos.First(x => x.ShipInfo.ID == answer.Item2.shipId);

                if(answer.Item2.slotIndex == 1)
                {
                    shipSlotInfo.Slot1 = airCraft;
                }
                else if(answer.Item2.slotIndex == 2)
                {
                    shipSlotInfo.Slot2 = airCraft;
                }
                else if (answer.Item2.slotIndex == 3)
                {
                    shipSlotInfo.Slot3 = airCraft;
                }
                else if (answer.Item2.slotIndex == 4)
                {
                    shipSlotInfo.Slot4 = airCraft;
                }
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
                    .Where(x => x.Item2.airCraftId == setting.AirCraft.Id && x.Item2.improvement == setting.Improvement))
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
            // 火力
            foreach (var info in GetInfoListFromVariables(variables))
            {
                var airCraft = GetAirCraft(info.Item2.airCraftId, info.Item2.improvement);
                var slotNum = GetSlotNum(info.Item2.shipId, info.Item2.slotIndex);

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
            foreach (var slotGroups in GetInfoListFromVariables(variables).GroupBy(x => (x.Item2.shipId, x.Item2.slotIndex)))
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
                var airCraft = GetAirCraft(info.Item2.airCraftId, info.Item2.improvement);
                var slotNum = GetSlotNum(info.Item2.shipId, info.Item2.slotIndex);

                constraint.SetCoefficient(info.variable, airCraft.AirSuperiorityPotential(slotNum));
            }
        }

        private static AirCraft GetAirCraft(int airCraftId, int improvement) => AirCraftSettingRecords.Instance.Records.Find(x => x.AirCraft.Id == airCraftId && x.AirCraft.Improvement == improvement).AirCraft;

        private static int GetSlotNum(int shipId, int slotIndex)
        {
            var ship = ShipInfoRecords.Instance.Records.Find(x => x.ID == shipId);
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

        private static (int shipId, int airCraftId, int improvement, int slotIndex) Parse(string variableName)
        {
            var val = variableName.Split('_');

            return (int.Parse(val[1]), int.Parse(val[2]), int.Parse(val[3]), int.Parse(val[4]));
        }

        private static List<Variable> CreateVariable(Solver solver, IEnumerable<ShipSlotInfo> shipSlotInfos)
        {
            var variables = new List<Variable>();
            var list = shipSlotInfos.SelectMany(ship =>
                                AirCraftSettingRecords.Instance.Records
                                .Where(y => Equippable(ship.ShipInfo, y.AirCraft))
                                .Select(airCraft => (ship, airCraft))).SelectMany(a => Enumerable.Range(1, a.ship.ShipInfo.SlotNum).Select(slotIndex => (a.ship, a.airCraft, slotIndex))).ToList();

            foreach (var item in list)
            {
                var variable = solver.MakeBoolVar($"_{item.ship.ShipInfo.ID}_{item.airCraft.AirCraft.Id}_{item.airCraft.AirCraft.Improvement}_{item.slotIndex}");

                variables.Add(variable);
            }

            return variables;
        }

        private static bool Equippable(ShipInfo ship, AirCraft airCraft)
        {
            Func<AirCraft, bool> predicate;

            if (IsSeaplaneEquippable(ship.Type))
            {
                if (ship.Type.Contains("航空"))
                {
                    predicate = (x) => x.Type == Consts.SeaplaneBomber || x.Type == Consts.SeaplaneFighter || x.Type == Consts.AviationPersonnel || x.AirCraftName == "装備なし";
                }
                else
                {
                    predicate = (x) => x.Type == Consts.SeaplaneBomber || x.Type == Consts.SeaplaneFighter || x.AirCraftName == "装備なし";
                }
            }
            else if (ship.Type == "揚陸艦")
            {
                predicate = (x) => x.Type == Consts.Fighter || x.AirCraftName == "装備なし";
            }
            else if (ship.Type == "補給艦")
            {
                predicate = (x) => x.Type == Consts.TorpedoBomber || x.AirCraftName == "装備なし";
            }
            else
            {
                predicate = (x) => x.Type == Consts.TorpedoBomber || x.Type == Consts.DiveBomber || x.Type == Consts.Fighter || x.Type == Consts.JetBomber || x.Type == Consts.ReconAircraft || x.Type == Consts.AviationPersonnel || x.AirCraftName == "装備なし";
            }

            return predicate(airCraft);
        }

        private static bool IsSeaplaneEquippable(string shipType)
        {
            switch (shipType)
            {
                case "航空巡洋艦":
                case "水上機母艦":
                case "航空戦艦":
                case "潜水空母":
                    return true;
                default:
                    return false;
            }
        }
    }
}
