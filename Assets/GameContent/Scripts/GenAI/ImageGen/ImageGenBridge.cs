using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace RPGWorldLLM.GenAI.ImageGen
{
    public class ImageGenBridge : MonoBehaviour
    {
        public static ImageGenBridge Instance { get; private set; }

        public string sdURL = "http://localhost:7861/sdapi/v1/txt2img";
        public string[] allModels = { "dreamshaper_8.safetensors" };
        public int selectedModelIndex = 0;

        public string SelectedModel => allModels[Mathf.Clamp(selectedModelIndex, 0, allModels.Length - 1)];

        [Header("Generation Settings")]
        public int width = 512;
        public int height = 512;
        public int steps = 20;
        public float cfgScale = 7f;
        public string negativePrompt = "";
        public string samplerName = "Euler";

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
        /// Sends a prompt to the Stable Diffusion A1111 API and returns a Texture2D via callback.
        /// Returns false if the bridge is busy.
        /// </summary>
        public bool Generate(string prompt, Action<Texture2D> onSuccess, Action<string> onError)
        {
            if (IsBusy)
            {
                onError?.Invoke("ImageGenBridge is busy.");
                return false;
            }
            StartCoroutine(GenerateCoroutine(prompt, onSuccess, onError));
            return true;
        }

        private IEnumerator GenerateCoroutine(string prompt, Action<Texture2D> onSuccess, Action<string> onError)
        {
            IsBusy = true;

            string requestBody = JsonUtility.ToJson(new SD_Txt2ImgRequest
            {
                prompt = prompt,
                negative_prompt = negativePrompt,
                width = width,
                height = height,
                steps = steps,
                cfg_scale = cfgScale,
                sampler_name = samplerName,
                override_settings = new SD_OverrideSettings
                {
                    sd_model_checkpoint = SelectedModel
                }
            });

            using var request = new UnityWebRequest(sdURL, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(requestBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke(request.error);
                IsBusy = false;
                yield break;
            }

            var response = JsonUtility.FromJson<SD_Txt2ImgResponse>(request.downloadHandler.text);

            if (response.images == null || response.images.Length == 0)
            {
                onError?.Invoke("No images in response.");
                IsBusy = false;
                yield break;
            }

            byte[] imageBytes;
            try
            {
                imageBytes = Convert.FromBase64String(response.images[0]);
            }
            catch (FormatException e)
            {
                onError?.Invoke("Failed to decode base64 image data: " + e.Message);
                IsBusy = false;
                yield break;
            }

            var texture = new Texture2D(2, 2);
            if (texture.LoadImage(imageBytes))
            {
                onSuccess?.Invoke(texture);
            }
            else
            {
                onError?.Invoke("Failed to load image from response data.");
            }

            IsBusy = false;
        }

        [Serializable]
        private class SD_Txt2ImgRequest
        {
            public string prompt;
            public string negative_prompt;
            public int width;
            public int height;
            public int steps;
            public float cfg_scale;
            public string sampler_name;
            public SD_OverrideSettings override_settings;
        }

        [Serializable]
        private class SD_OverrideSettings
        {
            public string sd_model_checkpoint;
        }

        [Serializable]
        private class SD_Txt2ImgResponse
        {
            public string[] images;
        }
    }
}
