using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.CallToAction
{
    public class CallToActionUI : MonoBehaviour
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
            infoText.rectTransform.anchoredPosition = new Vector3(0, 300, 0);
            bodyImage.rectTransform.anchoredPosition = new Vector3(0, 500, 0);

            infoText.rectTransform.DOKill();
            infoText.rectTransform.DOAnchorPosY(186, 0.5f);

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
