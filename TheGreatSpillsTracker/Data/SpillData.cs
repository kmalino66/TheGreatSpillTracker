using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheGreatSpillsTracker.Data
{
    [Serializable]
    public class SpillData
    {
        public DateTime EnterpriseSpill { get; set; }
        public DateTime HomeSpill { get; set; }

        public int SpillCount { get; set; }
        public int EnterpriseSpillCount { get; set; }
        public int HomeSpillCount { get; set; }
        public string PassHash { get; set; }

        public string EnterpriseSpillString()
        {
            return EnterpriseSpill.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
        }

        public string EnterpriseSpillStringNonUTC()
        {
            return EnterpriseSpill.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public String HomeSpillString()
        {
            return HomeSpill.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
        }

        public String HomeSpillStringNonUTC()
        {
            return HomeSpill.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
