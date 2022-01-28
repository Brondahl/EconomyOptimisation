using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace EconomyOptimisation.Test
{
    public class Tests
    {
        private Scenario testScenario;
        private List<StrategyBuilder> builders;
        [SetUp]
        public void Setup()
        {
            Upgrade.ResetIds();
            builders = new List<StrategyBuilder>
            {
                new AsListedStrategyBuilder(),
                new CheapGreedyStrategyBuilder(),
                new AdditiveRoiGreedyStrategyBuilder(),
                new InitialPointOfBuyBackStrategyBuilder(),
                new RecursivePointOfBuyBackStrategyBuilder(),
                new BruteForceBuilder()
            };
        }
        
        private Scenario BuildStandardScenario(params Upgrade[] upgrades)
        {
            return new Scenario(upgrades);
        }

        [Test]
        public void EvaluateAllStrategyBuildersForScenario1()
        {
            EvaluateAllStrategyBuilders(
                Upgrade.NewAdditive(17030, 10),
                Upgrade.NewAdditive(5030, 6),
                Upgrade.NewAdditive(16030, 4),
                Upgrade.NewAdditive(4030, 3),
                Upgrade.NewAdditive(1030, 1)
            );
        }
        
        [Test]
        public void EvaluateAllStrategyBuildersForScenario2()
        {
            EvaluateAllStrategyBuilders(
                Upgrade.NewAdditive(1,9),
                Upgrade.NewAdditive(2000, 10),
                Upgrade.NewAdditive(4000, 39)
            );
        }
        [Test]
        public void EvaluateAllStrategyBuildersForScenario3()
        {
            EvaluateAllStrategyBuilders(
                Upgrade.NewAdditive(999, 10),
                Upgrade.NewAdditive(1000, 20)
            );
        }

        public void EvaluateAllStrategyBuilders(params Upgrade[] upgrades)
        {
            testScenario = BuildStandardScenario(upgrades);
            foreach (var strategyBuilder in builders)
            {
                var strategyForTestScenario = strategyBuilder.CalculateStrategy(testScenario);
                var score = new StrategyEvaluator().Evaluate(strategyForTestScenario);
                Console.WriteLine($"{strategyBuilder.GetType()}: {score}. [{strategyForTestScenario.UpgradeOrderingText}]");
            }
        }
    }
}