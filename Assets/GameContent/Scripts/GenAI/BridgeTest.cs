using UnityEngine;
using RPGWorldLLM.GenAI.TextGen;
using RPGWorldLLM.GenAI.ImageGen;

namespace RPGWorldLLM.GenAI
{
    public class BridgeTest : MonoBehaviour
    {
        [Header("Text Generation Test")]
        public string textPrompt = "Describe a medieval tavern in two sentences.";

        [Header("Image Generation Test")]
        public string imagePrompt = "A medieval tavern at night, fantasy art";

        [Header("Run on Start")]
        public bool testTextOnStart = true;
        public bool testImageOnStart = true;

        private SpriteRenderer spriteRenderer;

        void Start()
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

            if (testTextOnStart)
                RunTextTest();
            if (testImageOnStart)
                RunImageTest();
        }

        public void RunTextTest()
        {
            if (TextGenBridge.Instance == null)
            {
                Debug.LogError("[BridgeTest] TextGenBridge instance not found.");
                return;
            }

            Debug.Log("[BridgeTest] Sending text prompt: " + textPrompt);

            bool accepted = TextGenBridge.Instance.Generate(
                textPrompt,
                onSuccess: response =>
                {
                    Debug.Log("[BridgeTest] Text response received:\n" + response);

                    // Test busy rejection by calling again immediately after completion
                    // (should succeed since we just finished)
                    TestTextBusyRejection();
                },
                onError: error =>
                {
                    Debug.LogError("[BridgeTest] Text error: " + error);
                }
            );

            if (!accepted)
            {
                Debug.LogWarning("[BridgeTest] TextGenBridge rejected request (busy).");
                return;
            }

            // Test busy rejection while the first request is in flight
            bool secondAccepted = TextGenBridge.Instance.Generate(
                "This should be rejected.",
                onSuccess: _ => Debug.LogError("[BridgeTest] FAIL: Second text request should not succeed while busy."),
                onError: error => Debug.Log("[BridgeTest] PASS: Busy rejection works - " + error)
            );

            if (!secondAccepted)
                Debug.Log("[BridgeTest] PASS: TextGenBridge.Generate returned false while busy.");
        }

        private void TestTextBusyRejection()
        {
            Debug.Log("[BridgeTest] TextGenBridge.IsBusy after completion: " + TextGenBridge.Instance.IsBusy);
        }

        public void RunImageTest()
        {
            if (ImageGenBridge.Instance == null)
            {
                Debug.LogError("[BridgeTest] ImageGenBridge instance not found.");
                return;
            }

            Debug.Log("[BridgeTest] Sending image prompt: " + imagePrompt);

            bool accepted = ImageGenBridge.Instance.Generate(
                imagePrompt,
                onSuccess: texture =>
                {
                    Debug.Log("[BridgeTest] Image received: " + texture.width + "x" + texture.height);
                    Sprite sprite = Sprite.Create(
                        texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f),
                        100f
                    );
                    spriteRenderer.sprite = sprite;
                },
                onError: error =>
                {
                    Debug.LogError("[BridgeTest] Image error: " + error);
                }
            );

            if (!accepted)
            {
                Debug.LogWarning("[BridgeTest] ImageGenBridge rejected request (busy).");
                return;
            }

            // Test busy rejection while the first request is in flight
            bool secondAccepted = ImageGenBridge.Instance.Generate(
                "This should be rejected.",
                onSuccess: _ => Debug.LogError("[BridgeTest] FAIL: Second image request should not succeed while busy."),
                onError: error => Debug.Log("[BridgeTest] PASS: Busy rejection works - " + error)
            );

            if (!secondAccepted)
                Debug.Log("[BridgeTest] PASS: ImageGenBridge.Generate returned false while busy.");
        }
    }
}
