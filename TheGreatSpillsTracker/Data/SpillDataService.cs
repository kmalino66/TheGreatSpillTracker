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
                spill.EnterpriseSpill = DateTime.Now;
                spill.EnterpriseSpillCount = 0;
                spill.HomeSpill = DateTime.Now;
                spill.HomeSpillCount = 0;
                spill.BigSpill = DateTime.Now;
                spill.BigSpillCount = 0;
                spill.SpillCount = 0;
                spill.PassHash = "";
                spill.HomeItemLastSpilled = "";
                spill.WorkItemLastSpilled = "";
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

        public void AddHomeSpill()
        {
            spill.CheckSetNewRecord(DateTime.Now);
            spill.HomeSpill = DateTime.Now;
            spill.HomeSpillCount++;
            spill.SpillCount++;
            spill.HomeBigSpill = false;
            SaveInfo();
        }

        public void AddEnterpriseSpill()
        {
            spill.CheckSetNewRecord(DateTime.Now);
            spill.EnterpriseSpill = DateTime.Now;
            spill.EnterpriseSpillCount++;
            spill.SpillCount++;
            spill.WorkBigSpill = false;
            SaveInfo();
        }

        public void AddRecordSpill(string basisTime)
        {
            switch (basisTime)
            {
                case "home":
                    spill.BigSpill = spill.HomeSpill;
                    spill.HomeBigSpill = true;
                    spill.WorkBigSpill = false;
                    break;
                case "work":
                    spill.BigSpill = spill.EnterpriseSpill;
                    spill.WorkBigSpill = true;
                    spill.HomeBigSpill = false;
                    break;
                default:
                    spill.BigSpill = DateTime.Now;
                    spill.WorkBigSpill = false;
                    spill.HomeBigSpill = false;
                    break;
            }

            spill.BigSpillCount++;

            SaveInfo();
        }

        public void ResetHomeCount()
        {
            spill.HomeSpillCount = 0;
            SaveInfo();
        }

        public void ResetEnterpriseCount()
        {
            spill.EnterpriseSpillCount = 0;
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
