using System;
using System.Linq;

namespace EconomyOptimisation
{
    public class StrategyEvaluator
    {
        public int Evaluate(Strategy strategy)
        {
            EnsureUpgradeSetMatch(strategy);
            var initialState = strategy.Scenario.CreateInitialState();

            foreach (var upgrade in strategy.UpgradeOrder)
            {
                initialState.EvolveToCash(upgrade.Cost);
                initialState.ApplyUpgrade(upgrade);
            }

            return initialState.Tick;
        }

        private void EnsureUpgradeSetMatch(Strategy strategy)
        {
            if (!strategy.UpgradeOrder.ToHashSet().SetEquals(strategy.Scenario.UpgradesAvailable))
            {
                throw new InvalidOperationException("Strategy does not use same Upgrades as Scenario.");
            }
        }
    }
}