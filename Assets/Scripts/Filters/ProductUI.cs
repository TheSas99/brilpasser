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
        continueButton.onClick.AddListener(StartEyeWearFitting);
    }

    public void Setup(Product product)
    {
        productNameText.text = product.Name;
        productShapeText.text = product.Shape;
        productMaterialText.text = product.Material;
        productPriceText.text = "$" + product.Price.ToString();
        productTypeText.text = product.Type;
        productColorText.text = product.Color;
        productImage.sprite = product.Image;
    }

    void ClosePanel()
    {
        Destroy(gameObject);
    }

    void StartEyeWearFitting()
    {
        // Get the selected product name
        string selectedProductName = productNameText.text;

        // Store the selected product name in PlayerPrefs
        PlayerPrefs.SetString("SelectedProductName", selectedProductName);

        // Load the next scene
        SceneManager.LoadScene("BrilChecker");
    }


}
