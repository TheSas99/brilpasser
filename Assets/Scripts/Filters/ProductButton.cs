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

        // Add a log to debug image loading
        Debug.Log("Loading image for product: " + product.Name);
        Sprite loadedSprite = product.Image;
        if (loadedSprite != null)
        {
            productImage.sprite = loadedSprite;
        }
        else
        {
            Debug.LogError("Failed to load image for product: " + product.Name);
        }

        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        GameObject productUIGO = Instantiate(productUIPrefab);
        productUIGO.transform.SetParent(uiContainer, false);
        ProductUI productUI = productUIGO.GetComponent<ProductUI>();

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