using TMPro;
using UnityEngine;
using Project.Manager.Popup.Modal.Base;

namespace Project.Manager.Popup.Modal
{
    public class PopupLoad1Modal : PopupBaseAlphaModal
    {
        [SerializeField]
        private TextMeshProUGUI loadText;


        public PopupLoad1Modal Setup(string message = null)
        {
            loadText.text = message != null ? message : "";

            return this;
        }
    }
}
