using System;

namespace EconomyOptimisation
{
    public enum UpgradeType
    {
        Additive = 0,
        Multiplicative = 1
    }
    public class Upgrade
    {
        public static void ResetIds() => nextId = 0;
        private static int nextId = 0;
        private Upgrade(int cost, int reward, UpgradeType type)
        {
            Id = nextId++;
            Cost = cost;
            Reward = reward;
            Type = type;
        }

        public int Id { get; }
        public int Cost { get; }
        public int Reward { get; }
        public UpgradeType Type { get; }

        public static Upgrade NewAdditive(int cost, int reward) => new Upgrade(cost, reward, UpgradeType.Additive);
        public static Upgrade NewMultiplicative(int cost, int reward) => new Upgrade(cost, reward, UpgradeType.Multiplicative);
    }
}