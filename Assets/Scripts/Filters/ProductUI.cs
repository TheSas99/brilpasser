using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProductUI : MonoBehaviour
{
    public TMP_Text productNameText;
    public TMP_Text productShapeText;
    public TMP_Text productMaterialText;
    public TMP_Text productPriceText;
    public TMP_Text productTypeText;
    public TMP_Text productColorText;
    public Image productImage;
    public Button closeButton;
    public Button continueButton;

    void Start()
    {
        closeButton.onClick.AddListener(ClosePanel);
    }

    public void Setup(Product product)
    {
        productNameText.text = product.Name;
        productShapeText.text = product.Shape;
        productMaterialText.text = product.Material;
        productPriceText.text = "€" + product.Price.ToString("F2");
        productTypeText.text = product.Type;
        productColorText.text = product.Color;
        productImage.sprite = product.Image;

        Debug.Log("IsPasbaar: " + product.IsPasbaar);

        // Check if the product is pasbaar
        if (product.IsPasbaar)
        {
            Debug.Log("Setting continueButton active");
            continueButton.gameObject.SetActive(true);
            continueButton.onClick.AddListener(StartEyeWearFitting);
        }
        else
        {
            Debug.Log("Setting continueButton inactive");
            continueButton.gameObject.SetActive(false);
        }
    }

    void ClosePanel()
    {
        Destroy(gameObject);
    }

    // Method to clear selected product names from PlayerPrefs
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

    void StartEyeWearFitting()
    {
        // Get the selected product name
        string selectedProductName = productNameText.text;

        // Store the selected product name in PlayerPrefs
        PlayerPrefs.SetString("SelectedProductName", selectedProductName);

        ClearSelectedProductNames();

        // Load the next scene
        SceneManager.LoadScene("BrilChecker");
    }
}
