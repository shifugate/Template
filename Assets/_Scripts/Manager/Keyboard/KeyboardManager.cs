using Assets._Scripts.Manager.Keyboard.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Assets._Scripts.Manager.Keyboard.Board;
using System;
using Assets._Scripts.Util;
using TMPro;
using Assets._Scripts.Manager.Keyboard.Data;
using System.Collections;

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
            Normal_Mobile,
            Email_Mobile,
            Numeric_Mobile,
        }
        public enum Direction 
        {
            Left,
            Top,
            Right,
            Bottom,
        }

        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private RectTransform keyboardHolder;
        [SerializeField]
        private KeyboardBoard keyboardBoard;

        private Dictionary<string, KeyboardModel> contents = new Dictionary<string, KeyboardModel>();

        private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        private List<KeyboardBoard> keyboardBoards = new List<KeyboardBoard>();
        public List<KeyboardBoard> KeyboardBoards { get { return keyboardBoards; } }

        private List<KeyboardData> keyboardDatas;

        private Coroutine focusCR;

        private TMP_InputField inputField;

        public Vector3 Scale { get { return canvas.transform.localScale; } }

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
                foreach (int type in content.Value.keyboards)
                {
#if UNITY_STANDALONE
                    try
                    {
                        KeyboardKeyboardModel keyboardKeyboardModel = JsonConvert.DeserializeObject<KeyboardKeyboardModel>(File.ReadAllText($"{Application.streamingAssetsPath}/Manager/Keyboard/Type/{type}_{content.Key}.json"));
                        keyboardKeyboardModel.type = (Type)type;

                        SetKeyboardData(content.Value, keyboardKeyboardModel);

                        keyboardBoards.Add(Instantiate(keyboardBoard, keyboardHolder).Setup(content.Key, keyboardKeyboardModel));
                    }
                    catch(Exception ex)
                    {
                        SystemUtil.Log(GetType(), ex, SystemUtil.LogType.Exception);
                    }
#elif UNITY_ANDROID || UNITY_IOS
                    try
                    {
                        KeyboardKeyboardModel keyboardKeyboardModel = JsonConvert.DeserializeObject<KeyboardKeyboardModel>(Resources.Load<TextAsset>($"Manager/Keyboard/Type/{type}_{content.Key}").text);
                        keyboardKeyboardModel.type = (Type)type;

                        SetKeyboardData(content.Value, keyboardKeyboardModel);

                        keyboardBoards.Add(Instantiate(keyboardBoard, keyboardHolder).Setup(content.Key, keyboardKeyboardModel));
                    }
                    catch (Exception ex)
                    {
                        SystemUtil.Log(GetType(), ex, SystemUtil.LogType.Exception);
                    }
#endif
                }
            }
        }

        private void SetKeyboardData(KeyboardModel keyboardModel, KeyboardKeyboardModel keyboardKeyboardModel)
        {
            if (keyboardKeyboardModel.font_release_color == null)
                keyboardKeyboardModel.font_release_color = keyboardModel.font_release_color;

            if (keyboardKeyboardModel.font_press_color == null)
                keyboardKeyboardModel.font_press_color = keyboardModel.font_press_color;

            if (keyboardKeyboardModel.font_lock_color == null)
                keyboardKeyboardModel.font_lock_color = keyboardModel.font_lock_color;

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

            if (keyboardKeyboardModel.start_at == null)
                keyboardKeyboardModel.start_at = keyboardModel.start_at;

            if (keyboardKeyboardModel.show_at == null)
                keyboardKeyboardModel.show_at = keyboardModel.show_at;

            if (keyboardKeyboardModel.show_margin == 0)
                keyboardKeyboardModel.show_margin = keyboardModel.show_margin;

            SetSprite(keyboardKeyboardModel.release_key);
            SetSprite(keyboardKeyboardModel.press_key);
            SetSprite(keyboardKeyboardModel.lock_key);
            SetSprite(keyboardKeyboardModel.background);

            foreach (KeyboardRowModel keyboardRowModel in keyboardKeyboardModel.rows)
                SetRowData(keyboardKeyboardModel, keyboardRowModel);
        }

        private void SetRowData(KeyboardKeyboardModel keyboardKeyboardModel, KeyboardRowModel keyboardRowModel)
        {
            if (keyboardRowModel.font_release_color == null)
                keyboardRowModel.font_release_color = keyboardKeyboardModel.font_release_color;

            if (keyboardRowModel.font_press_color == null)
                keyboardRowModel.font_press_color = keyboardKeyboardModel.font_press_color;

            if (keyboardRowModel.font_lock_color == null)
                keyboardRowModel.font_lock_color = keyboardKeyboardModel.font_lock_color;

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

            SetSprite(keyboardRowModel.release_key);
            SetSprite(keyboardRowModel.press_key);
            SetSprite(keyboardRowModel.lock_key);
            SetSprite(keyboardRowModel.background);

            foreach (KeyboardKeyModel keyboardKeyModel in keyboardRowModel.keys)
                SetKeyData(keyboardRowModel, keyboardKeyModel);
        }

        private void SetKeyData(KeyboardRowModel keyboardRowModel, KeyboardKeyModel keyboardKeyModel)
        {
            if (keyboardKeyModel.font_release_color == null)
                keyboardKeyModel.font_release_color = keyboardRowModel.font_release_color;

            if (keyboardKeyModel.font_press_color == null)
                keyboardKeyModel.font_press_color = keyboardRowModel.font_press_color;

            if (keyboardKeyModel.font_lock_color == null)
                keyboardKeyModel.font_lock_color = keyboardRowModel.font_lock_color;

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

            SetSprite(keyboardKeyModel.release_key);
            SetSprite(keyboardKeyModel.press_key);
            SetSprite(keyboardKeyModel.lock_key);
            SetSprite(keyboardKeyModel.background);
        }

        private void SetSprite(KeyboardSpriteModel spriteModel)
        {
            if (spriteModel?.file == null)
                return;

            if (sprites.ContainsKey(spriteModel.file))
                return;

            Texture2D texture = null;
            Sprite sprite = null;

            try
            {
#if UNITY_STANDALONE
                texture = new Texture2D(2, 2);
                texture.LoadImage(File.ReadAllBytes($"{Application.streamingAssetsPath}/Manager/Keyboard/Texture/{spriteModel.file}"));
#elif UNITY_ANDROID || UNITY_IOS
                texture = Resources.Load<Texture2D>($"Manager/Keyboard/Texture/{Path.GetFileNameWithoutExtension(spriteModel.file)}");
#endif

                if (spriteModel.margin == null || spriteModel.margin.Count != 4)
                    spriteModel.margin = new List<int>(new int[] { 0, 0, 0, 0 });

                sprite = Sprite.Create(texture, 
                    new Rect(0, 0, texture.width, texture.height), 
                    new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect, 
                    new Vector4(spriteModel.margin[0], spriteModel.margin[1], spriteModel.margin[2], spriteModel.margin[3]));
            }
            catch (Exception ex)
            {
                SystemUtil.Log(GetType(), ex, SystemUtil.LogType.Exception);

                return;
            }

            if (sprite == null)
                return;

            sprites.Add(spriteModel.file, sprite);
        }

        private IEnumerator FocusCR()
        {
            while (keyboardDatas != null && keyboardDatas.Count > 0)
            {
                keyboardDatas.RemoveAll(data => data?.inputField == null);

                foreach (KeyboardData keyboardData in keyboardDatas)
                {
                    if (keyboardData.inputField.isFocused && inputField != keyboardData.inputField)
                    {
                        inputField = keyboardData.inputField;

                        if (inputField != null)
                            foreach (KeyboardBoard keyboard in keyboardBoards)
                                if (keyboard.KeyboardKeyboardModel.type == keyboardData.type)
                                    keyboard.Show();
                                else
                                    keyboard.Hide();
                    }
                }

                yield return null;
            }
        }

        public Sprite GetSprite(KeyboardSpriteModel spriteModel)
        {
            return spriteModel?.file == null || !sprites.ContainsKey(spriteModel.file) ? null : sprites[spriteModel.file];
        }

        public bool HasSpriteBorder(Sprite sprite)
        {
            return sprite != null && sprite.border != null && (sprite.border[0] > 0 || sprite.border[1] > 0 || sprite.border[2] > 0 || sprite.border[3] > 0);
        }

        public void SetInputFields(List<KeyboardData> data)
        {
            keyboardDatas = data;

            if (focusCR != null)
                StopCoroutine(focusCR);

            focusCR = StartCoroutine(FocusCR());
        }

        public void AddInputField(KeyboardData data)
        {
            if (data == null)
                return;

            if (keyboardDatas == null)
                keyboardDatas = new List<KeyboardData>();

            keyboardDatas.RemoveAll(kData => kData?.inputField == null);

            if (!keyboardDatas.Exists(kData => kData.inputField == data.inputField))
                keyboardDatas.Add(data);

            if (focusCR != null)
                StopCoroutine(focusCR);

            focusCR = StartCoroutine(FocusCR());
        }

        public void RemoveInputField(KeyboardData data)
        {
            if (data == null)
                return;

            if (keyboardDatas == null || keyboardDatas.Count == 0)
                return;

            if (focusCR != null)
                StopCoroutine(focusCR);

            keyboardDatas.RemoveAll(kData => kData?.inputField == null);
            keyboardDatas.RemoveAll(kData => kData == data);

            if (keyboardDatas.Count == 0)
                return;

            focusCR = StartCoroutine(FocusCR());
        }

        public void RemoveInputField(TMP_InputField inputField)
        {
            if (inputField == null)
                return;

            if (keyboardDatas == null || keyboardDatas.Count == 0)
                return;

            if (focusCR != null)
                StopCoroutine(focusCR);

            keyboardDatas.RemoveAll(data => data?.inputField == null);
            keyboardDatas.RemoveAll(data => data.inputField == inputField);

            if (keyboardDatas.Count == 0)
                return;

            focusCR = StartCoroutine(FocusCR());
        }
    }
}
