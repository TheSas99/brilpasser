using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CameraButton : MonoBehaviour
{
    // Call this in the button.
    public void TakePhoto()
    {
        StartCoroutine(TakeAPhoto());
    }

    IEnumerator TakeAPhoto()
    {
        // Wait until rendering is complete, before taking the photo.
        yield return new WaitForEndOfFrame();

        Camera camera = Camera.main;
        int width = Screen.width;
        int height = Screen.height;
        // Create a new render texture the size of the screen.
        RenderTexture rt = new RenderTexture(width, height, 24);
        camera.targetTexture = rt;

        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        // Render the camera's view.
        camera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(width, height);
        image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        image.Apply();

        // Change back the camera target texture.
        camera.targetTexture = null;

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;

        // Save to an image file.
        // Encode the image texture into PNG.
        byte[] bytes = image.EncodeToPNG();

        // Save the image to the device's gallery
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(bytes, "MyGallery", DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png", (success, path) =>
        {
            if (success)
            {
                Debug.Log("Saved photo to gallery: " + path);
            }
            else
            {
                Debug.Log("Failed to save photo to gallery");
            }
        });

        // Check if the user has granted permission
        if (permission == NativeGallery.Permission.Granted)
        {
            Debug.Log("Permission granted");
        }
        else
        {
            Debug.Log("Permission denied");
        }

        // Free up memory.
        Destroy(rt);
        Destroy(image);
    }
}
