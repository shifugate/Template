using UnityEngine;

namespace Project.MC.__Base
{
    public class ControllerBase<TModel> : MonoBehaviour
    {
        private TModel model;
        public TModel Model
        {
            get
            {
                if (model == null)
                    model = GetComponent<TModel>();

                return model;
            }
        }
    }
}
