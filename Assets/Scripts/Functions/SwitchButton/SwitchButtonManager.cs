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
        foreach (Transform child in buttonsContainer)
        {
            Destroy(child.gameObject);
        }
        string selectedProductsString = PlayerPrefs.GetString("SelectedProductNames", string.Empty);
        string[] selectedProductNames = selectedProductsString.Split(',');

        if (selectedProductNames.Length > 1)
        {
            Vector2 buttonPosition = Vector2.zero;

            foreach (var productName in selectedProductNames)
            {
                GameObject switchButtonGO = Instantiate(switchButtonPrefab, buttonsContainer);
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

                buttonPosition += buttonSpacing;
            }
        }
    }

    void ClearSelectedProductNames()
    {
        PlayerPrefs.DeleteKey("SelectedProductNames");
        PlayerPrefs.Save();
        Debug.Log("Deleted SelectedProductNames from PlayerPrefs.");
    }

    void OnDisable()
    {
        ClearSelectedProductNames();
    }

    void OnDestroy()
    {
        ClearSelectedProductNames();
    }
}
