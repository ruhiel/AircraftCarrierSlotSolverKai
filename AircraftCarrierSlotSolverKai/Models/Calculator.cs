﻿using AircraftCarrierSlotSolverKai.Models.Records;
using Google.OrTools.LinearSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static AircraftCarrierSlotSolverKai.Models.NightCVCI;
using static AircraftCarrierSlotSolverKai.Models.Records.CVCIRecords;

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
        public static async Task<(bool result, string message, int resultAirSuperiority)> Calc(int airSuperiority, IEnumerable<ShipSlotInfo> shipSlotInfos) => await Task.Run(() =>
        {
            try
            {
                if (!shipSlotInfos.Any())
                {
                    return (false, "艦娘が追加されていないため計算実行できません。", 0);
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
                    return (false, "制空値を満たす解がありませんでした。", 0);
                }

                // 結果表示
                CalcResultViewProcess(solver, variables, shipSlotInfos);

                return (true, string.Empty, shipSlotInfos.Sum(x => x.EquipedSlots.Sum(y => y.Item2.AirCraft.AirSuperiorityPotential(y.Item1))));
            }
            catch (Exception e)
            {
                return (false, $"計算に失敗しました。{e.Message}", 0);
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
            AirCraftSettingConstraintsDetail(solver, variables, shipSlotInfos, x => x.MaintenancePersonnel, MaintenancePersonnelFilter);

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
                                                                    x.ship.ID == shipSlotInfo.ShipInfo.ID &&
                                                                    x.airCraft.Id == airCraftInfos.airCraft.Id &&
                                                                    x.improvement == airCraftInfos.airCraft.Improvement &&
                                                                    x.slotIndex == airCraftInfos.index);

                    constraint.SetCoefficient(info.variable, 1);
                }
            }

            // 水上戦闘機
            foreach (var shipSlotInfo in shipSlotInfos.Where(x => x.SeaplaneFighterNumEnable))
            {
                var constraint = solver.MakeConstraint(double.NegativeInfinity, shipSlotInfo.SeaplaneFighterNum);

                foreach (var info in GetInfoListFromVariables(variables).Where(x => x.ship.ID == shipSlotInfo.ShipInfo.ID && x.airCraft.Type.Equals(Consts.SeaplaneFighter)))
                {
                    constraint.SetCoefficient(info.variable, 1);
                }
            }

            // 水上爆撃機
            foreach (var shipSlotInfo in shipSlotInfos.Where(x => x.SeaplaneBomberNumEnable))
            {
                var constraint = solver.MakeConstraint(double.NegativeInfinity, shipSlotInfo.SeaplaneBomberNum);

                foreach (var info in GetInfoListFromVariables(variables).Where(x => x.ship.ID == shipSlotInfo.ShipInfo.ID && x.airCraft.Type.Equals(Consts.SeaplaneBomber)))
                {
                    constraint.SetCoefficient(info.variable, 1);
                }
            }

            // その他艦種用スロット装備数
            foreach (var shipSlotInfo in shipSlotInfos.Where(x => x.EquipSlotNumEnable))
            {
                var constraint = solver.MakeConstraint(double.NegativeInfinity, shipSlotInfo.EquipSlotNum);

                foreach (var info in GetInfoListFromVariables(variables).Where(x => x.ship.ID == shipSlotInfo.ShipInfo.ID))
                {
                    constraint.SetCoefficient(info.variable, 1);
                }
            }

            // 航空要員自動設定
            foreach (var shipSlotInfo in shipSlotInfos.Where(x => x.AutoMaintenancePersonnel))
            {
                var settings = shipSlotInfo.SlotSettings.Where(x => x.airCraft != null);

                var count = settings.Any() ? settings.Select(x => AirCraftRecords.Instance.Records.First(y => y.Id == x.airCraft.Id)).Count(z => z.Type.Equals(Consts.AviationPersonnel)) : 0;

                var constraint = solver.MakeConstraint(double.NegativeInfinity, count);

                foreach (var info in GetInfoListFromVariables(variables).Where(x => x.ship.ID == shipSlotInfo.ShipInfo.ID && x.airCraft.Type.Equals(Consts.AviationPersonnel)))
                {
                    constraint.SetCoefficient(info.variable, 1);
                }
            }

            // 戦爆連合カットイン
            foreach (var shipSlotInfo in shipSlotInfos.Where(x => x.CVCI))
            {
                // 種別取得
                var type = shipSlotInfo.CVCIType.FirstOrDefault(x => x.IsSelected)?.Type ?? CIType.DIVE_BOMBER_TORPEDO_BOMBER;

                // 艦上爆撃機
                if(type.HasFlag(CIType.DIVE_BOMBER) || type.HasFlag(CIType.DIVE_BOMBER2))
                {
                    var constraint = solver.MakeConstraint(type.HasFlag(CIType.DIVE_BOMBER) ? 1 : 2, double.PositiveInfinity);

                    foreach (var info in GetInfoListFromVariables(variables).Where(x => x.ship.ID == shipSlotInfo.ShipInfo.ID && x.airCraft.Type.Equals(Consts.DiveBomber)))
                    {
                        constraint.SetCoefficient(info.variable, 1);
                    }
                }

                // 艦上攻撃機
                if (type.HasFlag(CIType.TORPEDO_BOMBER))
                {
                    var constraint = solver.MakeConstraint(1, double.PositiveInfinity);

                    foreach (var info in GetInfoListFromVariables(variables).Where(x => x.ship.ID == shipSlotInfo.ShipInfo.ID && x.airCraft.Type.Equals(Consts.TorpedoBomber)))
                    {
                        constraint.SetCoefficient(info.variable, 1);
                    }
                }

                // 艦上戦闘機
                if (type.HasFlag(CIType.FIGHTER))
                {
                    var constraint = solver.MakeConstraint(1, double.PositiveInfinity);

                    foreach (var info in GetInfoListFromVariables(variables).Where(x => x.ship.ID == shipSlotInfo.ShipInfo.ID && x.airCraft.Type.Equals(Consts.Fighter)))
                    {
                        constraint.SetCoefficient(info.variable, 1);
                    }
                }
            }

            // 夜襲カットイン
            foreach (var shipSlotInfo in shipSlotInfos.Where(x => x.NightCVCI))
            {
                // 種別取得
                var type = shipSlotInfo.NightCVCIList.FirstOrDefault(x => x.IsSelected)?.Type ?? NightCVCIType.NIGHT_FIGHTER_NIGHT_BOMBER;

                // 夜戦
                if(type.HasFlag(NightCVCIType.NIGHT_FIGHTER) || type.HasFlag(NightCVCIType.NIGHT_FIGHTER2) || type.HasFlag(NightCVCIType.NIGHT_FIGHTER3))
                {
                    int num;
                    if (type.HasFlag(NightCVCIType.NIGHT_FIGHTER))
                    {
                        num = 1;
                    }
                    else if (type.HasFlag(NightCVCIType.NIGHT_FIGHTER2))
                    {
                        num = 2;
                    }
                    else
                    {
                        num = 3;
                    }
                    var constraint = solver.MakeConstraint(num, double.PositiveInfinity);

                    foreach (var info in GetInfoListFromVariables(variables).Where(x => x.ship.ID == shipSlotInfo.ShipInfo.ID &&
                                                                                    x.airCraft.Type.Equals(Consts.Fighter) &&
                                                                                    x.airCraft.NightType))
                    {
                        constraint.SetCoefficient(info.variable, 1);
                    }
                }
                
                // 夜攻
                if (type.HasFlag(NightCVCIType.NIGHT_BOMBER))
                {
                    var constraint = solver.MakeConstraint(1, double.PositiveInfinity);

                    foreach (var info in GetInfoListFromVariables(variables).Where(x => x.ship.ID == shipSlotInfo.ShipInfo.ID &&
                                                                                    x.airCraft.Attackable &&
                                                                                    x.airCraft.NightType))
                    {
                        constraint.SetCoefficient(info.variable, 1);
                    }
                }
                
                // 夜襲カットイン対応艦載機
                if (type.HasFlag(NightCVCIType.BOMBER) || type.HasFlag(NightCVCIType.BOMBER2))
                {
                    var constraint = solver.MakeConstraint(type.HasFlag(NightCVCIType.BOMBER) ? 1 : 2, double.PositiveInfinity);

                    foreach (var info in GetInfoListFromVariables(variables).Where(x => x.ship.ID == shipSlotInfo.ShipInfo.ID &&
                                                                                    x.airCraft.Attackable &&
                                                                                    x.airCraft.NightCutin))
                    {
                        constraint.SetCoefficient(info.variable, 1);
                    }
                }

                // 夜間作戦航空要員
                if(!shipSlotInfo.ShipInfo.NightCutin)
                {
                    var constraint = solver.MakeConstraint(1, double.PositiveInfinity);

                    foreach (var info in GetInfoListFromVariables(variables).Where(x => x.ship.ID == shipSlotInfo.ShipInfo.ID &&
                                                                                    x.airCraft.Type.Equals(Consts.AviationPersonnel) &&
                                                                                    x.airCraft.NightCutin))
                    {
                        constraint.SetCoefficient(info.variable, 1);
                    }
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
                                                                Func<ShipSlotInfo, Func<(Variable variable, Ship ship, AirCraft airCraft, int improvement, int slotIndex), bool>> predicate,
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
        private static Func<(Variable variable, Ship ship, AirCraft airCraft, int improvement, int slotIndex), bool> FirstAttackFilter(ShipSlotInfo ship) =>
                                v => v.ship.ID == ship.ShipInfo.ID &&
                                v.airCraft.Attackable &&
                                v.slotIndex == 1;

        /// <summary>
        /// 制約条件(艦載機設定)フィルタ(攻撃機を最小スロットに積まない)
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static Func<(Variable variable, Ship ship, AirCraft airCraft, int improvement, int slotIndex), bool> MinimumSlotFilter(ShipSlotInfo ship) =>
                                v => v.ship.ID == ship.ShipInfo.ID &&
                                v.airCraft.Attackable &&
                                v.slotIndex == ship.MinSlotIndex;

        /// <summary>
        /// 制約条件(艦載機設定)フィルタ(熟練艦載機整備員を最小スロットに積む)
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static Func<(Variable variable, Ship ship, AirCraft airCraft, int improvement, int slotIndex), bool> MaintenancePersonnelFilter(ShipSlotInfo ship) =>
                                        v => v.ship.ID == ship.ShipInfo.ID &&
                                        v.airCraft.Name.Contains("熟練艦載機整備員") &&
                                        v.slotIndex == ship.MinSlotIndex;

        /// <summary>
        /// 制約条件(艦載機設定)フィルタ(彩雲を最小スロットに積む)
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static Func<(Variable variable, Ship ship, AirCraft airCraft, int improvement, int slotIndex), bool> SaiunFilter(ShipSlotInfo ship) => 
                                        v => v.ship.ID == ship.ShipInfo.ID &&
                                        v.airCraft.Name.Contains("彩雲") &&
                                        v.slotIndex == ship.MinSlotIndex;

        /// <summary>
        /// 制約条件(艦載機設定)フィルタ(攻撃機を必ず積む)
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static Func<(Variable variable, Ship ship, AirCraft airCraft, int improvement, int slotIndex), bool> AttackFilter(ShipSlotInfo ship) => 
                                        v => v.ship.ID == ship.ShipInfo.ID &&
                                        v.airCraft.Attackable;

        /// <summary>
        /// 制約条件(艦載機設定)フィルタ(攻撃機のみ積む)
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static Func<(Variable variable, Ship ship, AirCraft airCraft, int improvement, int slotIndex), bool> OnlyAttackerFilter(ShipSlotInfo ship) =>
                        v => v.ship.ID == ship.ShipInfo.ID &&
                        !v.airCraft.Attackable;

        /// <summary>
        /// 変数リスト取得
        /// </summary>
        /// <param name="variables"></param>
        /// <returns></returns>
        private static IEnumerable<(Variable variable, Ship ship, AirCraft airCraft, int improvement, int slotIndex)> GetInfoListFromVariables(IEnumerable<Variable> variables) => variables.Select(variable =>
        {
            var info = Parse(variable.Name());
            return (variable, ShipRecords.Instance.Records.FirstOrDefault(x => x.ID == info.shipId), AirCraftRecords.Instance.Records.FirstOrDefault(x => x.Id == info.airCraftId), info.improvement, info.slotIndex);
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
                var airCraft = GetAirCraft(answer.airCraft.Id, answer.improvement);

                var shipSlotInfo = shipSlotInfos.First(x => x.ShipInfo.ID == answer.ship.ID);

                shipSlotInfo.GetType().GetProperty($"Slot{answer.slotIndex}").SetValue(shipSlotInfo, airCraft);
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
                    .Where(x => x.airCraft.Id == setting.AirCraft.Id && x.improvement == setting.Improvement))
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
                var airCraft = GetAirCraft(info.airCraft.Id, info.improvement);
                var slotNum = GetSlotNum(info.ship.ID, info.slotIndex);

                objective.SetCoefficient(info.variable, airCraft.AirCraft.Accuracy + airCraft.AirCraft.Evasion + airCraft.AirCraft.Power(slotNum));
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
            foreach (var slotGroups in GetInfoListFromVariables(variables).GroupBy(x => (x.ship.ID, x.slotIndex)))
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
                var airCraft = GetAirCraft(info.airCraft.Id, info.improvement);
                var slotNum = GetSlotNum(info.ship.ID, info.slotIndex);

                constraint.SetCoefficient(info.variable, airCraft.AirCraft.AirSuperiorityPotential(slotNum));
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
            var constraint = solver.MakeConstraint(double.NegativeInfinity, 0);

            foreach (var info in GetInfoListFromVariables(variables).Select(x => (x.variable,
                                                                                    shipSlotInfos.First(y => y.ShipInfo.ID == x.ship.ID),
                                                                                    x.airCraft))
                                                                    .Where(z => !Equippable(z.Item3.Type, z.Item2)))
            {
                constraint.SetCoefficient(info.variable, 1);
            }
        }

        /// <summary>
        /// 装備可能か
        /// </summary>
        /// <param name="airCraftType"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool Equippable(string airCraftType, ShipSlotInfo info)
        {
            var dic = new Dictionary<string, Func<ShipSlotInfo, bool>>()
            {
                { Consts.TorpedoBomber , x => Regex.IsMatch(x.ShipInfo.Type,"(正規|装甲|軽)空母") || x.ShipInfo.Name.Contains("速吸") },
                { Consts.Fighter , x => Regex.IsMatch(x.ShipInfo.Type,"(正規|装甲|軽)空母") || x.ShipInfo.Type.Equals("揚陸艦") },
                { Consts.ReconAircraft , x => Regex.IsMatch(x.ShipInfo.Type,"(正規|装甲|軽)空母") },
                { Consts.DiveBomber , x => Regex.IsMatch(x.ShipInfo.Type,"(正規|装甲|軽)空母") },
                { Consts.AviationPersonnel , x => Regex.IsMatch(x.ShipInfo.Type,"(正規|装甲|軽)空母") || new []{"航空戦艦", "航空巡洋艦"}.Contains(x.ShipInfo.Type) || new[]{ "由良改二", "Zara due", "速吸", "Commandant Teste", "秋津洲改" }.Any(y => x.ShipInfo.Name.Contains(y)) },
                { Consts.ReconnaissanceSeaplane , x => x.ShipInfo.Type.Contains("戦艦") || x.ShipInfo.Type.Contains("巡洋艦")},
                { Consts.SeaplaneFighter , x => new []{"水上機母艦", "航空戦艦", "航空巡洋艦", "重巡洋艦", "戦艦", "巡洋戦艦", "潜水空母", "補給艦" }.Contains(x.ShipInfo.Type) || new [] { "由良改二", "Zara due", "Italia", "Roma改", "長門", "陸奥", "大和", "武蔵"}.Any(y => x.ShipInfo.Name.Contains(y)) },
                { Consts.SeaplaneBomber , x => new []{"水上機母艦", "航空戦艦", "航空巡洋艦", "重巡洋艦", "巡洋戦艦", "潜水空母", "補給艦"}.Contains(x.ShipInfo.Type) || new [] { "由良改二", "Zara due", "Italia", "Roma改"}.Any(y => x.ShipInfo.Name.Contains(y)) },
                { Consts.JetBomber , x => Regex.IsMatch(x.ShipInfo.Name, "(翔鶴|瑞鶴)改二甲") },
            };

            return dic[airCraftType](info);
        }

        /// <summary>
        /// 艦載機取得
        /// </summary>
        /// <param name="airCraftId"></param>
        /// <param name="improvement"></param>
        /// <returns></returns>
        private static AirCraftInfo GetAirCraft(int airCraftId, int improvement) => new AirCraftInfo(airCraftId, improvement);

        /// <summary>
        /// スロット数取得
        /// </summary>
        /// <param name="shipId"></param>
        /// <param name="slotIndex"></param>
        /// <returns></returns>
        private static int GetSlotNum(int shipId, int slotIndex)
        {
            var ship = ShipRecords.Instance.Records.ToList().Find(x => x.ID == shipId);

            return (int)ship.GetType().GetProperty($"Slot{slotIndex}Num").GetValue(ship);
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
