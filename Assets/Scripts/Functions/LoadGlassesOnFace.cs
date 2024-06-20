using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARFace))]
public class LoadGlassesOnFace : MonoBehaviour
{
    public GameObject glassesPrefab; 
    public GameObject glassesContainer; 
    public float glassesScaleFactor = 1.0f; 

    private ARFace arFace; 
    private ARFaceLandmarks landmarks;

    void Start()
    {
        arFace = GetComponent<ARFace>(); 

        if (arFace == null)
        {
            Debug.LogError("ARFace component not found.");
            return;
        }

        landmarks = GetComponent<ARFaceLandmarks>(); 

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

        StartCoroutine(WaitForFaceInitialization());
    }

    private IEnumerator WaitForFaceInitialization()
    {
        while (landmarks.leftEye == null || landmarks.rightEye == null)
        {
            Debug.Log("Waiting for AR face landmarks...");
            yield return null;
        }

        string selectedGlasses = PlayerPrefs.GetString("SelectedProductName", "DefaultGlasses");
        Sprite glassesSprite = Resources.Load<Sprite>("Sprites/Images/Brillen/" + selectedGlasses);

        if (glassesSprite != null)
        {
            Debug.Log("Glasses sprite loaded successfully: " + selectedGlasses);

            Vector3 leftEyePosition = landmarks.leftEye.position;
            Vector3 rightEyePosition = landmarks.rightEye.position;

            Vector3 glassesPosition = (leftEyePosition + rightEyePosition) / 2f;

            GameObject glassesObject = Instantiate(glassesPrefab, glassesPosition, Quaternion.identity, glassesContainer.transform);

            if (glassesObject == null)
            {
                Debug.LogError("Failed to instantiate glasses prefab.");
                yield break;
            }

            glassesObject.SetActive(true);

            SpriteRenderer spriteRenderer = glassesObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = glassesObject.AddComponent<SpriteRenderer>();
            }
            spriteRenderer.sprite = glassesSprite;

            float distanceBetweenEyes = Vector3.Distance(leftEyePosition, rightEyePosition);
            float spriteWidth = glassesSprite.rect.width / glassesSprite.pixelsPerUnit;

            float scaleFactor = (distanceBetweenEyes * glassesScaleFactor) / spriteWidth;

            glassesObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            glassesObject.transform.rotation = Quaternion.LookRotation(arFace.transform.forward, arFace.transform.up);

            glassesObject.transform.position = glassesPosition - spriteRenderer.bounds.center * glassesObject.transform.localScale.x;

            Debug.Log("Glasses instantiated and positioned successfully.");
        }
        else
        {
            Debug.LogError("Failed to load glasses sprite: " + selectedGlasses);
            yield break;
        }
    }
}
