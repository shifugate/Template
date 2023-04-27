using Project.Manager.Route.Transition.Base;
using System;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Project.UI.Load;

namespace Project.Manager.Route.Transition
{
    public class RouterManagerFadeTestTransition : RouterManagerBaseTransition
    {
        [SerializeField]
        private RectTransform contentHolder;

        private TextMeshProUGUI progressionText;
        private Load01UI loadUI;
        private CanvasGroup group;
        private float time;
        private float progress;

        protected override void Awake()
        {
            base.Awake();

            progressionText = GetComponentInChildren<TextMeshProUGUI>();
            loadUI = GetComponentInChildren<Load01UI>();
            group = GetComponent<CanvasGroup>();
            group.alpha = 0;
            contentHolder.anchoredPosition = new Vector2(0, Screen.height);

            loadUI.gameObject.SetActive(false);
        }

        private void Update()
        {
            time += Time.deltaTime;

            if (time > 2)
            {
                loadUI.gameObject.SetActive(true);

                progress = Mathf.Lerp(progress, progression, Time.deltaTime);

                progressionText.text = progress == 0
                    ? ""
                    : $"{Mathf.RoundToInt(progress * 100)}%";
            }
        }

        public override void Initialize()
        {
            progressionText.text = "";
            loadUI.gameObject.SetActive(false);
        }

        public override void AnimationIn(Action callback)
        {
            group.DOKill();
            group.DOFade(1, 1)
                .OnComplete(() => callback?.Invoke());

            contentHolder.DOKill();
            contentHolder.DOAnchorPosY(0, 1);
        }

        public override void AnimationOut(Action callback)
        {
            group.DOKill();
            group.DOFade(0, 1)
                .SetDelay(1)
                .OnComplete(() =>
                {
                    callback();

                    Destroy(gameObject);
                });

            contentHolder.DOKill();
            contentHolder.DOAnchorPosY(-Screen.height, 1);
        }
    }
}
