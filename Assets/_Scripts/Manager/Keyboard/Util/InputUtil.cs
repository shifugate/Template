using UnityEngine;
using UnityEngine.UI;

namespace UnloopLib.Keyboards.Util
{
    public class InputUtil
    {
        static public InputField CreateInputField(string name, Font font, Sprite background, GameObject father)
        {
            InputField inputField = CreateInputField(name, font, background);
            inputField.transform.SetParent(father.transform);

            RectTransform rectTransform = inputField.GetComponent<RectTransform>();
            rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
            rectTransform.localScale = new Vector3(1, 1, 1);

            return inputField;
        }

        static public InputField CreateInputField(string name, Font font, Sprite background)
        {
            GameObject inputFieldGo = new GameObject(name);

            RectTransform rectTransform = inputFieldGo.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(300, 60);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            inputFieldGo.AddComponent<CanvasRenderer>();

            Image image = inputFieldGo.AddComponent<Image>();
            image.sprite = background;
            image.type = Image.Type.Sliced;

            InputField inputField = inputFieldGo.AddComponent<InputField>();

            GameObject placeholderGo = new GameObject("Placeholder");
            placeholderGo.transform.SetParent(inputFieldGo.transform);

            rectTransform = placeholderGo.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.offsetMin = new Vector2(10, 7);
            rectTransform.offsetMax = new Vector2(-10, -6);

            placeholderGo.AddComponent<CanvasRenderer>();

            Text text = placeholderGo.AddComponent<Text>();
            text.font = font;
            text.color = new Color32(50, 50, 50, 128);
            text.fontStyle = FontStyle.Italic;
            text.text = "Enter text...";

            inputField.placeholder = text;

            GameObject textGo = new GameObject("Text");
            textGo.transform.SetParent(inputFieldGo.transform);

            rectTransform = textGo.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.offsetMin = new Vector2(10, 7);
            rectTransform.offsetMax = new Vector2(-10, -6);

            textGo.AddComponent<CanvasRenderer>();

            text = textGo.AddComponent<Text>();
            text.font = font;
            text.fontSize = 25;
            text.color = new Color32(50, 50, 50, 255);
            text.supportRichText = false;

            inputField.textComponent = text;

            return inputField;
        }
    }
}