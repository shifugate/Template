using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts.Manager.Keyboard.Util
{
    public class UnloopKeyboardMapInput : MonoBehaviour
    {
        public string type = "NORMAL";
        int index = -1;

        private void Awake()
        {
            InputField input = GetComponent<InputField>();

            if (input != null)
                KeyboardManager.Instance.AddInputField(input, type, index);
        }
    }
}
