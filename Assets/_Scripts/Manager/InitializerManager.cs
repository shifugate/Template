using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Project.Manager.Language;
using Project.Manager.Route;
using Project.Manager.Setting;
using Project.Manager.System;
using Project.Manager.Keyboard;
using Project.Manager.Timeout;
using Project.Manager.Popup;

namespace Project.Manager
{
    public class InitializerManagerComplete : UnityEvent { }

    public class InitializerManager : MonoBehaviour
    {
        #region Singleton
        private static bool initialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitialize()
        {
            if (initialized)
                return;

            initialized = true;

            Instance.Initialize();
        }

        private static InitializerManager instance;
        public static InitializerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("InitializerManager");

                    instance = go.AddComponent<InitializerManager>();

                    DontDestroyOnLoad(go);
                }

                return instance;
            }
        }
        #endregion

        private static bool initializeComplete;
        public static bool InitializeComplete { get { return initializeComplete; } }

        public object ScreenManager { get; private set; }

        private void Initialize()
        {
            InstanceNW();
            StartCoroutine(InitializeCR());
        }

        private void InstanceNW()
        {
            SettingManager.InstanceNW(this);
            PopupManager.InstanceNW(this);
            LanguageManager.InstanceNW(this);
            SystemManager.InstanceNW(this);
            KeyboardManager.InstanceNW(this);
            TimeoutManager.InstanceNW(this);
        }

        private IEnumerator InitializeCR()
        {
            initializeComplete = false;

            yield return RouteManager.InstanceCR(this);

            initializeComplete = true;
        }
    }
}