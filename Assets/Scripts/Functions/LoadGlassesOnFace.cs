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
    private SpriteRenderer glassesRenderer;
    private GameObject currentGlassesObject; // Track the instantiated glasses prefab

    void Start()
    {
        arFace = GetComponent<ARFace>();
        landmarks = GetComponent<ARFaceLandmarks>();

        if (glassesContainer == null)
        {
            Debug.LogError("GlassesContainer is not assigned.");
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

        UpdateGlassesSprite();
    }

    public void UpdateGlassesSprite()
    {
        string selectedGlasses = PlayerPrefs.GetString("SelectedProductName", "DefaultGlasses");
        Sprite glassesSprite = Resources.Load<Sprite>("Sprites/Images/Brillen/" + selectedGlasses);

        if (glassesSprite != null)
        {
            Debug.Log("Glasses sprite loaded successfully: " + selectedGlasses);

            // Destroy previous glasses object if exists
            if (currentGlassesObject != null)
            {
                Destroy(currentGlassesObject);
            }

            Vector3 leftEyePosition = landmarks.leftEye.position;
            Vector3 rightEyePosition = landmarks.rightEye.position;

            Vector3 glassesPosition = (leftEyePosition + rightEyePosition) / 2f;

            // Instantiate new glasses prefab
            currentGlassesObject = Instantiate(glassesPrefab, glassesPosition, Quaternion.identity, glassesContainer.transform);

            if (currentGlassesObject == null)
            {
                Debug.LogError("Failed to instantiate glasses prefab.");
                return;
            }

            currentGlassesObject.SetActive(true);

            SpriteRenderer spriteRenderer = currentGlassesObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = currentGlassesObject.AddComponent<SpriteRenderer>();
            }
            spriteRenderer.sprite = glassesSprite;

            float distanceBetweenEyes = Vector3.Distance(leftEyePosition, rightEyePosition);
            float spriteWidth = glassesSprite.rect.width / glassesSprite.pixelsPerUnit;

            float scaleFactor = (distanceBetweenEyes * glassesScaleFactor) / spriteWidth;

            currentGlassesObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            currentGlassesObject.transform.rotation = Quaternion.LookRotation(arFace.transform.forward, arFace.transform.up);

            currentGlassesObject.transform.position = glassesPosition - spriteRenderer.bounds.center * currentGlassesObject.transform.localScale.x;

            Debug.Log("Glasses instantiated and positioned successfully.");
        }
        else
        {
            Debug.LogError("Failed to load glasses sprite: " + selectedGlasses);
        }
    }
}
