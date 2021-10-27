namespace AtomicTorch.SteamToEpicAchievementsConverter
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal static class SteamToEpicLocaleConverter
    {
        /// <summary>
        /// This is a collection of locales that were not found when requesting locale mapping.
        /// </summary>
        public static readonly HashSet<string> UnknownLocales = new HashSet<string>();

        private static readonly IReadOnlyDictionary<string, string> LocaleMapping;

        static SteamToEpicLocaleConverter()
        {
            Console.WriteLine("Reading LocaleMapping.ini file...");
            var localeMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            using (var reader = File.OpenText("LocaleMapping.ini"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Trim();
                    if (line.Length == 0)
                    {
                        continue;
                    }

                    var split = line.Split('=');
                    var key = split[0].Trim();
                    var value = split[1].Trim();
                    localeMapping.Add(key, value);
                }
            }

            LocaleMapping = localeMapping;
            Console.WriteLine($"Finished reading LocaleMapping.ini file. Total {LocaleMapping.Count} entries found.");
        }

        public static bool TryGetEpicLanguageCode(string steamLanguageCode, out string languageCode)
        {
            if (LocaleMapping.TryGetValue(steamLanguageCode, out languageCode))
            {
                return true;
            }

            UnknownLocales.Add(steamLanguageCode);
            return false;
        }
    }
}