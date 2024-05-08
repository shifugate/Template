using Assets._Scripts.Manager.Keyboard.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Assets._Scripts.Manager.Keyboard.Board;
using System;
using Assets._Scripts.Util;

namespace Assets._Scripts.Manager.Keyboard
{
    public class KeyboardManager : MonoBehaviour
    {
        #region Singleton
        private static KeyboardManager instance;
        public static KeyboardManager Instance { get { return instance; } }

        public static void InstanceNW(InitializerManager manager)
        {
            if (instance == null)
            {
                instance = GameObject.Instantiate<KeyboardManager>(Resources.Load<KeyboardManager>("Manager/Keyboard/KeyboardManager"));
                instance.name = "KeyboardManager";
            }

            instance.Initialize(manager);
        }
        #endregion 

        public enum Type 
        {
            Normal,
            Email,
            Numeric,
        }

        [SerializeField]
        private RectTransform keyboardHolder;
        [SerializeField]
        private KeyboardBoard keyboardBoard;

        private Dictionary<string, KeyboardModel> contents = new Dictionary<string, KeyboardModel>();

        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        private List<KeyboardBoard> keyboardBoards = new List<KeyboardBoard>();
        public List<KeyboardBoard> KeyboardBoards { get { return keyboardBoards; } }

        private void Initialize(InitializerManager manager)
        {
            transform.SetParent(manager.transform);

            SetProperties();
            SetKeyboards();
        }

        private void SetProperties()
        {
#if UNITY_STANDALONE
            string[] files = Directory.GetFiles($"{Application.streamingAssetsPath}/Manager/Keyboard", "*.json");

            contents.Clear();

            foreach (string file in files)
                contents.Add(Path.GetFileNameWithoutExtension(file), JsonConvert.DeserializeObject<KeyboardModel>(File.ReadAllText(file)));
#elif UNITY_ANDROID || UNITY_IOS
            TextAsset[] assets = Resources.LoadAll<TextAsset>("Manager/Keyboard");

            contents.Clear();

            foreach (TextAsset asset in assets)
                contents.Add(asset.name, JsonConvert.DeserializeObject<KeyboardModel>(asset.text));
#endif
        }

        private void SetKeyboards()
        {
            foreach (KeyValuePair<string, KeyboardModel> content in contents)
            {
                foreach (KeyboardKeyboardModel keyboardKeyboardModel in content.Value.keyboards)
                {
                    SetKeyboardData(content.Value, keyboardKeyboardModel);

                    keyboardBoards.Add(Instantiate(keyboardBoard, keyboardHolder).Setup(content.Key, keyboardKeyboardModel));
                }
            }
        }

        private void SetKeyboardData(KeyboardModel keyboardModel, KeyboardKeyboardModel keyboardKeyboardModel)
        {
            if (keyboardKeyboardModel.font_normal_color == null)
                keyboardKeyboardModel.font_normal_color = keyboardModel.font_normal_color;

            if (keyboardKeyboardModel.font_press_color == null)
                keyboardKeyboardModel.font_press_color = keyboardModel.font_press_color;

            if (keyboardKeyboardModel.release_key == null)
                keyboardKeyboardModel.release_key = keyboardModel.release_key;

            if (keyboardKeyboardModel.press_key == null)
                keyboardKeyboardModel.press_key = keyboardModel.press_key;

            if (keyboardKeyboardModel.lock_key == null)
                keyboardKeyboardModel.lock_key = keyboardModel.lock_key;

            if (keyboardKeyboardModel.background == null)
                keyboardKeyboardModel.background = keyboardModel.background;

            if (keyboardKeyboardModel.width_key == 0)
                keyboardKeyboardModel.width_key = keyboardModel.width_key;

            if (keyboardKeyboardModel.height_key == 0)
                keyboardKeyboardModel.height_key = keyboardModel.height_key;

            if (keyboardKeyboardModel.space_row == 0)
                keyboardKeyboardModel.space_row = keyboardModel.space_row;

            if (keyboardKeyboardModel.space_key == 0)
                keyboardKeyboardModel.space_key = keyboardModel.space_key;

            if (keyboardKeyboardModel.margin_y == 0)
                keyboardKeyboardModel.margin_y = keyboardModel.margin_y;

            if (keyboardKeyboardModel.margin_x == 0)
                keyboardKeyboardModel.margin_y = keyboardModel.margin_y;

            SetTexture(keyboardKeyboardModel.release_key);
            SetTexture(keyboardKeyboardModel.press_key);
            SetTexture(keyboardKeyboardModel.lock_key);
            SetTexture(keyboardKeyboardModel.background);

            foreach (KeyboardRowModel keyboardRowModel in keyboardKeyboardModel.rows)
                SetRowData(keyboardKeyboardModel, keyboardRowModel);
        }

        private void SetRowData(KeyboardKeyboardModel keyboardKeyboardModel, KeyboardRowModel keyboardRowModel)
        {
            if (keyboardRowModel.font_normal_color == null)
                keyboardRowModel.font_normal_color = keyboardKeyboardModel.font_normal_color;

            if (keyboardRowModel.font_press_color == null)
                keyboardRowModel.font_press_color = keyboardKeyboardModel.font_press_color;

            if (keyboardRowModel.release_key == null)
                keyboardRowModel.release_key = keyboardKeyboardModel.release_key;

            if (keyboardRowModel.press_key == null)
                keyboardRowModel.press_key = keyboardKeyboardModel.press_key;

            if (keyboardRowModel.lock_key == null)
                keyboardRowModel.lock_key = keyboardKeyboardModel.lock_key;

            if (keyboardRowModel.background == null)
                keyboardRowModel.background = keyboardKeyboardModel.background;

            if (keyboardRowModel.width_key == 0)
                keyboardRowModel.width_key = keyboardKeyboardModel.width_key;

            if (keyboardRowModel.height_key == 0)
                keyboardRowModel.height_key = keyboardKeyboardModel.height_key;

            if (keyboardRowModel.space_row == 0)
                keyboardRowModel.space_row = keyboardKeyboardModel.space_row;

            if (keyboardRowModel.space_key == 0)
                keyboardRowModel.space_key = keyboardKeyboardModel.space_key;

            if (keyboardRowModel.margin_y == 0)
                keyboardRowModel.margin_y = keyboardKeyboardModel.margin_y;

            if (keyboardRowModel.margin_x == 0)
                keyboardRowModel.margin_y = keyboardKeyboardModel.margin_y;

            SetTexture(keyboardRowModel.release_key);
            SetTexture(keyboardRowModel.press_key);
            SetTexture(keyboardRowModel.lock_key);
            SetTexture(keyboardRowModel.background);

            foreach (KeyboardKeyModel keyboardKeyModel in keyboardRowModel.keys)
                SetKeyData(keyboardRowModel, keyboardKeyModel);
        }

        private void SetKeyData(KeyboardRowModel keyboardRowModel, KeyboardKeyModel keyboardKeyModel)
        {
            if (keyboardKeyModel.font_normal_color == null)
                keyboardKeyModel.font_normal_color = keyboardRowModel.font_normal_color;

            if (keyboardKeyModel.font_press_color == null)
                keyboardKeyModel.font_press_color = keyboardRowModel.font_press_color;

            if (keyboardKeyModel.release_key == null)
                keyboardKeyModel.release_key = keyboardRowModel.release_key;

            if (keyboardKeyModel.press_key == null)
                keyboardKeyModel.press_key = keyboardRowModel.press_key;

            if (keyboardKeyModel.lock_key == null)
                keyboardKeyModel.lock_key = keyboardRowModel.lock_key;

            if (keyboardKeyModel.background == null)
                keyboardKeyModel.background = keyboardRowModel.background;

            if (keyboardKeyModel.width_key == 0)
                keyboardKeyModel.width_key = keyboardRowModel.width_key;

            if (keyboardKeyModel.height_key == 0)
                keyboardKeyModel.height_key = keyboardRowModel.height_key;

            if (keyboardKeyModel.space_row == 0)
                keyboardKeyModel.space_row = keyboardRowModel.space_row;

            if (keyboardKeyModel.space_key == 0)
                keyboardKeyModel.space_key = keyboardRowModel.space_key;

            if (keyboardKeyModel.margin_y == 0)
                keyboardKeyModel.margin_y = keyboardRowModel.margin_y;

            if (keyboardKeyModel.margin_x == 0)
                keyboardKeyModel.margin_y = keyboardRowModel.margin_y;

            SetTexture(keyboardKeyModel.release_key);
            SetTexture(keyboardKeyModel.press_key);
            SetTexture(keyboardKeyModel.lock_key);
            SetTexture(keyboardKeyModel.background);
        }

        private void SetTexture(string name)
        {
            if (name == null)
                return;

            if (textures.ContainsKey(name))
                return;

            Texture2D texture = null;

            try
            {
                texture = new Texture2D(2, 2);
                texture.LoadImage(File.ReadAllBytes($"{Application.streamingAssetsPath}/Manager/Keyboard/Texture/{name}"));
            }
            catch(Exception ex)
            {
                SystemUtil.Log(GetType(), ex, SystemUtil.LogType.Exception);

                return;
            }

            if (texture == null)
                return;

            textures.Add(name, texture);
        }

        public Texture2D GetTexture(string name)
        {
            return textures.ContainsKey(name) ? textures[name] : null;
        }
    }
}
