using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Project.Manager.Language;

namespace Project.UI.RandomInfo
{
    public class RandomInfoUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI infoText;
        [SerializeField]
        private Image bodyImage;
        [SerializeField]
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            SetProperties();
        }

        private void SetProperties()
        {
            infoText.rectTransform.anchoredPosition = new Vector3(0, 300, 0);
            bodyImage.rectTransform.anchoredPosition = new Vector3(0, 500, 0);
            canvasGroup.alpha = 0;
        }

        public void Show()
        {
            List<string> messages = LanguageManager.Instance.GetGroup("splash");

            infoText.text = messages[Random.Range(0, messages.Count - 1)];

            infoText.rectTransform.anchoredPosition = new Vector3(0, 300, 0);
            bodyImage.rectTransform.anchoredPosition = new Vector3(0, 500, 0);

            infoText.rectTransform.DOKill();
            infoText.rectTransform.DOAnchorPosY(-100, 0.5f);

            bodyImage.rectTransform.DOKill();
            bodyImage.rectTransform.DOAnchorPosY(0, 0.5f);

            canvasGroup.DOKill();
            canvasGroup.DOFade(1, 0.5f);
        }

        public void Hide()
        {
            infoText.rectTransform.DOKill();
            infoText.rectTransform.DOAnchorPosY(-300, 0.5f);

            bodyImage.rectTransform.DOKill();
            bodyImage.rectTransform.DOAnchorPosY(-300, 0.5f);

            canvasGroup.DOKill();
            canvasGroup.DOFade(0, 0.5f);
        }
    }
}
