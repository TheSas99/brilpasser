using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotManager : MonoBehaviour
{
    private static ScreenshotManager instance;

    private Camera usedCameraView;
    private bool takeScreenshotOnNextFrame;

    private void Awake()
    {
        instance = this;
        usedCameraView = gameObject.GetComponent<Camera>();
    }

    private void OnPostRender()
    {
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = usedCameraView.targetTexture;
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/cameraScreenshot.png", byteArray);
            Debug.Log("Saved SS");

            RenderTexture.ReleaseTemporary(renderTexture);
            usedCameraView.targetTexture = null;
        }
    }

    private void TakeScreenshot(int width, int height)
    {
        usedCameraView.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
    }

    public static void ActualScreenshot(int width, int height)
    {
        instance.TakeScreenshot(width, height);
    }
}
