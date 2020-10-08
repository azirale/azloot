using azloot.core;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Text.Json;

namespace winadmin
{
    static class CoreInteractions
    {
        /// <summary>
        /// The active/open configuration. Defaults to 
        /// </summary>
        private static Configuration config = new Configuration();
        private static bool isModifiedAfterSave = false;

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
            isModifiedAfterSave = true;
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
                var datapack =  JsonSerializer.Deserialize<ConfigurationDatapack>(jsonText);
                config = new Configuration(datapack);
                config = null;
            }
            isModifiedAfterSave = false;
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
            isModifiedAfterSave = false;
        }

        public static bool HasUnsavedData()
        {
            return isModifiedAfterSave;
        }
    }
}
