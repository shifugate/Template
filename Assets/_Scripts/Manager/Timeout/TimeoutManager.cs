using UnityEngine;
using UnityEngine.SceneManagement;
using Assets._Scripts.Manager.Route;
using Assets._Scripts.Manager.Setting;

namespace Assets._Scripts.Manager.Timeout
{
    public class TimeoutManager : MonoBehaviour
    {
        #region Singleton
        private static TimeoutManager instance;
        public static TimeoutManager Instance { get { return instance; } }

        public static void InstanceNW(InitializerManager manager)
        {
            if (instance == null)
            {
                GameObject go = new GameObject("TimeoutManager");

                instance = go.AddComponent<TimeoutManager>();
            }

            instance.Initialize(manager);
        }
        #endregion

        private float time;

        private void Update()
        {
            VerifyClick();
            UpdateTime();
        }

        private void Initialize(InitializerManager manager)
        {
            transform.SetParent(manager.transform);

            SetProperties();
        }

        private void SetProperties()
        {
            ResetTime();
        }

        private void VerifyClick()
        {
            if (Input.GetMouseButtonDown(0))
                time = 0;
        }

        private void UpdateTime()
        {
            if (SceneManager.GetActiveScene().name == RouteManager.Routes.SplashScene.ToString())
                return;

            time += Time.deltaTime;

            if (time > SettingManager.Instance.Data.timeout)
            {
                ResetTime();

                RouteManager.Instance.LoadScene(RouteManager.Routes.SplashScene);
            }
        }

        public void ResetTime()
        {
            time = 0;
        }
    }
}
