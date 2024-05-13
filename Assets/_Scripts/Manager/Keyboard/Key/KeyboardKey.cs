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
        private Image releaseImage;
        [SerializeField]
        private Image pressImage;
        [SerializeField]
        private Image lockImage;
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

            name = "KEYBOARDMANAGER";

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
