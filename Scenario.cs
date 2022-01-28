using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EconomyOptimisation
{
    public class Scenario
    {
        public readonly HashSet<Upgrade> UpgradesAvailable;
        public int InitialCash = 0;
        public int InitialIncome = 1;

        public Scenario(IEnumerable<Upgrade> upgrades)
        {
            UpgradesAvailable = upgrades.ToHashSet();
        }

        public Scenario(IEnumerable<Upgrade> upgrades, int cash, int income) : this(upgrades)
        {
            InitialCash = cash;
            InitialIncome = income;
        }

        public State CreateInitialState() => new State(0, InitialCash, InitialIncome);
    }
}