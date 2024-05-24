using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductFilter : MonoBehaviour
{
    public TMP_Dropdown shapeDropdown;
    public TMP_Dropdown materialDropdown;
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
        allProducts.Add(new Product { Name = "Product 1", Shape = "Rond", Material = "Kunststof", Price = 100 });
        allProducts.Add(new Product { Name = "Product 2", Shape = "Vijfhoek", Material = "Kunststof", Price = 50 });
        // Add more products as needed
    }

    void ApplyFilters()
    {
        string selectedShape = shapeDropdown.options[shapeDropdown.value].text;
        string selectedMaterial = materialDropdown.options[materialDropdown.value].text;

        filteredProducts = allProducts;

        if (selectedShape != "All")
        {
            filteredProducts = filteredProducts.FindAll(product => product.Shape == selectedShape);
        }
        
        if (selectedMaterial != "All")
        {
            filteredProducts = filteredProducts.FindAll(product => product.Material == selectedMaterial);
        }

        if(selectedShape == "GEEN KEUZE" && selectedMaterial == "GEEN KEUZE")
        {
            filteredProducts = new List<Product>(allProducts);
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

