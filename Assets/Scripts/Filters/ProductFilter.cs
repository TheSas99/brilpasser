using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductFilter : MonoBehaviour
{
    public TMP_Dropdown shapeDropdown;
    public TMP_Dropdown materialDropdown;
    public TMP_Dropdown priceDropdown;
    public Button applyFilterButton;
    public Button resetFilterButton;
    public GameObject productPrefab; // A prefab for displaying products
    public Transform productContainer; // Parent object for instantiated products

    private List<Product> allProducts = new List<Product>();
    private List<Product> filteredProducts = new List<Product>();

    void Start()
    {
        applyFilterButton.onClick.AddListener(ApplyFilters);
        resetFilterButton.onClick.AddListener(ResetFilters);

        // Load or initialize your products
        InitializeProducts();
    }

    void InitializeProducts()
    {
        // Example initialization
        allProducts.Add(new Product { Name = "Product 1", Shape = "Rond", Material = "Kunststof", Price = 100 });
        allProducts.Add(new Product { Name = "Product 2", Shape = "Hoekig", Material = "Kunststof", Price = 50 });
        // Add more products as needed
    }

    void ApplyFilters()
    {
        string selectedShape = shapeDropdown.options[shapeDropdown.value].text;
        string selectedMaterial = materialDropdown.options[materialDropdown.value].text;
        string selectedPriceRange = priceDropdown.options[priceDropdown.value].text;

        filteredProducts = allProducts;

        if (selectedShape != "All")
        {
            filteredProducts = filteredProducts.FindAll(product => product.Shape == selectedShape);
        }

        if (selectedPriceRange != "All")
        {
            float minPrice = 0, maxPrice = float.MaxValue;
            // Define your price ranges and parse accordingly
            if (selectedPriceRange == "$0 - $50") { minPrice = 0; maxPrice = 50; }
            else if (selectedPriceRange == "$50 - $100") { minPrice = 50; maxPrice = 100; }
            // Add more ranges as needed

            filteredProducts = filteredProducts.FindAll(product => product.Price >= minPrice && product.Price <= maxPrice);
        }

        UpdateProductDisplay();
    }

    void ResetFilters()
    {
        shapeDropdown.value = 0;
        materialDropdown.value = 0;
        priceDropdown.value = 0;
        filteredProducts = new List<Product>(allProducts);
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

