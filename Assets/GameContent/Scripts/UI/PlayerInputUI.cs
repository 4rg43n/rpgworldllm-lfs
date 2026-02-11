using RPGWorld.Chat;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace RPGWorld.UI
{
    public class PlayerInputUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button submitButton;

        private void Awake()
        {
            if (inputField == null)
                inputField = GetComponentInChildren<TMP_InputField>();
            if (submitButton == null)
                submitButton = GetComponentInChildren<Button>();

            WireInputFieldReferences();
        }

        private void WireInputFieldReferences()
        {
            if (inputField == null || inputField.textComponent != null)
                return;

            Transform textArea = inputField.transform.Find("Text Area");
            if (textArea == null)
                return;

            inputField.textViewport = textArea.GetComponent<RectTransform>();

            Transform textTransform = textArea.Find("Text");
            if (textTransform != null)
                inputField.textComponent = textTransform.GetComponent<TMP_Text>();

            Transform placeholderTransform = textArea.Find("Placeholder");
            if (placeholderTransform != null)
                inputField.placeholder = placeholderTransform.GetComponent<Graphic>();
        }


        private void OnEnable()
        {
            submitButton.onClick.AddListener(OnSubmit);
        }

        private void OnDisable()
        {
            submitButton.onClick.RemoveListener(OnSubmit);
        }

        private void OnSubmit()
        {
            string str = inputField.text;
            if (!str.IsUnityNull() && str.Length > 0)
            {
                ProcessInput(str);
            }
        }

        private void ProcessInput(string input_str)
        {
            ChatHistory.Instance.AddText("test", input_str);
            Debug.Log(input_str);
        }
    }
}
