using System.IO;
using Asuna.Utils;
using Newtonsoft.Json;
using UnityEditor;

namespace Asuna.Build
{
    public class BuildPlayerConfig
    {
        public string OutputDir { get; set; } = "Build";
        
        public BuildTarget BuildTarget { get; set; }
        
        public string[] ScriptingDefineSymbols { get; set; }

        public static BuildPlayerConfig Load(string path)
        {
            if (!File.Exists(path))
            {
                ADebug.Error("BuildPlayerConfig does not exist");
                return null;
            }
            var content = File.ReadAllText(path);
            if (string.IsNullOrEmpty(content))
            {
                ADebug.Error("BuildPlayerConfig is empty");
                return null;
            }
            var config = JsonConvert.DeserializeObject<BuildPlayerConfig>(content);
            return config;
        }

        public string Dump()
        {
            var content = JsonConvert.SerializeObject(this);
            return content;
        }
    }
}