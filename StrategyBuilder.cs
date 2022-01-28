using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq.Extensions;

namespace EconomyOptimisation
{
    public interface StrategyBuilder
    {
        public Strategy CalculateStrategy(Scenario scenario);
    }
    
    public class AsListedStrategyBuilder : StrategyBuilder
    {
        public Strategy CalculateStrategy(Scenario scenario)
        {
            return new Strategy
            {
                Scenario = scenario,
                UpgradeOrder = scenario.UpgradesAvailable.ToList()
            };
        }
    }
    
    public class CheapGreedyStrategyBuilder : StrategyBuilder
    {
        public Strategy CalculateStrategy(Scenario scenario)
        {
            return new Strategy
            {
                Scenario = scenario,
                UpgradeOrder = scenario.UpgradesAvailable.OrderBy(upgrade => upgrade.Cost).ToList()
            };
        }
    }
    
    public class AdditiveRoiGreedyStrategyBuilder : StrategyBuilder
    {
        public Strategy CalculateStrategy(Scenario scenario)
        {
            if (scenario.UpgradesAvailable.Any(upgrade => upgrade.Type != UpgradeType.Additive))
            {
                throw new NotSupportedException("Only handles Additive upgrades.");
            }

            var upgradesWithRoi =
                scenario.UpgradesAvailable
                    .Select(upgrade => new {upgrade, Roi = upgrade.Cost / (decimal) upgrade.Reward});
            
            return new Strategy
            {
                Scenario = scenario,
                UpgradeOrder = upgradesWithRoi.OrderBy(pair => pair.Roi).Select(pair => pair.upgrade).ToList()
            };
        }
    }
    public class InitialPointOfBuyBackStrategyBuilder : StrategyBuilder
    {
        public Strategy CalculateStrategy(Scenario scenario)
        {
            if (scenario.UpgradesAvailable.Any(upgrade => upgrade.Type != UpgradeType.Additive))
            {
                throw new NotSupportedException("Only handles Additive upgrades.");
            }

            var upgradesWithPob =
                scenario.UpgradesAvailable
                    .Select(upgrade => new {upgrade, Pob = (upgrade.Cost / (decimal)scenario.InitialIncome) + upgrade.Cost / (decimal) upgrade.Reward});
            
            return new Strategy
            {
                Scenario = scenario,
                UpgradeOrder = upgradesWithPob.OrderBy(pair => pair.Pob).Select(pair => pair.upgrade).ToList()
            };
        }
    }

    public class BruteForceBuilder : StrategyBuilder
    {
        public Strategy CalculateStrategy(Scenario scenario)
        {
            var allPossibleStrategies =
                scenario.UpgradesAvailable
                    .Permutations()
                    .Select(ordering => new Strategy
                    {
                        Scenario = scenario,
                        UpgradeOrder = ordering.ToList()
                    });
            
            var evaluator = new StrategyEvaluator();
            var strategiesWithScores = allPossibleStrategies
                .Select(strategy => new {strategy, score = evaluator.Evaluate(strategy)})
                .ToList();

            var optimalStrategiesWithScores = strategiesWithScores.MinBy(pair => pair.score).ToList();
            if (optimalStrategiesWithScores.Count > 1)
            {
                foreach (var optimalStrategy in optimalStrategiesWithScores)
                {
                    evaluator.Evaluate(optimalStrategy.strategy);
                }
            }
            return optimalStrategiesWithScores.First().strategy;
        }
    }

    public class RecursivePointOfBuyBackStrategyBuilder : StrategyBuilder
    {
        public Strategy CalculateStrategy(Scenario scenario)
        {
            var proposedStrategy = new Queue<Upgrade>();
            var remainingUpgrades = scenario.UpgradesAvailable.ToList();
            var currentIncome = scenario.InitialIncome;
            
            while (remainingUpgrades.Any())
            {
                var newScenario = new Scenario(remainingUpgrades, scenario.InitialCash, currentIncome);
                var nextUpgrade = new InitialPointOfBuyBackStrategyBuilder().CalculateStrategy(newScenario).UpgradeOrder.First();
                proposedStrategy.Enqueue(nextUpgrade);
                remainingUpgrades.Remove(nextUpgrade);
                currentIncome += nextUpgrade.Reward;
            }

            return new Strategy
            {
                Scenario = scenario,
                UpgradeOrder = proposedStrategy.ToList()
            };
        }
    }
}