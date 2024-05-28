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
    public Transform productContainer; // Parent object for instantiated products

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
        // Example initialization
        allProducts.Add(new Product { Name = "KYO98E", Shape = "Rond", Material = "Metaal", Color = "Bruin", Type = "Bril", Price = 100 });
        allProducts.Add(new Product { Name = "Product 2", Shape = "Vijfhoek", Material = "Metaal", Color = "Goud", Type = "Bril", Price = 50 });
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

        if(selectedColor != "All" && selectedColor != "GEEN KEUZE")
        {
            filteredProducts = filteredProducts.FindAll(product => product.Color == selectedColor);
        }

        if(selectedType != "All" && selectedType != "GEEN KEUZE")
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
            // Assume productPrefab has a script to update its UI based on product data
            productGO.GetComponent<ProductUI>().Setup(product);
        }
    }
}
