using DG.Tweening;
using System;
using System.Collections.Generic;
using Project.Manager.Language.Token;
using Project.Manager.Popup;
using Project.Manager.Popup.Modal;

namespace Project.Util
{
    public class CommonUtil
    {
        public static Tweener SetValue(int currentValue, int newValue, float delay = 0, float time = 1, Action<int> valueCallback = null)
        {
            int value = currentValue;

            Tweener tweener = DOTween.To(() => value, result => value = result, newValue, time)
                .SetDelay(delay)
                .OnUpdate(() => valueCallback?.Invoke(value))
                .OnComplete(() => valueCallback?.Invoke(value));

            return tweener;
        }

        public static string GetTimeMMSS(double seconds)
        {
            if (seconds < 0)
                seconds = 0;

            TimeSpan time = TimeSpan.FromSeconds(seconds);

            return time.ToString("mm\\:ss");
        }

        public static void ShuffleList<T>(IList<T> list, int seed)
        {
            int index = list.Count;

            Random random = new Random(seed);

            while (index > 1)
            {
                index--;
                int k = random.Next(index + 1);
                T value = list[k];
                list[k] = list[index];
                list[index] = value;
            }
        }

        public static void ShowError(string error)
        {
            PopupManager.Instance.ShowModal<PopupMessage1Modal>()
                .Setup(LanguageManagerToken.common.error_token,
                    error,
                    LanguageManagerToken.common.close_token);
        }
    }
}
