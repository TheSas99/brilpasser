using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductButton : MonoBehaviour
{
    public Button button;
    public TMP_Text productNameText;
    public Image productImage;
    private Product product;
    private GameObject productUIPrefab;
    private Transform uiContainer;

    public void Setup(Product product, GameObject productUIPrefab, Transform uiContainer)
    {
        this.product = product;
        this.productUIPrefab = productUIPrefab;
        this.uiContainer = uiContainer;

        productNameText.text = product.Name;
        productImage.sprite = product.Image;

        button.onClick.AddListener(OnButtonClick);
        Debug.Log("ProductButton Setup: " + product.Name);
    }

    void OnButtonClick()
    {
        Debug.Log("Button Clicked: " + product.Name);

        // Instantiate the ProductUI prefab
        GameObject productUIGO = Instantiate(productUIPrefab, uiContainer);

        // Get the ProductUI component from the instantiated GameObject
        ProductUI productUI = productUIGO.GetComponent<ProductUI>();

        // Set up the ProductUI with the product data
        if (productUI != null)
        {
            productUI.Setup(product);
        }
        else
        {
            Debug.LogError("ProductUI component not found on instantiated product UI prefab.");
        }
    }
}
