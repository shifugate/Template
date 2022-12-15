using Newtonsoft.Json;
using UnityEngine;
using Project.Manager.Setting.Model;
using System.IO;

namespace Project.Manager.Setting
{
    public class SettingManager : MonoBehaviour
    {
        #region Singleton
        private static SettingManager instance;
        public static SettingManager Instance { get { return instance; } }

        public static void InstanceNW(InitializerManager manager)
        {
            if (instance == null)
            {
                GameObject go = new GameObject("SettingManager");

                instance = go.AddComponent<SettingManager>();
                instance.Initialize(manager);
            }
        }
        #endregion

        private SettingModel data;
        public SettingModel Data { get { return data; } }

        private void Initialize(InitializerManager manager)
        {
            transform.SetParent(manager.transform);

            SetProperties();
        }

        private void SetProperties()
        {
            data = JsonConvert.DeserializeObject<SettingModel>(File.ReadAllText($"{Application.streamingAssetsPath}/Manager/Setting/setting.json"));
        }

        public void Save()
        {
            File.WriteAllText($"{Application.streamingAssetsPath}/Manager/Setting/setting.json", JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public void Reload()
        {
            SetProperties();
        }
    }
}
