using UnityEngine;

namespace Project.MVC.__Base
{
    public class ModelBase : MonoBehaviour
    {
        public object[] args;

        private CanvasGroup canvasGroup;
        public CanvasGroup CanvasGroup { get { return canvasGroup; } }

        protected void Awake()
        {
            canvasGroup = gameObject.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0;
        }
    }
}
