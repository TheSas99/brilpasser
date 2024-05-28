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
    public GameObject productPrefab; // A prefab for displaying products
    public GameObject productUIPrefab; // Reference to the ProductUI prefab
    public Transform productContainer; // Parent object for instantiated products
    public Transform uiContainer; // Parent object for instantiated product UI panels

    private List<Product> allProducts = new List<Product>();
    private List<Product> filteredProducts = new List<Product>();

    void Start()
    {
        applyFilterButton.onClick.AddListener(ApplyFilters);

        // Load or initialize your products
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
            Image = Resources.Load<Sprite>("Images/KYO98E") // Correctly load and cast the sprite
        });
        allProducts.Add(new Product
        {
            Name = "Product 2",
            Shape = "Vijfhoek",
            Material = "Metaal",
            Color = "Goud",
            Type = "Bril",
            Price = 50,
            Image = Resources.Load<Sprite>("Images/Product2") // Correctly load and cast the sprite
        });
        // Add more products as needed
    }

    void ApplyFilters()
    {
        string selectedShape = shapeDropdown.options[shapeDropdown.value].text;
        string selectedMaterial = materialDropdown.options[materialDropdown.value].text;
        string selectedColor = colorDropdown.options[colorDropdown.value].text;
        string selectedType = typeDropdown.options[typeDropdown.value].text;

        filteredProducts = allProducts;

        if (selectedShape != "All" && selectedShape != "GEEN KEUZE")
        {
            filteredProducts = filteredProducts.FindAll(product => product.Shape == selectedShape);
        }

        if (selectedMaterial != "All" && selectedMaterial != "GEEN KEUZE")
        {
            filteredProducts = filteredProducts.FindAll(product => product.Material == selectedMaterial);
        }

        if (selectedColor != "All" && selectedColor != "GEEN KEUZE")
        {
            filteredProducts = filteredProducts.FindAll(product => product.Color == selectedColor);
        }

        if (selectedType != "All" && selectedType != "GEEN KEUZE")
        {
            filteredProducts = filteredProducts.FindAll(product => product.Type == selectedType);
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

            // Get the ProductButton component from the child GameObject
            ProductButton productButton = productGO.GetComponentInChildren<ProductButton>();

            // Check if productButton is null before accessing its members
            if (productButton != null)
            {
                productButton.Setup(product, productUIPrefab, uiContainer);
                Debug.Log("Product instantiated: " + product.Name);
            }
            else
            {
                Debug.LogError("ProductButton component not found on instantiated product prefab.");
            }
        }
    }
}
