using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace TheGreatSpillsTracker.Data
{
    public class Spill : IComparable<Spill>
    {
        private const string STRING_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public DateTime Time { get; set; }
        public string Description { get; set; }
        public bool BigSpill { get; set; }
        public SpillType Type { get; set; }

        public int CompareTo(Spill other)
        {
            return Time.CompareTo(other.Time);
        }

        public Spill()
        {
            Time = DateTime.MinValue;
            Description = "";
            BigSpill = false;
            Type = SpillType.None;
        }

        public Spill(SpillType type) : this()
        {
            Type = type;
        }

        public Spill(DateTime time, string desc, bool bigSpill, SpillType type)
        {
            Time = time;
            Description = desc;
            BigSpill = bigSpill;
            Type = type;
        }
    }

}
