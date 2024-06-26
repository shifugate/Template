﻿#if UNITY_EDITOR
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets._Scripts.Tool
{
    public class LanguageTool : EditorWindow
    {
#if UNITY_STANDALONE
        private const string SOURCE_PATH = "StreamingAssets/Manager/Language";
#elif UNITY_ANDROID || UNITY_IOS
        private const string SOURCE_PATH = "Resources/Manager/Language";
#endif

        private static readonly string CLASS_EXPORT = $"{Application.dataPath}/_Scripts/Manager/Language/Token/";

        [MenuItem("Tools/Language/Generate Language Map Class")]
        private static void GenerateLanguageMapClass()
        {
            string source = $"{Application.dataPath}/{SOURCE_PATH}";
            string[] files = Directory.GetFiles(source, "*.json");

            Dictionary<string, Dictionary<string, Dictionary<string, string>>> contents = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

            if (files != null)
            {
                foreach (string file in files) 
                    contents.Add(Path.GetFileNameWithoutExtension(file), JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText(file)));

                AssetDatabase.Refresh();
            }

            Dictionary<string, string> toSave = new Dictionary<string, string>();

            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, string>>> save in contents)
                toSave.Add(save.Key, JsonConvert.SerializeObject(save.Value, Formatting.Indented));

            Dictionary<string, Dictionary<string, string>> json = JsonConvert
                .DeserializeObject<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText($"{files[0]}"));

            string result = "using Assets._Scripts.Manager.Language.Attributes;\n\n";
            result += "\n";
            result += "namespace Assets._Scripts.Manager.Language.Token\n{\n";
            result += "    public static class LanguageManagerToken\n    {\n";

            int count = 0;

            foreach (KeyValuePair<string, Dictionary<string, string>> data in json)
            {
                result += $"        public static class {data.Key}\n        {{\n";

                foreach (KeyValuePair<string, string> content in data.Value)
                {
                    result += $"            public static string {content.Key} {{ get {{ return LanguageManager.Instance.GetTranslation(\"{data.Key}\", \"{content.Key}\"); }} }}\n";

                    foreach (string file in files)
                    {
                        result += $"            [LanguageManagerIgnoreToken()]\n";
                        result += $"            public static string {content.Key}_{Path.GetFileNameWithoutExtension(file)} {{ get {{ return LanguageManager.Instance.GetTranslation(\"{Path.GetFileNameWithoutExtension(file)}\", \"{data.Key}\", \"{content.Key}\"); }} }}\n";
                    }
                }

                if (count < json.Count - 1)
                    result += "        }\n\n";
                else
                    result += "        }\n";

                count++;
            }

            result += "    }\n}";

            File.WriteAllText($"{CLASS_EXPORT}LanguageManagerToken.cs", result);

            AssetDatabase.Refresh();

            Debug.Log("Generate Language Map Class Complete");
        }
    }
}
#endif