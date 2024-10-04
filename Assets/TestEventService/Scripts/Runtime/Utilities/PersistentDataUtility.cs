using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using TestEventService.Scopes;
using UnityEngine;

namespace TestEventService.Utilities
{
    public static class PersistentDataUtility
    {
        public static string DataPath => Application.persistentDataPath;

        private static string GetFullPath(string path)
        {
            using (StringBuilderScope.Create(out var stringBuilder))
            {
                return stringBuilder.Append(DataPath)
                                    .Append(Path.AltDirectorySeparatorChar)
                                    .Append(path)
                                    .ToString();
            }
        }

        public static void Delete(string path)
        {
            var fullPath = GetFullPath(path);
            var backupPath = $"{fullPath}.backup";

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            if (File.Exists(backupPath))
            {
                File.Delete(backupPath);
            }
        }

        public static void WriteAllText(string path, string value)
        {
            var fullPath = GetFullPath(path);
            var backupPath = $"{fullPath}.backup";
            var tempPath = $"{fullPath}.temp";

            File.WriteAllText(tempPath, value, Encoding.UTF8);

            if (File.Exists(fullPath))
            {
                File.Replace(tempPath, fullPath, backupPath);
            }
            else
            {
                File.Move(tempPath, fullPath);
            }
        }

        public static bool TryReadJson(string path, out string json)
        {
            var fullPath = GetFullPath(path);

            return TryRead(fullPath, out json) || (TryRestore(fullPath) && TryRead(fullPath, out json));

            static bool TryRead(string path, out string json)
            {
                if (!File.Exists(path))
                {
                    json = default;

                    return false;
                }

                try
                {
                    json = File.ReadAllText(path);
                    JObject.Parse(json);

                    return true;
                }
                catch (Exception exception)
                {
                    Debug.LogWarning(exception);
                    json = default;

                    return false;
                }
            }

            static bool TryRestore(string path)
            {
                var backupPath = $"{path}.backup";

                if (!File.Exists(backupPath))
                {
                    return false;
                }

                if (File.Exists(path))
                {
                    File.Replace(backupPath, path, null);
                }
                else
                {
                    File.Move(backupPath, path);
                }

                return true;
            }
        }
    }
}