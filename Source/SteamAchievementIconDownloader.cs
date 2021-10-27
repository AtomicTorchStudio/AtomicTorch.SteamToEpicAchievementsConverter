namespace AtomicTorch.SteamToEpicAchievementsConverter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    public static class SteamAchievementIconDownloader
    {
        public static void DownloadSteamAchievementIcons(
            uint steamAppId,
            string outputPath,
            IReadOnlyList<AchievementEntry> achievementEntries)
        {
            if (string.IsNullOrEmpty(SettingsHelper.UrlDownloadSteamAchievementIcon))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(
                    "UrlDownloadSteamAchievementIcon is not configured in Settings.ini file. Achievement icons will be not downloaded.");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            DeleteImages(outputPath);

            foreach (var entry in achievementEntries)
            {
                DownloadIcon(filePath: Path.Combine(outputPath,
                                                    string.Format(SettingsHelper.LockedIconFileNameFormat,
                                                                  entry.Id)),
                             entry.SteamIconIdUnlocked);

                DownloadIcon(Path.Combine(outputPath,
                                          string.Format(SettingsHelper.UnlockedIconFileNameFormat,
                                                        entry.Id)),
                             entry.SteamIconIdLocked);
            }

            void DownloadIcon(string filePath, string iconId)
            {
                var url = string.Format(SettingsHelper.UrlDownloadSteamAchievementIcon, steamAppId, iconId);
                using (var client = new WebClient())
                {
                    client.DownloadFile(url, filePath);
                    Console.WriteLine("Downloaded achievement icon: " + filePath);
                }
            }
        }

        private static void DeleteImages(string outputPath)
        {
            var extensions = new HashSet<string> { ".png", ".jpg", ".jpeg" };
            var files = Directory.GetFiles(outputPath);

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file);
                if (extensions.Contains(extension))
                {
                    File.Delete(file);
                }
            }
        }
    }
}