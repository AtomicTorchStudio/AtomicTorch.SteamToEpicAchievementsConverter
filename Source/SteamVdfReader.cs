namespace AtomicTorch.SteamToEpicAchievementsConverter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using VdfParser;

    internal static class SteamVdfReader
    {
        public static List<AchievementEntry> ReadAchievementEntries(string filePath)
        {
            var result = new List<AchievementEntry>();
            Console.WriteLine("Reading the input VDF file: " + filePath);

            using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                var deserializer = new VdfDeserializer();
                var deserialized = deserializer.Deserialize(fileStream) as IDictionary<string, object>;

                dynamic root = deserialized.Values.First();
                dynamic stats = root.stats;
                var statEntries = (stats as IDictionary<string, object>);
                foreach (dynamic statEntry in statEntries.Values)
                {
                    var steamAchievements = statEntry.bits as IDictionary<string, object>;
                    foreach (dynamic entry in steamAchievements.Values)
                    {
                        var id = entry.name;
                        var name = GetLocalizedEntries(entry.display.name);
                        var description = GetLocalizedEntries(entry.display.desc);
                        var isHidden = entry.display.hidden == "1";
                        result.Add(new AchievementEntry(id, name, description, isHidden));
                    }
                }
            }

            Console.WriteLine(
                $"Finished reading the input VDF file. Total {result.Count} achievement entries found.");
            return result;
        }

        private static Dictionary<string, string> GetLocalizedEntries(dynamic root)
        {
            var result = new Dictionary<string, string>();
            var dict = (IDictionary<string, object>)root;

            foreach (var pair in dict)
            {
                var language = pair.Key;
                if (language == "token")
                {
                    // this is an internal Steam ID for the achievement, skip it
                    continue;
                }

                var languageCode = language == "english"
                                       ? string.Empty // English is the base language and has no locale ID
                                       : SteamToEpicLocaleConverter.GetEpicLanguageCode(language);
                var text = (string)pair.Value;
                result.Add(languageCode, text);
            }

            return result;
        }
    }
}