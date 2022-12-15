using TMPro;
using UnityEngine;

namespace Project.UI.Subtitle
{
    public class SubtitleUI : MonoBehaviour
    {

        [SerializeField]
        protected RectTransform subtitleHolder;
        [SerializeField]
        protected TextMeshProUGUI subtitleText;

        public virtual void SetText(string text)
        {
            subtitleText.text = !string.IsNullOrEmpty(text) ? text : "";
            subtitleHolder.gameObject.SetActive(!string.IsNullOrEmpty(text));
        }
    }
}
