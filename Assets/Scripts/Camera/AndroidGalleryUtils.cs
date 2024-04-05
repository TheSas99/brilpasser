using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Helper class for scanning the file to make it appear in the gallery
public static class AndroidGalleryUtils
{
    public static void ScanFile(string path)
    {
        AndroidJavaClass mediaScanner = new AndroidJavaClass("android.media.MediaScannerConnection");
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        mediaScanner.CallStatic("scanFile", currentActivity, new string[] { path }, null, null);
    }
}