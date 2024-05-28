using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductButton : MonoBehaviour
{
    public Button button;
    public TMP_Text productNameText;
    public Image productImage;
    private Product product;
    private ProductUI productUI;

    public void Setup(Product product, ProductUI productUI)
    {
        this.product = product;
        this.productUI = productUI;

        productNameText.text = product.Name;
        productImage.sprite = product.Image;

        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        productUI.Setup(product);
    }
}
