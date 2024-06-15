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
        allProducts = new List<Product>
        {
            new Product
            {
                Name = "KYO98E",
                Shape = "Rond",
                Material = "Metaal",
                Color = "Bruin",
                Brand = "Kansai",
                Gender = "Unisex",
                Type = "Bril",
                Price = 100,
                Image = LoadProductImage("KYO98E")
            },
            new Product
            {
                Name = "Product 2",
                Shape = "Vijfhoek",
                Material = "Metaal",
                Color = "Goud",
                Brand = "EPOS",
                Gender = "Unisex",
                Type = "Bril",
                Price = 50,
                Image = LoadProductImage("PRODUCT_2")
            },
            new Product
            {
                Name = "Rayban 24019/LE",
                Shape = "Ovaal",
                Material = "Kunststof",
                Color = "Blauw",
                Brand = "Rayban",
                Gender = "Unisex",
                Type = "Bril",
                Price = 150,
                Image = LoadProductImage("Rayban_24019LE")
            },
            new Product
            {
                Name = "2314Z10911",
                Shape = "Ovaal",
                Material = "Kunststof",
                Color = "Rood",
                Brand = "Rayban",
                Gender = "Unisex",
                Type = "Zonnebril",
                Price = 237,
                Image = LoadProductImage("2314Z10911")
            },
            new Product
            {
                Name = "Tom Tailor",
                Shape = "Rond",
                Material = "Metaal",
                Color = "Bruin",
                Brand = "Tom Tailor",
                Gender = "Unisex",
                Type = "Bril",
                Price = 237,
                Image = LoadProductImage("Tom_Tailor")
            },
            new Product
            {
                Name = "OfarKids MO0108B",
                Shape = "Ovaal",
                Material = "Kunststof",
                Color = "Paars",
                Brand = "Ofar",
                Gender = "Kinderen",
                Type = "Sportbril",
                Price = 65,
                Image = LoadProductImage("OfarKids_MO0108B")
            },
            new Product
            {
                Name = "Ofar Blue Glasses",
                Shape = "Ovaal",
                Material = "Metaal",
                Color = "Blauw",
                Brand = "Ofar",
                Gender = "Mannen",
                Type = "Sportbril",
                Price = 129,
                Image = LoadProductImage("Ofar_blue_glasses")
            },
            new Product
            {
                Name = "Puma Bril",
                Shape = "Ovaal",
                Material = "Kunststof",
                Color = "Zwart",
                Brand = "Puma",
                Gender = "Mannen",
                Type = "Bril",
                Price = 85,
                Image = LoadProductImage("Puma_Bril_Zwart")
            }
        };
    }

    Sprite LoadProductImage(string imageName)
    {
        return Resources.Load<Sprite>($"Sprites/Images/Brillen/{imageName}");
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
        filteredProducts = FilterProducts(selectedShape, selectedMaterial, selectedColor, selectedType, selectedBrand, selectedGender);
        UpdateProductDisplay();
    }

    List<Product> FilterProducts(string shape, string material, string color, string type, string brand, string gender)
    {
        List<Product> result = new List<Product>();

        foreach (var product in allProducts)
        {
            if (MatchesFilter(product.Shape, shape) &&
                MatchesFilter(product.Material, material) &&
                MatchesFilter(product.Color, color) &&
                MatchesFilter(product.Type, type) &&
                MatchesFilter(product.Brand, brand) &&
                MatchesGenderFilter(product.Gender, gender))
            {
                result.Add(product);
            }
        }

        return result;
    }

    bool MatchesFilter(string productAttribute, string selectedFilter)
    {
        return selectedFilter == "All" || selectedFilter == "GEEN KEUZE" || productAttribute == selectedFilter;
    }

    bool MatchesGenderFilter(string productGender, string selectedGender)
    {
        return selectedGender == "All" || selectedGender == "GEEN KEUZE" ||
               productGender == selectedGender ||
               productGender == "Unisex" && (selectedGender == "Mannen" || selectedGender == "Vrouwen" || selectedGender == "Unisex");
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
        SetDropdownValue(shapeDropdown, ShapePrefKey);
        SetDropdownValue(materialDropdown, MaterialPrefKey);
        SetDropdownValue(colorDropdown, ColorPrefKey);
        SetDropdownValue(typeDropdown, TypePrefKey);
        SetDropdownValue(brandDropdown, BrandPrefKey);
        SetDropdownValue(genderDropdown, GenderPrefKey);
    }

    void SetDropdownValue(TMP_Dropdown dropdown, string prefKey)
    {
        if (PlayerPrefs.HasKey(prefKey))
        {
            string savedValue = PlayerPrefs.GetString(prefKey);
            int index = dropdown.options.FindIndex(option => option.text == savedValue);
            if (index >= 0)
            {
                dropdown.value = index;
            }
        }
    }
}
