using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ProductSwitchManager : MonoBehaviour
{
    public GameObject switchButtonPrefab;
    public Transform buttonsContainer;
    public Vector2 buttonSpacing = new Vector2(0f, -10f); // Spacing between buttons (adjust as needed)

    void Start()
    {
        // Clear existing buttons
        foreach (Transform child in buttonsContainer)
        {
            Destroy(child.gameObject);
        }

        // Retrieve selected product names from PlayerPrefs
        string selectedProductsString = PlayerPrefs.GetString("SelectedProductNames", string.Empty);
        string[] selectedProductNames = selectedProductsString.Split(',');

        // Check if there are multiple selected products
        if (selectedProductNames.Length > 1)
        {
            Vector2 buttonPosition = Vector2.zero; // Initial position for the first button

            // Instantiate a button for each selected product name
            foreach (var productName in selectedProductNames)
            {
                GameObject switchButtonGO = Instantiate(switchButtonPrefab, buttonsContainer);

                // Set parent to buttonsContainer and reset local position and scale
                switchButtonGO.transform.SetParent(buttonsContainer);
                switchButtonGO.transform.localPosition = buttonPosition;
                switchButtonGO.transform.localScale = Vector3.one;

                SwitchButton switchButton = switchButtonGO.GetComponent<SwitchButton>();

                if (switchButton != null)
                {
                    switchButton.Setup(productName);
                }
                else
                {
                    Debug.LogError("SwitchButton component not found on prefab.");
                }

                // Update button position for the next button
                buttonPosition += buttonSpacing;
            }
        }
        // No need to instantiate buttons if only one or no products are selected
    }
}
