using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TheGreatSpillsTracker.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SpillData
    {
        private const string STRING_FORMAT = "yyyy-MM-dd HH:mm:ss";

        [JsonProperty]
        public List<Spill> spills { get; set; }

        [JsonProperty]
        public string PassHash { get; set; }

        public Spill WorkSpill { get; set; }
        public Spill HomeSpill { get; set; }
        public Spill BigSpill { get; set; }
        public TimeSpan MaxTimeNoSpill { get; set; }
        public TimeSpan MinTimeNoSpill { get; set; }

        public int SpillCount { get; set; }
        public int WorkSpillCount { get; set; }
        public int HomeSpillCount { get; set; }
        public int BigSpillCount { get; set; }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            spills.Sort();
            WorkSpill = new Spill();
            HomeSpill = new Spill();
            BigSpill = new Spill();
            MaxTimeNoSpill = TimeSpan.MinValue;
            MinTimeNoSpill = TimeSpan.MaxValue;

            foreach (Spill spill in spills)
            {
                TimeSpan span = TimeSinceLastSpill(spill.Time);

                if(span.Ticks > 0)
                {
                    if(span > MaxTimeNoSpill)
                    {
                        MaxTimeNoSpill = span;
                    }

                    if(span < MinTimeNoSpill)
                    {
                        MinTimeNoSpill = span;
                    }
                }

                if (spill.Type == SpillType.Home)
                {
                    HomeSpill = spill;
                    HomeSpillCount += 1;
                }
                else if (spill.Type == SpillType.Work)
                {
                    WorkSpill = spill;
                    WorkSpillCount += 1;
                }

                if (spill.BigSpill)
                {
                    BigSpill = spill;
                }
            }
        }

        public string WorkSpillString()
        {
            return WorkSpill.Time.ToUniversalTime().ToString(STRING_FORMAT);
        }

        public string WorkSpillStringNonUTC()
        {
            return WorkSpill.Time.ToString(STRING_FORMAT);
        }

        public string HomeSpillString()
        {
            return HomeSpill.Time.ToUniversalTime().ToString(STRING_FORMAT);
        }

        public string HomeSpillStringNonUTC()
        {
            return HomeSpill.Time.ToString(STRING_FORMAT);
        }

        public string BigSpillString()
        {
            return BigSpill.Time.ToUniversalTime().ToString(STRING_FORMAT);
        }

        public string BigSpillStringNonUTC()
        {
            return BigSpill.Time.ToString(STRING_FORMAT);
        }

        public TimeSpan TimeSinceLastSpill(DateTime spillTime)
        {
            Spill lastSpill;

            if (WorkSpill.Time.CompareTo(DateTime.MinValue) == 0 && HomeSpill.Time.CompareTo(DateTime.MinValue) == 0)
            {
                return new TimeSpan();
            }
            else if (WorkSpill.Time.CompareTo(DateTime.MinValue) == 0)
            {
                lastSpill = HomeSpill;
            }
            else if (HomeSpill.Time.CompareTo(DateTime.MinValue) == 0)
            {
                lastSpill = WorkSpill;
            }
            else if (WorkSpill.Time >= HomeSpill.Time)
            {
                lastSpill = WorkSpill;
            }
            else
            {
                lastSpill = HomeSpill;
            }

            return spillTime - lastSpill.Time;
        }

        public void CheckSetNewRecord(DateTime spillTime)
        {
            TimeSpan timeSinceLastSpill = TimeSinceLastSpill(spillTime);

            if (timeSinceLastSpill > MaxTimeNoSpill)
            {
                MaxTimeNoSpill = timeSinceLastSpill;
            }
        }

        public void CheckSetNewMinRecord(DateTime spillTime)
        {
            TimeSpan timeSinceLastSpill = TimeSinceLastSpill(spillTime);

            if (timeSinceLastSpill < MinTimeNoSpill)
            {
                MinTimeNoSpill = timeSinceLastSpill;
            }
        }

        public void AddNewSpill(SpillType type, DateTime spillTime)
        {
            AddNewSpill(type, spillTime, "");
        }

        public void AddNewSpill(SpillType type, DateTime spillTime, string spillDescription)
        {

            CheckSetNewRecord(spillTime);
            CheckSetNewMinRecord(spillTime);
            SpillCount++;

            Spill spill = new Spill(spillTime, spillDescription, false, type);
            spills.Add(spill);

            switch (type)
            {
                case SpillType.Home:
                    HomeSpill = spill;
                    HomeSpillCount++;
                    break;
                case SpillType.Work:
                    WorkSpill = spill;
                    WorkSpillCount++;
                    break;
                default:
                    throw new ArgumentException("Bad spill type");
            }
        }

        public void MarkAsBigSpill(SpillType type)
        {
            BigSpillCount++;

            switch (type)
            {
                case SpillType.Home:
                    HomeSpill.BigSpill = true;
                    BigSpill = HomeSpill;
                    break;
                case SpillType.Work:
                    WorkSpill.BigSpill = true;
                    BigSpill = WorkSpill;
                    break;
                default:
                    BigSpill = new Spill();
                    break;

            }
        }

        public void ResetSpillCount(SpillType type)
        {
            switch (type)
            {
                case SpillType.Home:
                    HomeSpillCount = 0;
                    break;
                case SpillType.Work:
                    WorkSpillCount = 0;
                    break;
                default:
                    throw new ArgumentException("Bad spill type");
            }
        }

    }

    public enum SpillType
    {
        Home,
        Work,
        None
    }
}
