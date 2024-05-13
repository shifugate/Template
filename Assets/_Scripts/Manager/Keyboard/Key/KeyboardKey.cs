using Assets._Scripts.Manager.Keyboard.Model;
using Assets._Scripts.Util;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts.Manager.Keyboard.Key
{
    public class KeyboardKey : MonoBehaviour
    {
        [SerializeField]
        private KeyboardHoldKey keyboardHoldKey;
        [SerializeField]
        private RectTransform rectTransform;
        [SerializeField]
        private RectTransform holdHolder;
        [SerializeField]
        private Image releaseImage;
        [SerializeField]
        private Image pressImage;
        [SerializeField]
        private Image lockImage;
        [SerializeField]
        private TextMeshProUGUI keyText;

        private RectTransform keyboardTransform;

        public string Key { get { return keyText.text; } }

        private KeyboardKeyModel keyboardKeyModel;

        private List<string> holdList;

        private KeyboardHoldKey holdKey;

        private Color fontReleaseColor;
        private Color fontPressColor;
        private Color fontLockColor;

        private bool press;

        private float time;

        private Vector2 position;

        private void Awake()
        {
            SetProperties();
        }

        private void Update()
        {
            UpdateInput();
            UpdateHolder();
        }

        private void SetProperties()
        {
            keyboardTransform = (RectTransform)transform.parent.parent;
        }

        private void UpdateInput()
        {
            if (!press)
            {
                if (!holdHolder.parent.Equals(transform))
                {
                    holdHolder.SetParent(transform, true);
                    holdHolder.anchoredPosition = Vector3.zero;
                }

                foreach (Transform transform in holdHolder)
                    Destroy(transform.gameObject);

                return;
            }

            time += Time.deltaTime;

            if (time <= keyboardKeyModel.click_time)
                return;

            holdList = keyboardKeyModel.normal_hold;

            if (holdList != null && holdList.Count > 0 && holdHolder.childCount == 0)
                foreach (string key in holdList)
                    Instantiate(keyboardHoldKey, holdHolder).Setup(keyboardKeyModel, key);

            if (!holdHolder.parent.Equals(keyboardTransform))
            {
                holdHolder.SetParent(keyboardTransform, true);

                Debug.Log(holdHolder.anchoredPosition.y);

                position = holdHolder.anchoredPosition;
            }

            GameObject keyObject = ScreenUtil.GetUIOverPointerByName("KEYBOARDMANAGER_KEY");

            if (keyObject != null)
            {
                KeyboardHoldKey holdKey = keyObject.GetComponent<KeyboardHoldKey>();

                if (this.holdKey != null && this.holdKey != holdKey)
                    this.holdKey.OutKey();

                if (holdKey != null && this.holdKey != holdKey)
                    holdKey.OverKey();

                this.holdKey = holdKey;
            }
            else if (holdKey != null)
            {
                holdKey.OutKey();
            }
        }

        private void UpdateHolder()
        {
            if (keyboardKeyModel == null || !holdHolder.parent.Equals(keyboardTransform))
                return;

            float x = position.x;
            float y = position.y + keyboardKeyModel.height_key;

            if (x - holdHolder.rect.width / 2 < -keyboardTransform.rect.width / 2)
                x = -keyboardTransform.rect.width / 2 + holdHolder.rect.width / 2;
            else if (x + holdHolder.rect.width / 2 > keyboardTransform.rect.width / 2)
                x = keyboardTransform.rect.width / 2 - holdHolder.rect.width / 2;

            Debug.Log($"{y + holdHolder.rect.height / 2} : {keyboardTransform.rect.height / 2}");

            //if (y + holdHolder.rect.height / 2 > keyboardTransform.rect.height / 2)

            holdHolder.anchoredPosition = new Vector2(x, y);
        }

        private void SetSize()
        {
            rectTransform.sizeDelta = new Vector2(keyboardKeyModel.width_key, keyboardKeyModel.height_key);
        }

        private void SetTextures()
        {
            releaseImage.sprite = KeyboardManager.Instance.GetSprite(keyboardKeyModel.release_key);
            releaseImage.type = KeyboardManager.Instance.HasSpriteBorder(releaseImage.sprite) ? Image.Type.Sliced : Image.Type.Simple;
            releaseImage.gameObject.SetActive(releaseImage.sprite != null);

            pressImage.sprite = KeyboardManager.Instance.GetSprite(keyboardKeyModel.press_key);
            pressImage.type = KeyboardManager.Instance.HasSpriteBorder(pressImage.sprite) ? Image.Type.Sliced : Image.Type.Simple;
            pressImage.gameObject.SetActive(pressImage.sprite != null);
            pressImage.color = new Color(1, 1, 1, 0);

            lockImage.sprite = KeyboardManager.Instance.GetSprite(keyboardKeyModel.lock_key);
            lockImage.type = KeyboardManager.Instance.HasSpriteBorder(lockImage.sprite) ? Image.Type.Sliced : Image.Type.Simple;
            lockImage.gameObject.SetActive(lockImage.sprite != null);
            lockImage.color = new Color(1, 1, 1, 0);
        }

        private void SetColors()
        {
            ColorUtility.TryParseHtmlString(keyboardKeyModel.font_release_color, out fontReleaseColor);
            ColorUtility.TryParseHtmlString(keyboardKeyModel.font_press_color, out fontPressColor);
            ColorUtility.TryParseHtmlString(keyboardKeyModel.font_lock_color, out fontLockColor);
        }

        private void SetText()
        {
            keyText.text = keyboardKeyModel.normal;
            keyText.color = fontReleaseColor;
        }

        public KeyboardKey Setup(KeyboardKeyModel keyboardKeyModel)
        {
            this.keyboardKeyModel = keyboardKeyModel;

            name = "KEYBOARDMANAGER_KEY";

            SetSize();
            SetTextures();
            SetColors();
            SetText();

            return this;
        }

        public void OverKey()
        {
            if (press)
                return;

            press = true;

            keyText.color = fontPressColor;
            pressImage.color = new Color(1, 1, 1, 1);

            time = 0;
        }

        public void OutKey()
        {
            if (!press)
                return;

            press = false;

            keyText.color = fontReleaseColor;
            pressImage.color = new Color(1, 1, 1, 0);

            if (time <= keyboardKeyModel.click_time)
                SystemUtil.Log(GetType(), Key);
            else if (holdKey != null)
                SystemUtil.Log(GetType(), holdKey.Key);

            holdKey = null;
        }
    }
}
