using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnloopLib.Keyboards.Util;
using Project.Manager.Keyboard.Models;

namespace Project.Manager.Keyboard
{
    [DefaultExecutionOrder(-1000)]
    public class KeyboardManager : MonoBehaviour
    {
        public delegate void EnterKeyBoard();
        public delegate void ShowKeyBoard();
        public delegate void HideKeyBoard();

        public EnterKeyBoard OnEnterKeyBoard;
        public ShowKeyBoard OnShowKeyBoard;
        public HideKeyBoard OnHideKeyBoard;

        public Font font;
        public List<InputField> fields = new List<InputField>();
        public List<string> types = new List<string>();

        public Project.Manager.Keyboard.Component.Keyboard keyboard;

        private InputField lastField;
        public InputField LastField { get { return lastField; } }

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

        private XmlDocument xml;
        private List<KeyboardBackground> backgrounds;
        private List<KeyImage> images;

        private void Initialize(InitializerManager manager)
        {
            transform.SetParent(manager.transform);
        }

        private void Start()
        {
            LoadXML();
            LoadBackground();
            LoadImages();
            LoadKeyboard();
        }

        private void Update()
        {
            VerifyInput();
        }

        private void LoadXML()
        {
            xml = new XmlDocument();
            xml.Load(Application.streamingAssetsPath + "/Keyboards/config.xml");
        }

        private void LoadBackground()
        {
            backgrounds = new List<KeyboardBackground>();

            foreach (XmlNode prop in xml.GetElementsByTagName("background"))
            {
                KeyboardBackground keyboardBackgrounds = new KeyboardBackground();
                keyboardBackgrounds.type = prop.Attributes["type"].Value;

                Texture2D tex = null;
                byte[] fileData = File.ReadAllBytes(Application.streamingAssetsPath + "/Keyboards/layout/" + prop.Attributes["background"].Value);

                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData);

                keyboardBackgrounds.background = tex;

                backgrounds.Add(keyboardBackgrounds);
            }
        }

        private void LoadImages()
        {
            images = new List<KeyImage>();

            foreach (XmlNode prop in xml.GetElementsByTagName("image"))
            {
                KeyImage keyImage = new KeyImage();
                keyImage.type = prop.Attributes["type"].Value;

                Texture2D up = null;
                byte[] fileData = File.ReadAllBytes(Application.streamingAssetsPath + "/Keyboards/layout/" + prop.Attributes["keyup"].Value);

                up = new Texture2D(2, 2);
                up.LoadImage(fileData);

                keyImage.up = up;

                fileData = File.ReadAllBytes(Application.streamingAssetsPath + "/Keyboards/layout/" + prop.Attributes["keydown"].Value);

                Texture2D down = new Texture2D(2, 2);
                down.LoadImage(fileData);

                keyImage.down = down;

                fileData = File.ReadAllBytes(Application.streamingAssetsPath + "/Keyboards/layout/" + prop.Attributes["keyselected"].Value);

                Texture2D selected = new Texture2D(2, 2);
                selected.LoadImage(fileData);

                keyImage.selected = selected;

                images.Add(keyImage);
            }
        }

        private void LoadKeyboard()
        {
            keyboard = gameObject.AddComponent<Project.Manager.Keyboard.Component.Keyboard>();
            keyboard.KeyboardIntialize(xml, images, backgrounds);
        }

        private void VerifyInput()
        {
            if (fields.Count == 0)
                keyboard.FocusOut();

            for (int i = 0; i < fields.Count; i++)
            {
                lastField = fields[i];

                if (lastField.isFocused)
                {
                    keyboard.FocusIn(lastField, types[i]);

                    break;
                }
                else
                {
                    keyboard.FocusOut();
                }
            }
        }

        private void AddField(InputField input, string type, int index = -1)
        {
            if (!fields.Contains(input))
            {
                if (index > -1)
                {
                    fields.Insert(index, input);
                    types.Insert(index, type);
                }
                else
                {
                    fields.Add(input);
                    types.Add(type);
                }
            }
        }

        public InputField AddInputField(InputField input, string type)
        {
            AddField(input, type);

            return input;
        }

        public InputField AddInputField(InputField input, string type, int index)
        {
            AddField(input, type, index);

            return input;
        }

        public InputField AddInputField(string name, string type, Font font, Sprite background)
        {
            InputField input = InputUtil.CreateInputField(name, font, background);

            AddField(input, type);

            return input;
        }

        public InputField AddInputField(string name, string type, Font font, Sprite background, int index)
        {
            InputField input = InputUtil.CreateInputField(name, font, background);

            AddField(input, type, index);

            return input;
        }

        public InputField AddInputField(string name, string type, Font font, Sprite background, GameObject father)
        {
            InputField input = InputUtil.CreateInputField(name, font, background, father);

            AddField(input, type);

            return input;
        }

        public InputField AddInputField(string name, string type, Font font, Sprite background, GameObject father, int index)
        {
            InputField input = InputUtil.CreateInputField(name, font, background, father);

            AddField(input, type, index);

            return input;
        }

        public void RemoveField(InputField input)
        {
            int index = fields.IndexOf(input);

            if (index > -1)
            {
                fields.RemoveAt(index);
                types.RemoveAt(index);
            }
        }

        public void RemoveField(string name)
        {
            for (int i = fields.Count - 1; i >= 0; i--)
            {
                if (fields[i].name == name)
                {
                    fields.RemoveAt(i);
                    types.RemoveAt(i);
                }
            }
        }

        public void RemoveAllFields()
        {
            fields.Clear();
            types.Clear();
        }
    }
}