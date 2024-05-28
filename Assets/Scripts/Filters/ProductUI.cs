using TMPro;
using UnityEngine;
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

    public void Setup(Product product)
    {
        productNameText.text = product.Name;
        productShapeText.text = product.Shape;
        productMaterialText.text = product.Material;
        productPriceText.text = "$" + product.Price.ToString();
        productTypeText.text = product.Type;
        productColorText.text = product.Color;
    }
}
