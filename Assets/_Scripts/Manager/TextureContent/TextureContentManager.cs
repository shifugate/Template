using Project.Util;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Project.Manager.TextureContent
{
    public class TextureContentManager : MonoBehaviour
    {
        #region Singleton
        private static TextureContentManager instance;
        public static TextureContentManager Instance { get { return instance; } }

        public static void InstanceNW(InitializerManager manager)
        {
            if (instance == null)
            {
                GameObject go = new GameObject("TextureContentManager");

                instance = go.AddComponent<TextureContentManager>();
            }

            instance.Initialize(manager);
        }
        #endregion

        private void Initialize(InitializerManager manager)
        {
            transform.SetParent(manager.transform);
        }

        public void LoadTexture(string path, Action<Texture2D> completeCallback)
        {
            StartCoroutine(LoadTextureCR(path, completeCallback));
        }

        private IEnumerator LoadTextureCR(string path, Action<Texture2D> completeCallback)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(path);

            yield return request.SendWebRequest();

            if (HttpUtil.HasRequestError(request))
            {
                SystemUtil.Log(GetType(), request.error, SystemUtil.LogType.Warning);

                completeCallback?.Invoke(null);
            }

            completeCallback?.Invoke(DownloadHandlerTexture.GetContent(request));
        }
    }
}
