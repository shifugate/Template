using Project.Manager.Route.Transition.Base;
using System;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Project.UI.Load;

namespace Project.Manager.Route.Transition
{
    public class RouterManagerFadeTransition : RouterManagerBaseTransition
    {
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
            RouteManager.Instance.ModelBase.CanvasGroup.DOKill();
            RouteManager.Instance.ModelBase.CanvasGroup.DOFade(0, 0.25f)
                .OnComplete(() => callback?.Invoke());

            /*group.DOKill();
            group.DOFade(1, 0.25f)
                .OnComplete(() => callback?.Invoke());*/
        }

        public override void AnimationOut(Action callback)
        {
            RouteManager.Instance.ModelBase.CanvasGroup.DOKill();
            RouteManager.Instance.ModelBase.CanvasGroup.DOFade(1, 0.25f)
               .OnComplete(() =>
               {
                   callback();

                   Destroy(gameObject);
               });

            /*group.DOKill();
            group.DOFade(0, 0.25f)
                .SetDelay(1)
                .OnComplete(() =>
                {
                    callback();

                    Destroy(gameObject);
                });*/
        }
    }
}
