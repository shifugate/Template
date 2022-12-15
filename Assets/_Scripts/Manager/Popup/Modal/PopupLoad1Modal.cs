using TMPro;
using UnityEngine;
using Project.Manager.Popup.Modal.Base;
using System;
using UnityEngine.UI;

namespace Project.Manager.Popup.Modal
{
    public class PopupLoad1Modal : PopupBaseAlphaModal
    {
        [SerializeField]
        private TextMeshProUGUI loadText;
        [SerializeField]
        private TextMeshProUGUI cancelText;
        [SerializeField]
        private Button cancelButton;

        private bool show;
        private Action cancelCallback;

        protected override void Awake()
        {
            base.Awake();

            if (!show)
                cancelButton.gameObject.SetActive(false);
        }

        public PopupLoad1Modal Setup(string message = null, string cancel = null, Action cancelCallback = null)
        {
            this.cancelCallback = cancelCallback;

            show = true;

            cancelText.text = cancel != null ? cancel : "";

            cancelButton.gameObject.SetActive(cancel != null);

            loadText.text = message != null ? message : "";

            return this;
        }

        public void CancealAction()
        {
            cancelCallback?.Invoke();

            Hide();
        }
    }
}
