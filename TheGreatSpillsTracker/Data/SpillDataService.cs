using Newtonsoft.Json;
using System;
using System.IO;

namespace TheGreatSpillsTracker.Data
{
    public class SpillDataService
    {
        private SpillData spill = new SpillData();

        public void Initialize()
        {
            SpillData tmp = ReadInfo();

            if (tmp != null)
            {
                spill = tmp;
            }
            else
            {
                spill.WorkSpill = DateTime.Now;
                spill.WorkSpillCount = 0;
                spill.HomeSpill = DateTime.Now;
                spill.HomeSpillCount = 0;
                spill.BigSpill = DateTime.Now;
                spill.BigSpillCount = 0;
                spill.SpillCount = 0;
                spill.PassHash = "";
                spill.HomeSpillDescription = "";
                spill.WorkSpillDescription = "";
                spill.RecordSpillItem = "";
                spill.HomeBigSpill = false;
                spill.WorkBigSpill = false;
                SaveInfo();
            }
        }

        public SpillData GetSpill()
        {
            return spill;
        }
        
        [Obsolete("AddNewSpill should be used instead")]
        public void AddHomeSpill(DateTime spillTime)
        {
            spill.AddNewSpill(SpillType.Home, spillTime);
            SaveInfo();
        }

        [Obsolete("AddNewSpill should be used instead")]
        public void AddEnterpriseSpill(DateTime spillTime)
        {
            spill.AddNewSpill(SpillType.Work, spillTime);
            SaveInfo();
        }

        public void AddNewSpill(SpillType type, DateTime spillTime, string description)
        {
            spill.AddNewSpill(type, spillTime, description);
            SaveInfo();
        }

        public void AddNewSpill(SpillType type, DateTime spillTime)
        {
            AddNewSpill(type, spillTime, "");
        }

        public void AddRecordSpill(string basisTime)
        {
            switch (basisTime)
            {
                case "home":
                    spill.MarkAsBigSpill(SpillType.Home);
                    break;
                case "work":
                    spill.MarkAsBigSpill(SpillType.Work);
                    break;
                default:
                    spill.MarkAsBigSpill(SpillType.None);
                    break;
            }

            SaveInfo();
        }

        public void ResetHomeCount()
        {
            spill.ResetSpillCount(SpillType.Home);
            SaveInfo();
        }

        public void ResetEnterpriseCount()
        {
            spill.ResetSpillCount(SpillType.Work);
            SaveInfo();
        }

        public void SaveInfo()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "SpillData.json");
            string json = JsonConvert.SerializeObject(spill, Formatting.Indented);
            using (StreamWriter outputFile = new StreamWriter(path))
            {
                outputFile.WriteLine(json);
            }
        }

        public SpillData ReadInfo()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "SpillData.json");

            if (!System.IO.File.Exists(path))
            {
                return null;
            }

            string json = System.IO.File.ReadAllText(path);
            return JsonConvert.DeserializeObject<SpillData>(json);
        }

        public string ReadInfoRaw()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "SpillData.json");

            if (!System.IO.File.Exists(path))
            {
                return null;
            }

            return System.IO.File.ReadAllText(path);
        }
    }
}
