using UnityEngine;

public class Brilpasser : MonoBehaviour
{
    public Transform parent;

    void Start()
    {
        // Load the selected child index from PlayerPrefs
        int selectedIndex = PlayerPrefs.GetInt("SelectedButton", -1);

        // If a valid index is found, activate the corresponding child
        if (selectedIndex >= 0 && selectedIndex < parent.childCount)
        {
            // Deactivate all children first
            DeactivateAllChildren();

            // Activate the selected child
            parent.GetChild(selectedIndex).gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Invalid selected index or no index found in PlayerPrefs.");
        }
    }

    // Deactivate all children of the parent GameObject
    void DeactivateAllChildren()
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
        }
    }
}
