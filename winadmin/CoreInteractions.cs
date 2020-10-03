using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using azloot.core;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace winadmin
{
    static class CoreInteractions
    {
        /// <summary>
        /// The active/open configuration. Defaults to 
        /// </summary>
        private static Configuration config = new Configuration();

        public static void CreateNewConfiguration()
        {
            config = new Configuration();
            // for now with dev/debugging throw in some random data to look at
            var rank = new Rank("somerank", 1);
            config.Ranks.Add(rank);
            var person = new Person("DangerousDave", rank, "Warrior", "Tenk");
            var plist = new PointsList("Da List", 1);
            plist.Add(person, 17f);
            config.Ranks.Add(rank);
            config.Persons.Add(person);
            config.PointsLists.Add(plist);
        }

        /// <summary>
        /// Opens a configuration from a datapack
        /// </summary>
        public static void OpenConfiguration(string filePath)
        {
            using (var fileRead = File.OpenRead(filePath))
            using (var streamRead = new StreamReader(fileRead))
            {
                var jsonText = streamRead.ReadToEnd();
                JsonSerializer.Deserialize(jsonText, typeof(ConfigurationDatapack));
            }
        }

        /// <summary>
        /// saves the active configuration to a file
        /// </summary>
        /// <param name="filePath"></param>
        public static void SaveConfiguration(string filePath)
        {
            var datapack = config.ToDatapack();
            var opts = new JsonSerializerOptions() { WriteIndented = true };
            var jsonText = JsonSerializer.Serialize(datapack, typeof(ConfigurationDatapack), opts);
            using (var fileWrite = File.OpenWrite(filePath))
            using (var streamWrite = new StreamWriter(fileWrite))
            {
                streamWrite.Write(jsonText);
            }
        }
    }
}
