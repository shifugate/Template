using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Project.Util
{
    public static class ScreenUtil
    {
        public static bool PointerOverUIName()
        {
            GameObject go = PointerOverUIElement(GetEventSystemRaycastResults());

            return go != null;
        }

        private static GameObject PointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
        {
            if (eventSystemRaysastResults.Count > 0)
                return eventSystemRaysastResults[0].gameObject;

            return null;
        }

        private static List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }
    }
}
