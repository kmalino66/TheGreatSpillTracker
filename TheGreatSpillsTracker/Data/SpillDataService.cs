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
                spill.RecordSpill = DateTime.Now;
                spill.SpillCount = 0;
                spill.PassHash = "";
                SaveInfo();
            }
        }

        public SpillData GetSpill()
        {
            return spill;
        }

        public void AddHomeSpill()
        {
            spill.HomeSpill = DateTime.Now;
            spill.HomeSpillCount++;
            spill.SpillCount++;
            SaveInfo();
        }

        public void AddEnterpriseSpill()
        {
            spill.EnterpriseSpill = DateTime.Now;
            spill.EnterpriseSpillCount++;
            spill.SpillCount++;
            SaveInfo();
        }

        public void AddRecordSpill(string basisTime)
        {
            switch (basisTime)
            {
                case "home":
                    spill.RecordSpill = spill.HomeSpill;
                    break;
                case "work":
                    spill.RecordSpill = spill.EnterpriseSpill;
                    break;
                default:
                    spill.RecordSpill = DateTime.Now;
                    break;
            }

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
    }
}
