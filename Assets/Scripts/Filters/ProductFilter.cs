using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductFilter : MonoBehaviour
{
    public TMP_Dropdown shapeDropdown;
    public TMP_Dropdown materialDropdown;
    public TMP_Dropdown colorDropdown;
    public TMP_Dropdown typeDropdown;
    public Button applyFilterButton;
    public GameObject productPrefab;
    public Transform productContainer;
    public GameObject productUIPrefab;
    public Transform uiContainer;

    public List<Product> allProducts = new List<Product>();
    private List<Product> filteredProducts = new List<Product>();

    void Start()
    {
        applyFilterButton.onClick.AddListener(ApplyFilters);
        InitializeProducts();
    }

    void InitializeProducts()
    {
        // Example initialization with correct casting
        allProducts.Add(new Product
        {
            Name = "KYO98E",
            Shape = "Rond",
            Material = "Metaal",
            Color = "Bruin",
            Type = "Bril",
            Price = 100,
            Image = Resources.Load<Sprite>("Sprites/Images/Brillen/KYO98E")
        });
        allProducts.Add(new Product
        {
            Name = "Product 2",
            Shape = "Vijfhoek",
            Material = "Metaal",
            Color = "Goud",
            Type = "Bril",
            Price = 50,
            Image = Resources.Load<Sprite>("Sprites/Images/Brillen/PRODUCT_2")
        });
        allProducts.Add(new Product
        {
            Name = "Rayban 24019/LE",
            Shape = "Ovaal",
            Material = "Kunststof",
            Color = "Blauw",
            Type = "Bril",
            Price = 150,
            Image = Resources.Load<Sprite>("Sprites/Images/Brillen/Rayban_24019LE")
        });
        allProducts.Add(new Product
        {
            Name = "2314Z10911",
            Shape = "Ovaal",
            Material = "Kunststof",
            Color = "Rood",
            Type = "Zonnebril",
            Price = 237,
            Image = Resources.Load<Sprite>("Sprites/Images/Brillen/2314Z10911")
        });
        allProducts.Add(new Product
        {
            Name = "Tom Tailor",
            Shape = "Rond",
            Material = "Metaal",
            Color = "Bruin",
            Type = "Bril",
            Price = 237,
            Image = Resources.Load<Sprite>("Sprites/Images/Brillen/Tom_Tailor")
        });
    }

    void ApplyFilters()
    {
        string selectedShape = shapeDropdown.options[shapeDropdown.value].text;
        string selectedMaterial = materialDropdown.options[materialDropdown.value].text;
        string selectedColor = colorDropdown.options[colorDropdown.value].text;
        string selectedType = typeDropdown.options[typeDropdown.value].text;

        filteredProducts.Clear();

        foreach (var product in allProducts)
        {
            bool shapeMatch = selectedShape == "All" || selectedShape == "GEEN KEUZE" || product.Shape == selectedShape;
            bool materialMatch = selectedMaterial == "All" || selectedMaterial == "GEEN KEUZE" || product.Material == selectedMaterial;
            bool colorMatch = selectedColor == "All" || selectedColor == "GEEN KEUZE" || product.Color == selectedColor;
            bool typeMatch = selectedType == "All" || selectedType == "GEEN KEUZE" || product.Type == selectedType;

            if (shapeMatch && materialMatch && colorMatch && typeMatch)
            {
                filteredProducts.Add(product);
            }
        }

        UpdateProductDisplay();
    }

    void UpdateProductDisplay()
    {
        foreach (Transform child in productContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var product in filteredProducts)
        {
            GameObject productGO = Instantiate(productPrefab, productContainer);

            ProductButton productButton = productGO.GetComponentInChildren<ProductButton>();

            if (productButton != null)
            {
                productButton.Setup(product, productUIPrefab, uiContainer);
            }
            else
            {
                Debug.LogError("ProductButton component not found on instantiated product prefab.");
            }
        }
    }
}
