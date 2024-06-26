using UnityEngine;

using TMPro;

public class Brilpasser : MonoBehaviour
{
    public Transform parent;

    void Start()
    {
        // Retrieve the selected product name from PlayerPrefs
        string selectedProductName = PlayerPrefs.GetString("SelectedProductName", "No Product Selected");

        DeactivateAllChildren();

        // Iterate through the children of the parent GameObject
        foreach (Transform child in parent)
        {
            // Check if the name of the child matches the selected product name
            if (child.name == selectedProductName && selectedProductName != "No Product Selected")
            {
                Debug.Log("Found matching product: " + selectedProductName);
                child.gameObject.SetActive(true);
            }
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
