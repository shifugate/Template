using Assets._Scripts.Manager.Keyboard.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts.Manager.Keyboard.Key
{
    public class KeyboardKey : MonoBehaviour
    {
        [SerializeField]
        private RectTransform rectTransform;
        [SerializeField]
        private RawImage releaseImage;
        [SerializeField]
        private RawImage pressImage;
        [SerializeField]
        private RawImage lockImage;
        [SerializeField]
        private TextMeshProUGUI keyText;

        private KeyboardKeyModel keyboardKeyModel;

        private Color fontReleaseColor;
        private Color fontPressColor;
        private Color fontLockColor;

        private void SetSize()
        {
            rectTransform.sizeDelta = new Vector2(keyboardKeyModel.width_key, keyboardKeyModel.height_key);
        }

        private void SetTextures()
        {
            releaseImage.texture = KeyboardManager.Instance.GetTexture(keyboardKeyModel.release_key);
            releaseImage.gameObject.SetActive(releaseImage.texture != null);

            pressImage.texture = KeyboardManager.Instance.GetTexture(keyboardKeyModel.press_key);
            pressImage.gameObject.SetActive(pressImage.texture != null);
            pressImage.color = new Color(1, 1, 1, 0);

            lockImage.texture = KeyboardManager.Instance.GetTexture(keyboardKeyModel.lock_key);
            lockImage.gameObject.SetActive(lockImage.texture != null);
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

            SetSize();
            SetTextures();
            SetColors();
            SetText();

            return this;
        }

        public void OnPointerDown()
        {

        }

        public void OnPointerUp()
        {

        }

        public void OnPointerEnter()
        {

        }
    }
}
