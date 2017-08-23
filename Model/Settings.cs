using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Model
{
    public static class Settings
    {
        static Settings()
        {
            var settings = File.ReadAllLines("settings.txt");
            Url = settings[0];
            Username = settings[1];
            Password = settings[2];
            SelectedPackId = settings[3];
            SelectedAuthor = settings[4];
        }

        public static void UpdateSettings(string selectedPackId, string selectedAuthor)
        {
            if (String.Compare(selectedPackId, SelectedPackId, StringComparison.OrdinalIgnoreCase) != 0 ||
                String.Compare(selectedAuthor, SelectedAuthor, StringComparison.OrdinalIgnoreCase) != 0)
            {
                File.Delete("settings.txt");
                File.WriteAllLines("settings.txt", new[] {Url, Username, Password, selectedPackId, selectedAuthor});
            }
        }

        public static string SelectedAuthor { get; }

        public static string SelectedPackId { get; }

        public static string Password { get; }

        public static string Username { get; }

        public static string Url { get; }
    }
}
