using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace RPGWorldLLM.GenAI.TextGen
{
    public class TextGenBridge : MonoBehaviour
    {
        public static TextGenBridge Instance { get; private set; }

        public string ollamaURL = "http://localhost:11434/api/generate";
        public string[] allModels = { "dolphin-mixtral-gpu", "mistral-gpu", "mistral-instruct-gpu" };
        public int selectedModelIndex = 0;

        public string SelectedModel => allModels[Mathf.Clamp(selectedModelIndex, 0, allModels.Length - 1)];

        public bool IsBusy { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Sends a prompt to the LLM and returns the response via callback.
        /// Returns false if the bridge is busy.
        /// </summary>
        public bool Generate(string prompt, Action<string> onSuccess, Action<string> onError)
        {
            if (IsBusy)
            {
                onError?.Invoke("TextGenBridge is busy.");
                return false;
            }
            StartCoroutine(GenerateCoroutine(prompt, onSuccess, onError));
            return true;
        }

        private IEnumerator GenerateCoroutine(string prompt, Action<string> onSuccess, Action<string> onError)
        {
            IsBusy = true;

            string requestBody = JsonUtility.ToJson(new OllamaTextRequest
            {
                model = SelectedModel,
                prompt = prompt,
                stream = false
            });

            using var request = new UnityWebRequest(ollamaURL, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(requestBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke(request.error);
            }
            else
            {
                var response = JsonUtility.FromJson<OllamaTextResponse>(request.downloadHandler.text);
                onSuccess?.Invoke(response.response);
            }

            IsBusy = false;
        }

        [Serializable]
        private class OllamaTextRequest
        {
            public string model;
            public string prompt;
            public bool stream;
        }

        [Serializable]
        private class OllamaTextResponse
        {
            public string response;
        }
    }
}
