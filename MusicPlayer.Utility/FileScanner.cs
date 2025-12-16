using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace MusicPlayer.Utility
{
    /// <summary>
    /// Singleton class for scanning and loading PMIS Bot audio files.
    /// Designed for PMIS Bot Music Player to dynamically retrieve task or notification sounds.
    /// </summary>
    public class FileScanner
    {
        // Singleton instance
        private static FileScanner _instance;

        public static FileScanner Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FileScanner();
                return _instance;
            }
        }

        /// <summary>
        /// Base folder where PMIS bot audio files are stored
        /// </summary>
        public string FilePath { get; private set; } = @"./PMISBotAudio";

        /// <summary>
        /// Private constructor for singleton
        /// </summary>
        private FileScanner()
        {
            // Could initialize default folders or logging here
        }

        /// <summary>
        /// Updates the audio files folder for the PMIS bot
        /// </summary>
        public void UpdateFilePath(string newFilePath)
        {
            if (!string.IsNullOrEmpty(newFilePath))
                FilePath = newFilePath;
        }

        /// <summary>
        /// Scans the folder and returns all supported PMIS bot audio files
        /// </summary>
        public ObservableCollection<string> ScanBotAudioFiles()
        {
            var audioFiles = new ObservableCollection<string>();

            try
            {
                string folder = FilePath;

                // PMIS bot supports mp3 and wav files
                var allowedExtensions = new[] { ".mp3", ".wav" };

                List<string> files = Directory.GetFiles(folder)
                    .Where(f => allowedExtensions.Any(ext => f.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                audioFiles = new ObservableCollection<string>(files);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PMISBot FileScanner] Error reading files: {ex.Message}");
            }

            return audioFiles;
        }
    }
}
