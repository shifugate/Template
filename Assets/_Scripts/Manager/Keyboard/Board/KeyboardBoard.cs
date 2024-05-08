using Assets._Scripts.Manager.Keyboard.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts.Manager.Keyboard.Board
{
    public class KeyboardBoard : MonoBehaviour
    {
        [SerializeField]
        private RawImage backgroundImage;

        private string language;
        public string Language { get { return language; } }

        private KeyboardKeyboardModel keyboardKeyboardModel;
        public KeyboardKeyboardModel KeyboardKeyboardModel { get { return keyboardKeyboardModel; } }

        public KeyboardBoard Setup(string language, KeyboardKeyboardModel keyboardKeyboardModel)
        {
            this.language = language;
            this.keyboardKeyboardModel = keyboardKeyboardModel;

            return this;
        }
    }
}
