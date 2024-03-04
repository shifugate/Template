#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Project.Tool
{
    public class PersistenceTool : EditorWindow
    {
        [MenuItem("Project/Persistence/Clear Persistence")]
        private static void ClearPersistence()
        {
            string[] directories = Directory.GetDirectories(Application.persistentDataPath);
            string[] files = Directory.GetFiles(Application.persistentDataPath);

            foreach (string directory in directories)
            {
                try
                {
                    Directory.Delete(directory, true);
                }
                catch(Exception ex)
                {
                    Debug.Log(ex);
                }
            }

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }

            PlayerPrefs.DeleteAll();
        }
    }
}
#endif