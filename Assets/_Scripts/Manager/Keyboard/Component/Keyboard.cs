using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Project.Manager.Keyboard.Component.Keys;
using Project.Manager.Keyboard.Component.Type;
using Project.Manager.Keyboard.Models;
using Project.Manager.Keyboard.Util;

namespace Project.Manager.Keyboard.Component
{
    public class Keyboard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public enum Event { EnterKeyBoard, ShowKeyboard, HideKeyboard }

        private long keyTimer;
        private List<KeyboardType> keyboardList;
        private XmlDocument xml;
        private List<KeyImage> images;
        private List<KeyboardBackground> backgrounds;
        private KeyboardType currKeyboard;
        private bool keyboardIsActive;
        private long verifyFocusTime = -1;
        private bool inBeravior;
        private InputField currField;
        private string currText;
        private bool hide;
        private Key keyPressed;
        private long deleteTimer;
        private long downStartTimer = 0;

        public void KeyboardIntialize(XmlDocument xml, List<KeyImage> images, List<KeyboardBackground> backgrounds)
        {
            this.xml = xml;
            this.images = images;
            this.backgrounds = backgrounds;

            CreateKeyboards();
        }

        private void CreateKeyboards()
        {
            keyboardList = new List<KeyboardType>();

            hide = xml.GetElementsByTagName("keyboards")[0].Attributes["hide"] != null ? bool.Parse(xml.GetElementsByTagName("keyboards")[0].Attributes["hide"].Value) : true;
            keyTimer = xml.GetElementsByTagName("keyboards")[0].Attributes["keytimer"] != null ? long.Parse(xml.GetElementsByTagName("keyboards")[0].Attributes["keytimer"].Value) : 500;

            foreach (XmlNode keyboard in xml.GetElementsByTagName("keyboard"))
            {
                bool newLine = false;
                bool newLevel = false;

                Canvas keyboardContainer = new GameObject("Keyboard", typeof(Canvas)).GetComponent<Canvas>();
                keyboardContainer.transform.SetParent(gameObject.transform);
                keyboardContainer.gameObject.layer = gameObject.layer;

                KeyboardType keyboardType = keyboardContainer.gameObject.AddComponent<KeyboardType>();
                keyboardType.KeyboardTypeIntialize(keyboardContainer, keyboard.Attributes["background"] != null ? GetKeyboardBackground(keyboard.Attributes["background"].Value) : null);
                keyboardType.type = keyboard.Attributes["type"].Value;

                float spacekb = keyboard.Attributes["space"] != null ? float.Parse(keyboard.Attributes["space"].Value, CultureInfo.InvariantCulture) : 0;
                KeyImage imagekb = keyboard.Attributes["image"] != null ? GetKeyImage(keyboard.Attributes["image"].Value) : null;
                float subdistancekb = keyboard.Attributes["subdistance"] != null ? float.Parse(keyboard.Attributes["subdistance"].Value, CultureInfo.InvariantCulture) : 0;
                bool notshiftkb = keyboard.Attributes["notshift"] != null ? bool.Parse(keyboard.Attributes["notshift"].Value) : false;
                int fontsizekb = keyboard.Attributes["fontsize"] != null ? int.Parse(keyboard.Attributes["fontsize"].Value) : 24;
                string fontcolorkb = keyboard.Attributes["fontcolor"] != null ? keyboard.Attributes["fontcolor"].Value : "255,255,255";
                string fontcoloroverkb = keyboard.Attributes["fontcolorover"] != null ? keyboard.Attributes["fontcolorover"].Value : "255,255,255";

                foreach (XmlNode level in keyboard.SelectNodes("level"))
                {
                    float spacelv = level.Attributes["space"] != null ? float.Parse(level.Attributes["space"].Value, CultureInfo.InvariantCulture) : spacekb;
                    KeyImage imagelv = level.Attributes["image"] != null ? GetKeyImage(level.Attributes["image"].Value) : imagekb;
                    float subdistancelv = level.Attributes["subdistance"] != null ? float.Parse(level.Attributes["subdistance"].Value, CultureInfo.InvariantCulture) : subdistancekb;
                    bool notshiftlv = level.Attributes["notshift"] != null ? bool.Parse(level.Attributes["notshift"].Value) : notshiftkb;
                    int fontsizelv = level.Attributes["fontsize"] != null ? int.Parse(level.Attributes["fontsize"].Value) : fontsizekb;
                    string fontcolorlv = level.Attributes["fontcolor"] != null ? level.Attributes["fontcolor"].Value : fontcolorkb;
                    string fontcoloroverlv = level.Attributes["fontcolorover"] != null ? level.Attributes["fontcolorover"].Value : fontcoloroverkb;

                    foreach (XmlNode line in level.SelectNodes("line"))
                    {
                        float spaceln = line.Attributes["space"] != null ? float.Parse(line.Attributes["space"].Value, CultureInfo.InvariantCulture) : spacelv;
                        KeyImage imageln = line.Attributes["image"] != null ? GetKeyImage(line.Attributes["image"].Value) : imagelv;
                        float subdistanceln = line.Attributes["subdistance"] != null ? float.Parse(line.Attributes["subdistance"].Value, CultureInfo.InvariantCulture) : subdistancelv;
                        bool notshiftln = line.Attributes["notshift"] != null ? bool.Parse(line.Attributes["notshift"].Value) : notshiftlv;
                        int fontsizeln = line.Attributes["fontsize"] != null ? int.Parse(line.Attributes["fontsize"].Value) : fontsizelv;
                        string fontcolorln = line.Attributes["fontcolor"] != null ? line.Attributes["fontcolor"].Value : fontcolorlv;
                        string fontcoloroverln = line.Attributes["fontcolorover"] != null ? line.Attributes["fontcolorover"].Value : fontcoloroverlv;

                        foreach (XmlNode key in line.SelectNodes("key"))
                        {
                            float spaceky = key.Attributes["space"] != null ? float.Parse(key.Attributes["space"].Value, CultureInfo.InvariantCulture) : spaceln;
                            KeyImage imageky = key.Attributes["image"] != null ? GetKeyImage(key.Attributes["image"].Value) : imageln;
                            float subdistanceky = key.Attributes["subdistance"] != null ? float.Parse(key.Attributes["subdistance"].Value, CultureInfo.InvariantCulture) : subdistanceln;
                            bool notshiftky = key.Attributes["notshift"] != null ? bool.Parse(key.Attributes["notshift"].Value) : notshiftln;
                            int fontsizeky = key.Attributes["fontsize"] != null ? int.Parse(key.Attributes["fontsize"].Value) : fontsizeln;
                            string fontcolorky = key.Attributes["fontcolor"] != null ? key.Attributes["fontcolor"].Value : fontcolorln;
                            string fontcoloroverky = key.Attributes["fontcolorover"] != null ? key.Attributes["fontcolorover"].Value : fontcoloroverln;
                            List<Key> subkeys = new List<Key>();

                            foreach (XmlNode subkey in key.SelectNodes("subkey"))
                            {
                                float spacesk = subkey.Attributes["space"] != null ? float.Parse(subkey.Attributes["space"].Value, CultureInfo.InvariantCulture) : spaceky;
                                KeyImage imagesk = subkey.Attributes["image"] != null ? GetKeyImage(subkey.Attributes["image"].Value) : imageky;
                                float subdistancesk = subkey.Attributes["subdistance"] != null ? float.Parse(subkey.Attributes["subdistance"].Value, CultureInfo.InvariantCulture) : subdistanceky;
                                bool notshiftsk = subkey.Attributes["notshift"] != null ? bool.Parse(subkey.Attributes["notshift"].Value) : notshiftky;
                                int fontsizesk = subkey.Attributes["fontsize"] != null ? int.Parse(subkey.Attributes["fontsize"].Value) : fontsizeky;
                                string fontcolorsk = subkey.Attributes["fontcolor"] != null ? subkey.Attributes["fontcolor"].Value : fontcolorky;
                                string fontcoloroversk = subkey.Attributes["fontcolorover"] != null ? subkey.Attributes["fontcolorover"].Value : fontcoloroverky;

                                Button subContainer = new GameObject("Key", typeof(Button)).GetComponent<Button>();

                                Key subkeyres = subContainer.gameObject.AddComponent<Key>();
                                subkeyres.KeyIntialize(subContainer, imagesk,
                                    subkey.Attributes["text"] != null ? subkey.Attributes["text"].Value : null,
                                    null,
                                    null,
                                    spacesk,
                                    null,
                                    null,
                                    subdistancesk,
                                    notshiftsk,
                                    fontsizesk,
                                    fontcolorsk,
                                    fontcoloroversk);

                                subkeys.Add(subkeyres);

                                subkeyres.OnDownHandler += new Key.DownHandler(OnDownHandler);
                                subkeyres.OnUpHandler += new Key.DownHandler(OnUpHandler);
                            }

                            Button keyContainer = new GameObject("Key", typeof(Button)).GetComponent<Button>();

                            Key keyres = keyContainer.gameObject.AddComponent<Key>();
                            keyres.KeyIntialize(keyContainer, imageky,
                                    key.Attributes["text"] != null ? key.Attributes["text"].Value : null,
                                    key.Attributes["accent"] != null ? key.Attributes["accent"].Value : null,
                                    subkeys,
                                    spaceky,
                                    key.Attributes["type"] != null ? key.Attributes["type"].Value : null,
                                    key.Attributes["level"] != null ? key.Attributes["level"].Value : null,
                                    subdistanceky,
                                    notshiftky,
                                    fontsizeky,
                                    fontcolorky,
                                    fontcoloroverky);

                            if (subkeys != null)
                            {
                                foreach (Key sub in subkeys)
                                    sub.SetSort(keyres);
                            }

                            keyboardType.AddKey(keyres, newLine, newLevel, level.Attributes["level"].Value);

                            keyres.OnDownHandler += new Key.DownHandler(OnDownHandler);
                            keyres.OnUpHandler += new Key.DownHandler(OnUpHandler);

                            newLine = false;
                            newLevel = false;
                        }

                        newLine = true;
                    }

                    newLevel = true;
                }

                RectTransform rec = keyboardType.GetComponent<RectTransform>();
                keyboardType.xposin = keyboard.Attributes["xposin"] != null && keyboard.Attributes["xposin"].Value != "" ? float.Parse(keyboard.Attributes["xposin"].Value, CultureInfo.InvariantCulture) : Screen.width / 2 - keyboardType.width / 2;
                keyboardType.xposout = keyboard.Attributes["xposout"] != null && keyboard.Attributes["xposout"].Value != "" ? float.Parse(keyboard.Attributes["xposout"].Value, CultureInfo.InvariantCulture) : Screen.width / 2 - keyboardType.width / 2;
                keyboardType.yposin = keyboard.Attributes["yposin"] != null && keyboard.Attributes["yposin"].Value != "" ? float.Parse(keyboard.Attributes["yposin"].Value, CultureInfo.InvariantCulture) : Screen.height - keyboardType.height;
                keyboardType.yposout = keyboard.Attributes["yposout"] != null && keyboard.Attributes["yposout"].Value != "" ? float.Parse(keyboard.Attributes["yposout"].Value, CultureInfo.InvariantCulture) : Screen.height;
                keyboardType.alphain = keyboard.Attributes["alphain"] != null && keyboard.Attributes["alphain"].Value != "" ? float.Parse(keyboard.Attributes["alphain"].Value, CultureInfo.InvariantCulture) : 1;
                keyboardType.alphaout = keyboard.Attributes["alphaout"] != null && keyboard.Attributes["alphaout"].Value != "" ? float.Parse(keyboard.Attributes["alphaout"].Value, CultureInfo.InvariantCulture) : 1;
                
                float scale = keyboard.Attributes["scale"] != null && keyboard.Attributes["scale"].Value != "" ? float.Parse(keyboard.Attributes["scale"].Value, CultureInfo.InvariantCulture) : 1;

                rec.anchoredPosition = new Vector2(keyboardType.xposout, keyboardType.yposout);
                rec.localScale = new Vector3(scale, scale, scale);
                keyboardType.canvasGroup.alpha = keyboardType.alphaout;
                keyboardType.time = keyboard.Attributes["time"] != null && keyboard.Attributes["time"].Value != "" ? float.Parse(keyboard.Attributes["time"].Value, CultureInfo.InvariantCulture) : 0.25f;

                keyboardList.Add(keyboardType);

                keyboardType.gameObject.SetActive(false);
            }
        }

        private KeyboardBackground GetKeyboardBackground(string type)
        {
            foreach (KeyboardBackground background in backgrounds)
            {
                if (background.type == type)
                    return background;
            }

            return null;
        }

        private KeyImage GetKeyImage(string type)
        {
            foreach (KeyImage image in images)
            {
                if (image.type == type)
                    return image;
            }

            return null;
        }

        public void SetKeyboard(string type)
        {
            if (hide && type == null)
                keyboardIsActive = false;

            if (type == null && currKeyboard != null)
            {
                currKeyboard.SwapShift(true);
                currKeyboard.HideKeyboard();
                currKeyboard = null;

                if (KeyboardManager.Instance.OnHideKeyBoard != null)
                    KeyboardManager.Instance.OnHideKeyBoard();

                KeyboardEventUtil.HideKeyboard?.Invoke();

                return;
            }

            if (hide && type == null && currKeyboard != null && currKeyboard.gameObject.activeInHierarchy)
            {
                currKeyboard.SwapShift(true);
                currKeyboard.HideKeyboard();

                if (KeyboardManager.Instance.OnHideKeyBoard != null)
                    KeyboardManager.Instance.OnHideKeyBoard();

                KeyboardEventUtil.HideKeyboard?.Invoke();

                return;
            }

            //CORRIGIR E REMOVER ISSO DEPOIS
            if (KeyboardManager.Instance == null)
            {
                return;
            }

            if (!hide && type == null && KeyboardManager.Instance.fields.Count > 0)
            {
                currField = KeyboardManager.Instance.fields[0];
                currText = currField.text;

                OnPointerUp(null);

                return;
            }

            foreach (KeyboardType keyboard in keyboardList)
            {
                if (keyboard.type == type)
                {
                    if (currKeyboard != null && currKeyboard.gameObject.activeInHierarchy && currKeyboard != keyboard)
                    {
                        currKeyboard.SwapShift(true);
                        currKeyboard.HideKeyboard();
                    }

                    currKeyboard = keyboard;
                    currKeyboard.ShowKeyboard();

                    if (!keyboardIsActive)
                    {
                        keyboardIsActive = true;

                        if (KeyboardManager.Instance.OnShowKeyBoard != null)
                            KeyboardManager.Instance.OnShowKeyBoard();

                        KeyboardEventUtil.ShowKeyboard?.Invoke();
                    }

                    break;
                }
            }
        }

        public void OnDownHandler(Key key)
        {
            inBeravior = true;
            keyPressed = key;
            downStartTimer = DateTime.Now.Ticks;

            deleteTimer = keyTimer;

            if (currField)
                currField.ActivateInputField();
        }

        public void OnUpHandler(Key key)
        {
            keyPressed = null;

            if (currKeyboard != null)
            {
                if (key.type != null && key.type != "" && key.type.ToString().ToLower() == "backspace")
                    currField.text = currKeyboard.DelText(currField);
                else if (key.type != null && key.type != "" && key.type.ToString().ToLower() == "swap")
                    currKeyboard.SwapKeyboard(key.level);
                else if (key.type != null && key.type != "" && key.type.ToString().ToLower() == "tab")
                    SetNextActiveTextFields();
                else if (key.type != null && key.type != "" && key.type.ToString().ToLower() == "shift")
                    currKeyboard.SwapShift();
                else if (key.type != null && key.type != "" && key.type.ToString().ToLower() == "enter")
                    currKeyboard.EnterText(currField, true);
                else if (key.type == null)
                    currKeyboard.TypeChar(key, currField);
            }

            currText = currField.text;

            OnPointerUp(null);
        }

        public void SetNextActiveTextFields()
        {
            if (KeyboardManager.Instance.fields.Count > 0)
            {
                int last = -1;

                for (int i = 0; i < KeyboardManager.Instance.fields.Count; i++)
                {
                    InputField field = KeyboardManager.Instance.fields[i];

                    if (field == currField)
                    {
                        last = i;

                        break;
                    }
                }

                if (last == KeyboardManager.Instance.fields.Count - 1)
                    last = 0;
                else
                    last += 1;

                currField = KeyboardManager.Instance.fields[last];
                currText = currField.text;

                OnPointerUp(null);
            }
        }

        public void SetActiveTextFields(InputField newField)
        {
            if (KeyboardManager.Instance.fields.Count > 0)
            {
                int last = -1;

                for (int i = 0; i < KeyboardManager.Instance.fields.Count; i++)
                {
                    InputField field = KeyboardManager.Instance.fields[i];

                    if (field == newField)
                    {
                        last = i;

                        break;
                    }
                }

                currField = KeyboardManager.Instance.fields[last];
                currText = currField.text;

                OnPointerUp(null);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            inBeravior = true;

            deleteTimer = keyTimer;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (currField != null)
            {
                verifyFocusTime = -1;

                currField.Select();
            }

            inBeravior = false;
        }

        public void FocusIn(InputField field, string type)
        {
            verifyFocusTime = -1;

            currField = field;
            currField.onValueChanged.AddListener(delegate { ValueChanged(); });

            KeyboardType keyboard = keyboardList.Find(x => x.type == type);

            if (keyboard != null)
                SetKeyboard(keyboard.type);
        }

        public void FocusOut()
        {
            if (verifyFocusTime == -1 && keyboardIsActive)
                verifyFocusTime = DateTime.Now.Ticks;
        }

        private void ValueChanged()
        {
            if (currText == null)
                currText = "";

            currText = currField.text;
        }

        private void Update()
        {
            VerifyFocusTimer();
            VerifyCaret();
            VerifyInit();
            VerifyKeyPress();
        }

        private void VerifyFocusTimer()
        {
            if (verifyFocusTime >= 0 && !inBeravior)
            {
                double start = Math.Round(new TimeSpan(verifyFocusTime).TotalMilliseconds);
                double end = Math.Round(new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds);

                int time = int.Parse((end - start).ToString());

                if (time > 250)
                {
                    verifyFocusTime = -1;

                    SetKeyboard(null);
                }
            }
        }

        private void VerifyCaret()
        {
            if (currField != null)
                currField.MoveTextEnd(false);
        }

        private void VerifyInit()
        {
            if (!hide && currKeyboard == null)
                SetKeyboard(null);
        }

        private void VerifyKeyPress()
        {
            if (currKeyboard != null && downStartTimer > 0 && inBeravior && keyPressed != null)
            {
                double start = Math.Round(new TimeSpan(downStartTimer).TotalMilliseconds);
                double end = Math.Round(new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds);

                int time = int.Parse((end - start).ToString());

                if (keyPressed.type != null && keyPressed.type.ToLower() == "backspace" && time > deleteTimer)
                {
                    downStartTimer = DateTime.Now.Ticks;

                    currField.text = currKeyboard.DelText(currField);

                    deleteTimer -= 50;
                }
                else if (keyPressed.type != null && keyPressed.type.ToLower() == "enter" && time > deleteTimer)
                {
                    downStartTimer = DateTime.Now.Ticks;

                    currKeyboard.EnterText(currField);

                    deleteTimer -= 50;
                }
            }
        }
    }
}