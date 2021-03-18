using System;
using System.IO;
using Tomlyn;
using Tomlyn.Model;
using Tomlyn.Syntax;

namespace TheGreatSpillsTracker.Data
{
    public class SpillConfiguration
    {
        public string General_Heading { get; set; }
        public string General_SubHeading { get; set; }
        public bool General_ShowAdmin { get; set; }
        public bool General_ShowChangeLog { get; set; }
        public bool General_ShowGitHub { get; set; }

        public SpillConfiguration()
        {
            string path = Path.Combine(Environment.CurrentDirectory, Path.Combine("Config", "Config.toml"));

            if(!System.IO.File.Exists(path))
            {
                LoadDefaultConfiguration();
            }
            else
            {
                LoadFromToml(path);
            }

        }

        private void LoadDefaultConfiguration()
        {
            General_Heading = "The Great Spills Tracker";
            General_SubHeading = "A simple app to keep track of time since Trent last spilled.";
            General_ShowAdmin = true;
            General_ShowChangeLog = true;
            General_ShowGitHub = true;
        }

        private void LoadFromToml(string path)
        {
            LoadDefaultConfiguration();

            string doc = System.IO.File.ReadAllText(path);
            DocumentSyntax docSyn = Toml.Parse(doc);
            TomlTable table = docSyn.ToModel();

            foreach (var prop in this.GetType().GetProperties())
            {
                string _table = prop.Name.Split("_")[0];
                string _prop = prop.Name.Split("_")[1];

                try
                {
                    if (prop.GetType() == typeof(bool))
                    {
                        bool val = (bool)((TomlTable)table[_table])[_prop];
                        prop.SetValue(this, val);

                    }
                    else
                    {
                        string val = (string)((TomlTable)table[_table])[_prop];
                        prop.SetValue(this, val);
                    }

                }
                catch (Exception e)
                {
                    // Do nothing.
                }
            }

        }

        private T CastObject<T>(object input)
        {
            return (T)input;
        }
    }

  
}
