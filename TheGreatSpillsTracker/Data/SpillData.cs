using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheGreatSpillsTracker.Data
{
    [Serializable]
    public class SpillData
    {
        private const string STRING_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public DateTime WorkSpill { get; set; }
        public DateTime HomeSpill { get; set; }

        public DateTime BigSpill { get; set; }
        public TimeSpan MaxTimeNoSpill { get; set; }
        public TimeSpan MinTimeNoSpill { get; set; }

        public int SpillCount { get; set; }
        public int WorkSpillCount { get; set; }
        public int HomeSpillCount { get; set; }
        public int BigSpillCount { get; set; }
        public string PassHash { get; set; }
        public string HomeSpillDescription { get; set; }
        public string WorkSpillDescription { get; set; }
        public string BigSpillDescription { get; set; }
        public string RecordSpillItem { get; set; }
        public bool HomeBigSpill { get; set; }
        public bool WorkBigSpill { get; set; }

        public string WorkSpillString()
        {
            return WorkSpill.ToUniversalTime().ToString(STRING_FORMAT);
        }

        public string WorkSpillStringNonUTC()
        {
            return WorkSpill.ToString(STRING_FORMAT);
        }

        public string HomeSpillString()
        {
            return HomeSpill.ToUniversalTime().ToString(STRING_FORMAT);
        }

        public string HomeSpillStringNonUTC()
        {
            return HomeSpill.ToString(STRING_FORMAT);
        }

        public string BigSpillString()
        {
            return BigSpill.ToUniversalTime().ToString(STRING_FORMAT);
        }

        public string BigSpillStringNonUTC()
        {
            return BigSpill.ToString(STRING_FORMAT);
        }

        public TimeSpan TimeSinceLastSpill(DateTime spillTime)
        {
            DateTime lastSpill;
            if (WorkSpill >= HomeSpill)
            {
                lastSpill = WorkSpill;
            }
            else
            {
                lastSpill = HomeSpill;
            }

            return spillTime - lastSpill;
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

        public void AddNewSpill(SpillType type, DateTime spillTime, String spillDescription)
        {

            CheckSetNewRecord(spillTime);
            CheckSetNewMinRecord(spillTime);
            SpillCount++;

            switch (type)
            {
                case SpillType.Home:
                    HomeSpill = spillTime;
                    HomeSpillCount++;
                    HomeBigSpill = false;
                    HomeSpillDescription = spillDescription;
                    break;
                case SpillType.Work:
                    WorkSpill = spillTime;
                    WorkSpillCount++;
                    WorkBigSpill = false;
                    WorkSpillDescription = spillDescription;
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
                    BigSpill = HomeSpill;
                    BigSpillDescription = HomeSpillDescription;
                    HomeBigSpill = true;
                    WorkBigSpill = false;
                    break;
                case SpillType.Work:
                    BigSpill = WorkSpill;
                    BigSpillDescription = WorkSpillDescription;
                    HomeBigSpill = false;
                    WorkBigSpill = true;
                    break;
                default:
                    BigSpill = DateTime.Now;
                    BigSpillDescription = "";
                    HomeBigSpill = false;
                    WorkBigSpill = false;
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
