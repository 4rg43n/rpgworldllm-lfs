using System;
using System.IO;
using UnityEngine;

namespace RPGWorldLLM.Logging
{
    public class LogManager : MonoBehaviour
    {
        private static LogManager instance;
        public static LogManager Instance => instance;

        private string logDirectory;
        private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);

            logDirectory = Path.Combine(Application.persistentDataPath, "Logs");
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);
        }

        public void Log(LogMessageData logMessage)
        {
            string filePath = Path.Combine(logDirectory, logMessage.LogFileName);

            RotateIfNeeded(filePath);

            DateTime now = DateTime.Now;
            string timestamp = $"{now:HH:mm:ss}/{now.Day}/{now.Month}/{now.Year}";
            string trimmedData = logMessage.data.TrimEnd('\n', '\r');
            string entry = $"{logMessage.messageType} {timestamp}\n{trimmedData}\n\n\n\n";

            File.AppendAllText(filePath, entry);
        }

        private void RotateIfNeeded(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Length >= MaxFileSize)
            {
                DateTime now = DateTime.Now;
                string rotatedName = $"{Path.GetFileNameWithoutExtension(filePath)}_{now:yyyyMMdd_HHmmss}{Path.GetExtension(filePath)}";
                string rotatedPath = Path.Combine(Path.GetDirectoryName(filePath), rotatedName);
                File.Move(filePath, rotatedPath);
            }
        }
    }
}
