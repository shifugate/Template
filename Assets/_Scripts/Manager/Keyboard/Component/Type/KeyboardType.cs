using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Project.Manager.Keyboard.Component.Keys;
using Project.Manager.Keyboard.Component.Level;
using Project.Manager.Keyboard.Models;
using Project.Manager.Keyboard.Util;

namespace Project.Manager.Keyboard.Component.Type
{
    class KeyboardType : MonoBehaviour
    {
        public string type;
        public float width;
        public float height;
        public float xposin;
        public float xposout;
        public float yposin;
        public float yposout;
        public float alphain;
        public float alphaout;
        public float time;
        public Canvas keyboardContainer;
        public CanvasGroup canvasGroup;
        public GraphicRaycaster graphicRaycaster;

        private float xPos = 0;
        private float yPos = 0;
        private float maxWidth = 0;
        private float maxHeight = 0;
        private Image background;
        private List<Key> keys = new List<Key>();
        private List<KeyboardLine> lines = new List<KeyboardLine>();
        private List<KeyboardLevel> levels = new List<KeyboardLevel>();
        private KeyboardLine line;
        private KeyboardLevel level;
        private Key oldKey;
        private bool show;
        private RectTransform selfRect;
        private int shiftState = 0;
        private Key waitingAccent;
        private float showhidetime;

        public void KeyboardTypeIntialize(Canvas keyboardContainer, KeyboardBackground background = null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();

            selfRect = GetComponent<RectTransform>();

            RectTransform rectTransform = keyboardContainer.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.offsetMax = new Vector2();
            rectTransform.offsetMin = new Vector2();
            rectTransform.localPosition = new Vector3();

            this.keyboardContainer = keyboardContainer;

            if (background != null)
            {
                this.background = new GameObject("Background", typeof(Image)).GetComponent<Image>();
                this.background.transform.SetParent(gameObject.transform);
                this.background.overrideSprite = Sprite.Create(background.background, new Rect(0, 0, background.background.width, background.background.height), new Vector2(0, 1));
                this.background.SetNativeSize();

                rectTransform.offsetMax = new Vector2();
                rectTransform.offsetMin = new Vector2();

                rectTransform = this.background.GetComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(0, 1);
                rectTransform.pivot = new Vector2(0, 1);
                rectTransform.localPosition = new Vector3();
            }
        }

        public void AddKey(Key key, bool newLine = false, bool newLevel = false, string levelId = null)
        {
            RectTransform rec;

            keys.Add(key);

            if (level == null)
            {
                level = new KeyboardLevel();
                level.level = levelId;
                level.canvas = new GameObject("canvas", typeof(Canvas)).GetComponent<Canvas>();
                level.canvas.transform.SetParent(keyboardContainer.transform);
                rec = level.canvas.GetComponent<RectTransform>();
                rec.anchoredPosition = new Vector2();
                rec.sizeDelta = new Vector2();
                rec.localScale = new Vector3(1, 1, 1);
                rec.anchorMin = new Vector2(0, 1);
                rec.anchorMax = new Vector2(0, 1);
                rec.pivot = new Vector2(0, 1);

                levels.Add(level);
            }

            if (line == null)
            {
                line = new KeyboardLine();
                line.canvas = new GameObject("line", typeof(Canvas)).GetComponent<Canvas>();
                line.canvas.transform.SetParent(level.canvas.transform);
                rec = line.canvas.GetComponent<RectTransform>();
                rec.anchoredPosition = new Vector2();
                rec.sizeDelta = new Vector2();
                rec.localScale = new Vector3(1, 1, 1);
                rec.anchorMin = new Vector2(0, 1);
                rec.anchorMax = new Vector2(0, 1);
                rec.pivot = new Vector2(0, 1);

                lines.Add(line);
            }

            if (newLevel)
            {
                xPos = 0;
                yPos = 0;
                maxWidth = 0;
                maxHeight = 0;

                level = new KeyboardLevel();
                level.level = levelId;
                level.canvas = new GameObject("canvas", typeof(Canvas)).GetComponent<Canvas>();
                level.canvas.transform.SetParent(keyboardContainer.transform);
                rec = level.canvas.GetComponent<RectTransform>();
                rec.anchoredPosition = new Vector2();
                rec.sizeDelta = new Vector2();
                rec.localScale = new Vector3(1, 1, 1);
                rec.anchorMin = new Vector2(0, 1);
                rec.anchorMax = new Vector2(0, 1);
                rec.pivot = new Vector2(0, 1);

                levels.Add(level);

                line = new KeyboardLine();
                line.canvas = new GameObject("line", typeof(Canvas)).GetComponent<Canvas>();
                line.canvas.transform.SetParent(level.canvas.transform);
                rec = line.canvas.GetComponent<RectTransform>();
                rec.anchoredPosition = new Vector2();
                rec.sizeDelta = new Vector2();
                rec.localScale = new Vector3(1, 1, 1);
                rec.anchorMin = new Vector2(0, 1);
                rec.anchorMax = new Vector2(0, 1);
                rec.pivot = new Vector2(0, 1);

                lines.Add(line);

                level.canvas.gameObject.SetActive(false);
            }
            else if (newLine)
            {
                xPos = 0;
                yPos -= oldKey.upImage.rectTransform.sizeDelta.y + key.space;

                line = new KeyboardLine();
                line.canvas = new GameObject("line", typeof(Canvas)).GetComponent<Canvas>();
                line.canvas.transform.SetParent(level.canvas.transform);
                rec = line.canvas.GetComponent<RectTransform>();
                rec.anchoredPosition = new Vector2(0, yPos);
                rec.sizeDelta = new Vector2();
                rec.localScale = new Vector3(1, 1, 1);
                rec.anchorMin = new Vector2(0, 1);
                rec.anchorMax = new Vector2(0, 1);
                rec.pivot = new Vector2(0, 1);

                lines.Add(line);
            }

            key.transform.SetParent(line.canvas.transform);

            rec = key.GetComponent<RectTransform>();
            rec.anchoredPosition = new Vector2(xPos, 0);

            xPos += key.upImage.rectTransform.sizeDelta.x + key.space;

            line.width = xPos - key.space;

            oldKey = key;

            level.height = yPos - key.upImage.rectTransform.sizeDelta.y;

            if (line.width > maxWidth)
                maxWidth = line.width;

            if (level.height > maxHeight)
                maxHeight = level.height;

            if (background == null)
            {
                width = maxWidth;
                height = maxHeight;

                foreach (KeyboardLine ln in lines)
                {
                    rec = ln.canvas.GetComponent<RectTransform>();
                    rec.anchoredPosition = new Vector2(maxWidth / 2 - ln.width / 2, rec.anchoredPosition.y);
                }
            }
            else
            {
                width = background.rectTransform.sizeDelta.x;
                height = background.rectTransform.sizeDelta.y;

                foreach (KeyboardLine ln in lines)
                {
                    rec = ln.canvas.GetComponent<RectTransform>();
                    rec.anchoredPosition = new Vector2(background.rectTransform.anchoredPosition.x + background.rectTransform.sizeDelta.x / 2 - ln.width / 2, rec.anchoredPosition.y);
                }

                foreach (KeyboardLevel lv in levels)
                {
                    rec = lv.canvas.GetComponent<RectTransform>();
                    rec.anchoredPosition = new Vector2(rec.anchoredPosition.x, -background.rectTransform.sizeDelta.y / 2 - lv.height / 2);
                }
            }

            RectTransform rectTransform = keyboardContainer.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, height);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        void LateUpdate()
        {
            if (show && showhidetime <= 1)
            {
                showhidetime += Time.deltaTime;
                
                selfRect.anchoredPosition = Vector2.Lerp(selfRect.anchoredPosition, new Vector2(xposin, yposin), showhidetime);
            }
            else if (!show && showhidetime <= 1)
            {
                showhidetime += Time.deltaTime;

                selfRect.anchoredPosition = Vector2.Lerp(selfRect.anchoredPosition, new Vector2(xposout, yposout), showhidetime);

                if (selfRect.anchoredPosition.y == yposout)
                    gameObject.SetActive(false);
            }
        }

        public void ShowKeyboard()
        {
            if (show)
                return;

            show = true;

            showhidetime = 0;

            gameObject.SetActive(true);
        }

        public void HideKeyboard()
        {
            if (!show)
                return;

            show = false;

            foreach (Key key in keys)
                key.ForceHide();

            showhidetime = 0;
        }

        public string DelText(InputField field)
        {
            if (field.text.Length > 0)
                return field.text.Substring(0, field.text.Length - 1);

            return "";
        }

        public void SwapKeyboard(string level = null)
        {
            if (level == null)
                level = levels[0].level;

            foreach (KeyboardLevel lv in levels)
            {
                if (lv.level == level)
                    lv.canvas.gameObject.SetActive(true);
                else
                    lv.canvas.gameObject.SetActive(false);
            }
        }

        public void SwapShift(bool reset = false)
        {
            if (!reset)
            {
                if (shiftState == 0)
                {
                    shiftState = 1;

                    ToShift(true, shiftState);
                }
                else if (shiftState == 1)
                {
                    shiftState = 2;

                    ToShift(true, shiftState);
                }
                else
                {
                    shiftState = 0;

                    ToShift(false, shiftState);
                }
            }
            else
            {
                shiftState = 0;

                ToShift(false, shiftState);
            }
        }

        private void ToShift(bool up, int state)
        {
            foreach (Key key in keys)
            {
                if (key.type == null && !key.notshift)
                {
                    key.UpdateTextCase(up);
                }
                else if (key.type != null && key.type.ToLower() == "shift")
                {
                    if (state == 0)
                    {
                        key.downImage.color = new Color(key.downImage.color.r, key.downImage.color.g, key.downImage.color.b, 0);
                        key.selectedImage.color = new Color(key.selectedImage.color.r, key.selectedImage.color.g, key.selectedImage.color.b, 0);
                    }
                    else if (state == 1)
                    {
                        key.downImage.color = new Color(key.downImage.color.r, key.downImage.color.g, key.downImage.color.b, 1);
                        key.selectedImage.color = new Color(key.selectedImage.color.r, key.selectedImage.color.g, key.selectedImage.color.b, 0);
                    }
                    else if (state == 2)
                    {
                        key.downImage.color = new Color(key.downImage.color.r, key.downImage.color.g, key.downImage.color.b, 0);
                        key.selectedImage.color = new Color(key.selectedImage.color.r, key.selectedImage.color.g, key.selectedImage.color.b, 1);
                    }
                }
            }
        }

        public void EnterText(InputField field, bool last = false)
        {
            if (field.lineType == InputField.LineType.MultiLineNewline)
            {
                field.text = field.text + "\n";
            }
            else if (last)
            {
                if (KeyboardManager.Instance.OnEnterKeyBoard != null)
                    KeyboardManager.Instance.OnEnterKeyBoard();

                KeyboardEventUtil.EnterKeyBoard?.Invoke();
            }
        }

        public void TypeChar(Key key, InputField field)
        {
            if (waitingAccent)
            {
                SetTextInField(GetAccentChar(key), field);

                waitingAccent = null;
            }
            else if (key.accents != null && key.accents.Count > 0)
            {
                waitingAccent = key;
            }
            else
            {
                if (key.subtext != null)
                {
                    SetTextInField(key.subtext, field);

                    key.subtext = null;
                }
                else
                {
                    SetTextInField(key.text, field);
                }
            }
        }

        private void SetTextInField(string text, InputField field)
        {
            field.text += text;

            if (shiftState == 1)
                SwapShift(true);
        }

        private string GetAccentChar(Key key)
        {
            string text = key.text.ToLower();

            if (key.subtext != null)
                text = key.subtext.ToLower();

            key.subtext = null;

            foreach (Accent accent in waitingAccent.accents)
            {
                if (accent.character == text)
                {
                    if (shiftState == 0)
                        return accent.result;
                    else
                        return accent.result.ToUpper();
                }
            }

            return waitingAccent.text != text ? waitingAccent.text + text : waitingAccent.text;
        }
    }
}