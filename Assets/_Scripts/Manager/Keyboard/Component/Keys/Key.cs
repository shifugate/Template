using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets._Scripts.Manager.Keyboard.Models;

namespace Assets._Scripts.Manager.Keyboard.Component.Keys
{
    public class Key : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public delegate void DownHandler(Key key);
        public DownHandler OnDownHandler;

        public delegate void UpHandler(Key key);
        public DownHandler OnUpHandler;

        public string text;
        public string subtext;
        public List<Accent> accents;
        public List<Key> subkeys;
        public float space;
        public Sprite down;
        public Sprite up;
        public Sprite selected;
        public string type;
        public string level;
        public float subdistance;
        public bool notshift;
        public Button keyContainer;
        public Image upImage;
        public Image downImage;
        public Image selectedImage;
        public Canvas subkeyContainer;
        public bool showDown;
        public string fontcolor;
        public string fontcolorover;

        private Canvas canvasSort;
        private Text textField;
        private float timeHide = 0;
        private long timeSub = -1;
        private float subWidth = 0;
        private float subHeight = 0;
        private float subHeightKey = 0;
        private Key subky = null;

        public void KeyIntialize(Button keyContainer, KeyImage image, string text = null, string accents = null, List<Key> subkeys = null,
            float space = 0, string type = null, string level = null, float subdistance = 0, bool notshift = false, int fontsize = 24,
            string fontcolor = "255,255,255", string fontcolorover = "0,0,0")
        {
            gameObject.AddComponent<GraphicRaycaster>();

            this.keyContainer = keyContainer;
            this.text = text;
            this.type = type;
            this.level = level;
            this.space = space;
            this.subdistance = subdistance;
            this.subkeys = subkeys;
            this.notshift = notshift;
            this.fontcolor = fontcolor;
            this.fontcolorover = fontcolorover;

            if (text != null)
            {
                Color color = GetColor(fontcolor); ;

                textField = new GameObject("text", typeof(Text)).GetComponent<Text>();
                textField.transform.SetParent(gameObject.transform);
                textField.rectTransform.sizeDelta = new Vector2();
                textField.horizontalOverflow = HorizontalWrapMode.Overflow;
                textField.verticalOverflow = VerticalWrapMode.Overflow;
                textField.font = KeyboardManager.Instance.font;
                textField.fontSize = fontsize;
                textField.alignment = TextAnchor.MiddleCenter;
                textField.color = color;
                textField.text = text;
            }

            up = Sprite.Create(image.up, new Rect(0, 0, image.up.width, image.up.height), new Vector2(0, 1));
            down = Sprite.Create(image.down, new Rect(0, 0, image.down.width, image.down.height), new Vector2(0, 1));
            selected = Sprite.Create(image.selected, new Rect(0, 0, image.selected.width, image.selected.height), new Vector2(0, 1));

            upImage = gameObject.AddComponent<Image>();
            upImage.sprite = up;
            upImage.SetNativeSize();

            RectTransform rectTransform = upImage.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.localPosition = new Vector3();

            downImage = new GameObject("DownImage", typeof(Image)).GetComponent<Image>();
            downImage.transform.SetParent(gameObject.transform);
            downImage.sprite = down;
            downImage.SetNativeSize();
            downImage.color = new Color(downImage.color.r, downImage.color.g, downImage.color.b, 0);

            rectTransform = downImage.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.localPosition = new Vector3();

            selectedImage = new GameObject("SelectedImage", typeof(Image)).GetComponent<Image>();
            selectedImage.transform.SetParent(gameObject.transform);
            selectedImage.sprite = selected;
            selectedImage.SetNativeSize();
            selectedImage.color = new Color(selectedImage.color.r, selectedImage.color.g, selectedImage.color.b, 0);

            rectTransform = selectedImage.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.localPosition = new Vector3();

            if (textField != null)
                textField.transform.SetAsLastSibling();

            keyContainer.targetGraphic = upImage;
            keyContainer.transition = Selectable.Transition.None;

            this.accents = new List<Accent>();

            string[] accentsSplit = accents != null ? accents.Split(';') : new string[0];

            foreach (string accentStr in accentsSplit)
            {
                string[] accentEqual = accentStr.Split('=');
                string[] accentPlus = accentEqual[0].Split('+');

                Accent accentAdd = new Accent();
                accentAdd.accent = accentPlus[0];
                accentAdd.character = accentPlus[1];
                accentAdd.result = accentEqual[1];

                this.accents.Add(accentAdd);
            }

            if (subkeys != null && subkeys.Count > 0)
            {
                float xPos = 0;
                float yPos = 0;
                Key oldKey = null;

                subkeyContainer = new GameObject("SubkeysContainer", typeof(Canvas)).GetComponent<Canvas>();
                subkeyContainer.transform.SetParent(gameObject.transform);
                subkeyContainer.gameObject.SetActive(false);
                RectTransform subkeyContainerRec = subkeyContainer.GetComponent<RectTransform>();
                subkeyContainerRec.anchorMin = new Vector2(0, 1);
                subkeyContainerRec.anchorMax = new Vector2(0, 1);
                subkeyContainerRec.pivot = new Vector2(0, 1);
                subkeyContainerRec.anchoredPosition = new Vector2(0, 0);

                for (int i = 0; i < subkeys.Count; i++)
                {
                    Key key = subkeys[i];
                    RectTransform rec = key.GetComponent<RectTransform>();

                    rec.anchoredPosition = new Vector2(xPos, yPos);

                    if (i == 0 || (i + 1) % 5 != 0)
                    {
                        xPos += key.upImage.rectTransform.sizeDelta.x + key.space;
                    }
                    else
                    {
                        xPos = 0;
                        yPos += oldKey.upImage.rectTransform.sizeDelta.y + key.space;
                    }

                    key.transform.SetParent(subkeyContainer.transform);

                    oldKey = key;

                    if (subWidth < oldKey.upImage.rectTransform.anchoredPosition.x + oldKey.upImage.rectTransform.sizeDelta.x)
                        subWidth = oldKey.upImage.rectTransform.anchoredPosition.x + oldKey.upImage.rectTransform.sizeDelta.x;

                    if (subHeight < oldKey.upImage.rectTransform.sizeDelta.y + oldKey.space)
                        subHeight = oldKey.upImage.rectTransform.sizeDelta.y + oldKey.space;

                    if (subHeightKey < oldKey.upImage.rectTransform.anchoredPosition.y)
                        subHeightKey = oldKey.upImage.rectTransform.anchoredPosition.y;
                }
            }

            SetSort(this);
        }

        public void SetSort(Key key)
        {
            canvasSort = key.GetComponent<Canvas>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (textField != null)
                textField.color = GetColor(fontcolorover);

            canvasSort.overrideSorting = true;
            canvasSort.sortingOrder = 1000000;

            showDown = true;

            if (OnDownHandler != null)
                OnDownHandler(this);

            timeSub = DateTime.Now.Ticks;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (textField != null)
                textField.color = GetColor(fontcolor);

            canvasSort.sortingOrder = 0;
            canvasSort.overrideSorting = false;

            showDown = false;

            if (OnUpHandler != null && subky == null)
                OnUpHandler(this);

            timeSub = -1;
        }

        public void UpdateTextCase(bool up)
        {
            if (up)
                text = text.ToUpper();
            else
                text = text.ToLower();

            textField.text = text;

            if (subkeys != null)
            {
                foreach (Key sub in subkeys)
                    sub.UpdateTextCase(up);
            }
        }

        public void VerifyShowSub()
        {
            if (subkeyContainer != null && !subkeyContainer.gameObject.activeInHierarchy && timeSub > -1)
            {
                double start = Math.Round(new TimeSpan(timeSub).TotalMilliseconds);
                double end = Math.Round(new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds);

                int time = int.Parse((end - start).ToString());

                if (time > 250)
                {
                    subky = null;
                    subkeyContainer.gameObject.SetActive(true);
                }
            }
            else if (subkeyContainer != null && subkeyContainer.gameObject.activeInHierarchy && timeSub == -1)
            {
                if (subky != null)
                {
                    if (OnUpHandler != null)
                        OnUpHandler(subky);

                    subky = null;
                }

                subkeyContainer.gameObject.SetActive(false);
            }
        }

        private void VerifySubKey()
        {
            Key currKey = null;

            if (subkeyContainer != null && subkeyContainer.gameObject.activeInHierarchy)
            {
                RectTransform subkeyContainerRec = subkeyContainer.GetComponent<RectTransform>();

                RectTransform keyRec = GetComponent<RectTransform>();
                float posKX = keyRec.anchoredPosition.x;
                float posX = 0;
                float posY = subHeight;

                if (posKX + subWidth < 0)
                    posX = 0;
                else if (posKX + subWidth > Screen.width)
                    posX = -subWidth + upImage.rectTransform.sizeDelta.x;

                subkeyContainerRec.anchoredPosition = new Vector2(posX, posY);

                Vector3[] corners = new Vector3[4];
                subkeyContainerRec.GetWorldCorners(corners);

                float dist = 1000000;

                for (int i = 0; i < subkeys.Count; i++)
                {
                    Key key = subkeys[i];
                    RectTransform rec = key.GetComponent<RectTransform>();

                    corners = new Vector3[4];
                    rec.GetWorldCorners(corners);
                    Rect newRect = new Rect(corners[0], corners[2] - corners[0]);

                    Vector2 input = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    Vector2 target = new Vector2(newRect.x + upImage.rectTransform.sizeDelta.x / 2, newRect.y + +upImage.rectTransform.sizeDelta.y / 2);
                    float mag = (target - input).magnitude;

                    if (dist > mag)
                    {
                        dist = mag;

                        currKey = key;
                    }
                }
            }

            if (currKey != null)
            {
                for (int i = 0; i < subkeys.Count; i++)
                {
                    Key key = subkeys[i];

                    if (currKey == key)
                    {
                        key.showDown = true;
                        subky = key;
                    }
                    else
                    {
                        key.showDown = false;
                    }
                }
            }
        }

        private void ShowDown()
        {
            if (downImage.color.a < 1)
                downImage.color = new Color(downImage.color.r, downImage.color.g, downImage.color.b, 1);

            timeHide = 0;
        }

        private void HideDown()
        {
            if (downImage.color.a > 0)
            {
                float alpha = Mathf.Lerp(downImage.color.a, 0, timeHide);

                timeHide += Time.deltaTime;

                downImage.color = new Color(downImage.color.r, downImage.color.g, downImage.color.b, alpha);
            }
        }

        private Color GetColor(string fontcolor)
        {
            string[] colorSplit = fontcolor.Split(',');
            Color color = new Color(float.Parse(colorSplit[0]) / 255, float.Parse(colorSplit[1]) / 255, float.Parse(colorSplit[2]) / 255);

            return color;
        }

        public void ForceHide()
        {
            showDown = false;

            downImage.color = new Color(downImage.color.r, downImage.color.g, downImage.color.b, 0);
            selectedImage.color = new Color(selectedImage.color.r, selectedImage.color.g, selectedImage.color.b, 0);

            timeHide = 0;
        }

        void Update()
        {
            VerifyShowSub();
            VerifySubKey();

            if ((type == null || type.ToLower() != "shift") && showDown)
                ShowDown();
            else if (type == null || type.ToLower() != "shift")
                HideDown();
        }
    }
}