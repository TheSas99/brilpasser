using UnityEngine;
using UnityEngine.UI;

public class ProductUI : MonoBehaviour
{
    public Text productNameText;
    public Text productShapeText;
    public Text productMaterialText;
    public Text productPriceText;

    public void Setup(Product product)
    {
        productNameText.text = product.Name;
        productShapeText.text = product.Shape;
        productMaterialText.text = product.Material;
        productPriceText.text = "$" + product.Price.ToString();
    }
}
