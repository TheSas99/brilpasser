using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraButton : MonoBehaviour
{
    public void TakePicture()
    {
        //ScreenshotManager.ActualScreenshot(Screen.width, Screen.height);
        ScreenCapture.CaptureScreenshot("Screenshot.png");
    }
}
