using Assets._Scripts.Manager.Keyboard.Model;
using Assets._Scripts.Manager.Keyboard.Row;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts.Manager.Keyboard.Board
{
    public class KeyboardBoard : MonoBehaviour
    {
        [SerializeField]
        private RectTransform rowHolder;
        [SerializeField]
        private KeyboardRow keyboardRow;
        [SerializeField]
        private RawImage backgroundImage;
        [SerializeField]
        private VerticalLayoutGroup verticalLayoutGroup;

        private string language;
        public string Language { get { return language; } }

        private KeyboardKeyboardModel keyboardKeyboardModel;
        public KeyboardKeyboardModel KeyboardKeyboardModel { get { return keyboardKeyboardModel; } }

        private void SetSpace()
        {
            verticalLayoutGroup.spacing = keyboardKeyboardModel.space_row;
        }

        private void SetMargin()
        {
            verticalLayoutGroup.padding = new RectOffset(keyboardKeyboardModel.margin_x, keyboardKeyboardModel.margin_x, keyboardKeyboardModel.margin_y, keyboardKeyboardModel.margin_y);
        }

        private void SetTextures()
        {
            backgroundImage.texture = KeyboardManager.Instance.GetTexture(keyboardKeyboardModel.background);
            backgroundImage.gameObject.SetActive(backgroundImage.texture != null);
        }

        private void SetRows()
        {
            foreach (KeyboardRowModel keyboardRowModel in keyboardKeyboardModel.rows)
                Instantiate(keyboardRow, rowHolder).Setup(keyboardRowModel);
        }

        public KeyboardBoard Setup(string language, KeyboardKeyboardModel keyboardKeyboardModel)
        {
            this.language = language;
            this.keyboardKeyboardModel = keyboardKeyboardModel;

            SetSpace();
            SetMargin();
            SetTextures();
            SetRows();

            return this;
        }
    }
}
