using System;
using System.IO;
using System.Linq;
using UnityEngine;

//Windows / Mac / Linux:
//MyGame / Logs / Player_YYYY - MM - DD_HH - mm - ss.log

//Android:
//storage / emulated / 0 / Android / data / com.Company.Product / files / Logs / Player_YYYY - MM - DD_HH - mm - ss.log

//iOS:
//< App Sandbox >/ Documents / Logs / Player_YYYY - MM - DD_HH - mm - ss.log

namespace Modules.SharedModule.Runtime.Shared.Scripts.QoL
{
    public class CustomLogger : MonoBehaviour
    {
        private static StreamWriter logWriter;
        private static CustomLogger instance;
        private const int MaxLogs = 20;

        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            if (transform.parent == null)
                DontDestroyOnLoad(gameObject);

            string logsDir = GetLogsDirectory();

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string logPath = Path.Combine(logsDir, $"Player_{timestamp}.log");

            logWriter = new StreamWriter(logPath, false);
            logWriter.AutoFlush = true;

            WriteHeader();

            Application.logMessageReceived += HandleLog;

            CleanupOldLogs(logsDir);
        }

        string GetLogsDirectory()
        {
#if UNITY_STANDALONE
            string exeDir = Path.GetDirectoryName(Application.dataPath);
            string logsDir = Path.Combine(exeDir, "Logs");
#elif UNITY_ANDROID || UNITY_IOS
        string logsDir = Path.Combine(Application.persistentDataPath, "Logs");
#else
        string logsDir = Path.Combine(Application.persistentDataPath, "Logs");
#endif
            if (!Directory.Exists(logsDir))
                Directory.CreateDirectory(logsDir);

            return logsDir;
        }

        void WriteHeader()
        {
            logWriter.WriteLine($"Unity Player Custom Log");
            logWriter.WriteLine($"Start Time: {DateTime.Now}");
            logWriter.WriteLine($"Unity Version: {Application.unityVersion}");
            logWriter.WriteLine($"Platform: {Application.platform}");
            logWriter.WriteLine($"Product: {Application.productName} ({Application.version})");
            logWriter.WriteLine($"Company: {Application.companyName}");
            logWriter.WriteLine($"System: {SystemInfo.operatingSystem}");
            logWriter.WriteLine($"CPU: {SystemInfo.processorType} x{SystemInfo.processorCount}");
            logWriter.WriteLine($"RAM: {SystemInfo.systemMemorySize} MB");
            logWriter.WriteLine(
                $"GPU: {SystemInfo.graphicsDeviceName} ({SystemInfo.graphicsDeviceType}, {SystemInfo.graphicsMemorySize} MB)");
            logWriter.WriteLine(new string('-', 80));
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            string time = DateTime.Now.ToString("HH:mm:ss.fff");
            logWriter.WriteLine($"[{time}] {type}: {logString}");

            if (!string.IsNullOrEmpty(stackTrace))
                logWriter.WriteLine(stackTrace);
        }

        void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
            logWriter?.Close();
        }

        void CleanupOldLogs(string logsDir)
        {
            try
            {
                var files = new DirectoryInfo(logsDir)
                    .GetFiles("Player_*.log")
                    .OrderByDescending(f => f.CreationTimeUtc)
                    .ToList();

                if (files.Count > MaxLogs)
                {
                    foreach (var file in files.Skip(MaxLogs))
                        file.Delete();
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[CustomLogger] Failed to clear old logs: {e.Message}");
            }
        }
    }
}