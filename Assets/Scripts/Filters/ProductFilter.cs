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
    public TMP_Dropdown brandDropdown;
    public TMP_Dropdown genderDropdown;
    public Button applyFilterButton;
    public GameObject productPrefab;
    public Transform productContainer;
    public GameObject productUIPrefab;
    public Transform uiContainer;

    public List<Product> allProducts = new List<Product>();
    private List<Product> filteredProducts = new List<Product>();

    private const string ShapePrefKey = "ShapeFilter";
    private const string MaterialPrefKey = "MaterialFilter";
    private const string ColorPrefKey = "ColorFilter";
    private const string TypePrefKey = "TypeFilter";
    private const string BrandPrefKey = "BrandFilter";
    private const string GenderPrefKey = "GenderFilter";

    void Start()
    {
        applyFilterButton.onClick.AddListener(ApplyFilters);
        InitializeProducts();
        LoadFilters();
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
            Brand = "Kansai",
            Gender = "Unisex",
            Type = "Bril",
            Price = 100,
            Image = Resources.Load<Sprite>("Sprites/Images/Brillen/KYO98E/KYO98E")
        });
        allProducts.Add(new Product
        {
            Name = "Product 2",
            Shape = "Vijfhoek",
            Material = "Metaal",
            Color = "Goud",
            Brand = "EPOS",
            Gender = "Unisex",
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
            Brand = "Rayban",
            Gender = "Unisex",
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
            Brand = "Rayban",
            Gender = "Unisex",
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
            Brand = "Tom Tailor",
            Gender = "Unisex",
            Type = "Bril",
            Price = 237,
            Image = Resources.Load<Sprite>("Sprites/Images/Brillen/Tom_Tailor")
        });
        allProducts.Add(new Product
        {
            Name = "OfarKids MO0108B",
            Shape = "Ovaal",
            Material = "Kunststof",
            Brand = "Ofar",
            Gender = "Kinderen",
            Color = "Paars",
            Type = "Sportbril",
            Price = 65,
            Image = Resources.Load<Sprite>("Sprites/Images/Brillen/OfarKids_MO0108B")
        });
        allProducts.Add(new Product
        {
            Name = "Ofar Blue Glasses",
            Shape = "Ovaal",
            Material = "Metaal",
            Color = "Blauw",
            Brand = "Ofar",
            Gender = "Mannen",
            Type = "Sportbril",
            Price = 129,
            Image = Resources.Load<Sprite>("Sprites/Images/Brillen/Ofar_blue_glasses")
        });
        allProducts.Add(new Product
        {
            Name = "Puma Bril",
            Shape = "Ovaal",
            Material = "Kunststof",
            Color = "Zwart",
            Brand = "Puma",
            Gender = "Mannen",
            Type = "Bril",
            Price = 85,
            Image = Resources.Load<Sprite>("Sprites/Images/Brillen/Puma_Bril_Zwart")
        });
    }

    void ApplyFilters()
    {
        string selectedShape = shapeDropdown.options[shapeDropdown.value].text;
        string selectedMaterial = materialDropdown.options[materialDropdown.value].text;
        string selectedColor = colorDropdown.options[colorDropdown.value].text;
        string selectedType = typeDropdown.options[typeDropdown.value].text;
        string selectedBrand = brandDropdown.options[brandDropdown.value].text;
        string selectedGender = genderDropdown.options[genderDropdown.value].text;

        SaveFilters(selectedShape, selectedMaterial, selectedColor, selectedType, selectedBrand, selectedGender);

        filteredProducts.Clear();

        foreach (var product in allProducts)
        {
            bool shapeMatch = selectedShape == "All" || selectedShape == "GEEN KEUZE" || product.Shape == selectedShape;
            bool materialMatch = selectedMaterial == "All" || selectedMaterial == "GEEN KEUZE" || product.Material == selectedMaterial;
            bool colorMatch = selectedColor == "All" || selectedColor == "GEEN KEUZE" || product.Color == selectedColor;
            bool typeMatch = selectedType == "All" || selectedType == "GEEN KEUZE" || product.Type == selectedType;
            bool brandMatch = selectedBrand == "All" || selectedBrand == "GEEN KEUZE" || product.Brand == selectedBrand;

            // Adjust gender match logic
            bool genderMatch = selectedGender == "All" || selectedGender == "GEEN KEUZE" ||
                               product.Gender == selectedGender || product.Gender == "Unisex" &&
                               (selectedGender == "Mannen" || selectedGender == "Vrouwen" || selectedGender == "Unisex");

            if (shapeMatch && materialMatch && colorMatch && typeMatch && brandMatch && genderMatch)
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

    void SaveFilters(string shape, string material, string color, string type, string brand, string gender)
    {
        PlayerPrefs.SetString(ShapePrefKey, shape);
        PlayerPrefs.SetString(MaterialPrefKey, material);
        PlayerPrefs.SetString(ColorPrefKey, color);
        PlayerPrefs.SetString(TypePrefKey, type);
        PlayerPrefs.SetString(BrandPrefKey, brand);
        PlayerPrefs.SetString(GenderPrefKey, gender);
    }

    void LoadFilters()
    {
        if (PlayerPrefs.HasKey(ShapePrefKey)) shapeDropdown.value = shapeDropdown.options.FindIndex(option => option.text == PlayerPrefs.GetString(ShapePrefKey));
        if (PlayerPrefs.HasKey(MaterialPrefKey)) materialDropdown.value = materialDropdown.options.FindIndex(option => option.text == PlayerPrefs.GetString(MaterialPrefKey));
        if (PlayerPrefs.HasKey(ColorPrefKey)) colorDropdown.value = colorDropdown.options.FindIndex(option => option.text == PlayerPrefs.GetString(ColorPrefKey));
        if (PlayerPrefs.HasKey(TypePrefKey)) typeDropdown.value = typeDropdown.options.FindIndex(option => option.text == PlayerPrefs.GetString(TypePrefKey));
        if (PlayerPrefs.HasKey(BrandPrefKey)) brandDropdown.value = brandDropdown.options.FindIndex(option => option.text == PlayerPrefs.GetString(BrandPrefKey));
        if (PlayerPrefs.HasKey(GenderPrefKey)) genderDropdown.value = genderDropdown.options.FindIndex(option => option.text == PlayerPrefs.GetString(GenderPrefKey));
    }
}
