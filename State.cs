using System;

namespace EconomyOptimisation
{
    public class State
    {
        public State(int tick, int cash, int income, int incomeMultiplier = 1)
        {
            Tick = tick;
            Cash = cash;
            BaseIncome = income;
            IncomeMultiplier = incomeMultiplier;
        }

        public void EvolveByXTicks(int ticks)
        {
            Tick += ticks;
            Cash += ticks * TotalIncome;
        }
        
        public void EvolveToTick(int finalTick)
        {
            EvolveByXTicks(finalTick - Tick);
        }
        
        public int EvolveToCash(int targetCash)
        {
            if (Cash >= targetCash)
            {
                return 0;
            }
            
            var initialTicks = Tick;
            var cashIncrease = targetCash - Cash;
            var lowerBoundTicks = cashIncrease / TotalIncome;
            
            EvolveByXTicks(lowerBoundTicks);
            if (Cash < targetCash)
            {
                EvolveByXTicks(1);
                if (Cash < targetCash)
                {
                    throw new InvalidOperationException("Logic error.");
                }
            }

            return Tick - initialTicks;
        }

        public void ApplyUpgrade(Upgrade upgrade)
        {
            if (upgrade.Cost > Cash)
            {
                throw new InvalidOperationException("Upgrade is too expensive!");
            }

            Cash -= upgrade.Cost;
            
            switch (upgrade.Type)
            {
                case UpgradeType.Additive:
                    BaseIncome += upgrade.Reward;
                    break;
                case UpgradeType.Multiplicative:
                    IncomeMultiplier *= upgrade.Reward;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public int Tick { get; private set; } = 0;
        public int Cash { get; private set; } = 0;
        public int BaseIncome { get; private set; }
        public int IncomeMultiplier { get; private set; }
        private int TotalIncome => BaseIncome * IncomeMultiplier;
    }
}