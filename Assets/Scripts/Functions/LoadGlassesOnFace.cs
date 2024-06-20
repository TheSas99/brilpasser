using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARFace))]
public class LoadGlassesOnFace : MonoBehaviour
{
    public GameObject glassesPrefab; // Prefab for glasses object
    public GameObject glassesContainer; // Parent object to hold instantiated glasses
    public float glassesScaleFactor = 1.0f; // Scale factor to adjust glasses size

    private ARFace arFace; // Reference to the ARFace component attached to this GameObject
    private ARFaceLandmarks landmarks;

    void Start()
    {
        arFace = GetComponent<ARFace>(); // Get the ARFace component attached to this GameObject

        if (arFace == null)
        {
            Debug.LogError("ARFace component not found.");
            return;
        }

        landmarks = GetComponent<ARFaceLandmarks>(); // Get the ARFaceLandmarks component attached to this GameObject

        if (landmarks == null)
        {
            Debug.LogError("ARFaceLandmarks component not found.");
            return;
        }

        if (glassesContainer == null)
        {
            Debug.LogError("GlassesContainer is not assigned.");
            return;
        }

        if (glassesPrefab == null)
        {
            Debug.LogError("GlassesPrefab is not assigned.");
            return;
        }

        // Start a coroutine to wait for ARFace initialization
        StartCoroutine(WaitForFaceInitialization());
    }

    private IEnumerator WaitForFaceInitialization()
    {
        // Wait until leftEye and rightEye are available
        while (landmarks.leftEye == null || landmarks.rightEye == null)
        {
            Debug.Log("Waiting for AR face landmarks...");
            yield return null;
        }

        // Load the selected glasses sprite
        string selectedGlasses = PlayerPrefs.GetString("SelectedProductName", "DefaultGlasses");
        Sprite glassesSprite = Resources.Load<Sprite>("Sprites/Images/Brillen/" + selectedGlasses);

        if (glassesSprite != null)
        {
            Debug.Log("Glasses sprite loaded successfully: " + selectedGlasses);

            Vector3 leftEyePosition = landmarks.leftEye.position;
            Vector3 rightEyePosition = landmarks.rightEye.position;
            Vector3 glassesPosition = (leftEyePosition + rightEyePosition) / 2f;

            // Instantiate glasses prefab relative to the ARFace's position
            GameObject glassesObject = Instantiate(glassesPrefab, glassesPosition, Quaternion.identity, glassesContainer.transform);

            if (glassesObject == null)
            {
                Debug.LogError("Failed to instantiate glasses prefab.");
                yield break;
            }

            // Ensure the instantiated object is active
            glassesObject.SetActive(true);

            // Set the glasses sprite
            SpriteRenderer spriteRenderer = glassesObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = glassesObject.AddComponent<SpriteRenderer>();
            }
            spriteRenderer.sprite = glassesSprite;

            // Calculate the scale factor based on the distance between the eyes and the sprite's width
            float distanceX = Vector3.Distance(leftEyePosition, rightEyePosition);
            float spriteWidth = glassesSprite.rect.width / glassesSprite.pixelsPerUnit;
            float scaleFactor = distanceX * glassesScaleFactor / spriteWidth;

            // Apply scale factor uniformly
            glassesObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            // Align glasses with face orientation
            glassesObject.transform.rotation = Quaternion.LookRotation(arFace.transform.forward, arFace.transform.up);

            // Calculate offset to align glasses' center with the midpoint between the eyes
            Vector3 offset = (rightEyePosition - leftEyePosition) / 2f;
            glassesObject.transform.position = leftEyePosition + offset;

            Debug.Log("Glasses instantiated and positioned successfully.");
        }
        else
        {
            Debug.LogError("Failed to load glasses sprite: " + selectedGlasses);
            yield break;
        }
    }
}
