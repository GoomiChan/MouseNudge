using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;

namespace MouseNudge
{
    public class Settings
    {
        private static Settings Self = new Settings();
        private static string SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"MouseNudge", "Settings.json");

        public int UpdateRate = 200;
        public int NudgeMargin = 1;

        public static Settings Get()
        {
            return Self;
        }

        public void Load()
        {
            if (!File.Exists(SettingsPath))
            {
                // Save the defaults to file
                Save();
            }

            try
            {
                using (FileStream fs = File.Open(SettingsPath, FileMode.Open))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Settings));
                    Self = (Settings)serializer.ReadObject(fs);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Error loading settings.json");
            }
        }

        public void Save()
        {
            if (!File.Exists(SettingsPath))
            {
                // Make the file path
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));
            }

            try
            {
                using (FileStream fs = File.Open(SettingsPath, FileMode.Create))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Settings));
                    serializer.WriteObject(fs, this);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Error saving settings.json");
            }
        }

    }
}
