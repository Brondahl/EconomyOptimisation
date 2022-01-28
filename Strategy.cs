using System.Collections.Generic;
using System.Linq;

namespace EconomyOptimisation
{
    public class Strategy
    {
        public Scenario Scenario; 
        public List<Upgrade> UpgradeOrder = new List<Upgrade>();
        public string UpgradeOrderingText => string.Join(",",UpgradeOrder.Select(upg => upg.Id.ToString()));
    }
}