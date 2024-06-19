using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System;

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
    public GameObject NoResultsMessage;

    public List<Product> allProducts = new List<Product>();
    private List<Product> filteredProducts = new List<Product>();

    private const string ShapePrefKey = "ShapeFilter";
    private const string MaterialPrefKey = "MaterialFilter";
    private const string ColorPrefKey = "ColorFilter";
    private const string TypePrefKey = "TypeFilter";
    private const string BrandPrefKey = "BrandFilter";
    private const string GenderPrefKey = "GenderFilter";

    private const string serverUrl = "https://thunderleafstudios.nl/brilpasser-backend/unity/getMonturen.php";

    private string localJsonFilePath;

    void Start()
    {
        applyFilterButton.onClick.AddListener(ApplyFilters);

#if UNITY_EDITOR
        localJsonFilePath = Path.Combine(Application.dataPath, "Resources/ProductData/monturen.json");
#elif UNITY_ANDROID
        localJsonFilePath = Path.Combine(Application.persistentDataPath, "monturen.json");
#endif

        StartCoroutine(GetProducts());
    }

    IEnumerator GetProducts()
    {
        // Check for internet connection
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(serverUrl))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogWarning("Failed to fetch data from server. Loading locally stored JSON data.");
                    LoadLocalJson();
                }
                else
                {
                    string jsonResponse = webRequest.downloadHandler.text;
                    ProductList productList = JsonUtility.FromJson<ProductList>(jsonResponse);

                    allProducts = productList.products;
                    SaveLocalJson(jsonResponse); // Save the JSON data to local file
                    PopulateDropdowns(); // Populate dropdowns with JSON data
                }
            }
        }
        else
        {
            // No internet connection, fallback to local JSON
            Debug.LogWarning("No internet connection. Loading locally stored JSON data.");
            LoadLocalJson();
        }
    }

    void LoadLocalJson()
    {
        if (File.Exists(localJsonFilePath))
        {
            try
            {
                string json = File.ReadAllText(localJsonFilePath);
                Debug.Log("Loaded JSON from local file: " + json);
                ProductList productList = JsonUtility.FromJson<ProductList>(json);
                allProducts = productList.products;
                PopulateDropdowns(); // Populate dropdowns with JSON data
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to parse local JSON file: " + localJsonFilePath);
                Debug.LogError("Error: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Local JSON file not found: " + localJsonFilePath);
        }
    }

    void SaveLocalJson(string jsonData)
    {
        Debug.Log(localJsonFilePath);
        File.WriteAllText(localJsonFilePath, jsonData);
        Debug.Log("Local JSON file updated.");
    }

    void ApplyFilters()
    {
        string selectedShape = shapeDropdown.options[shapeDropdown.value].text;
        string selectedMaterial = materialDropdown.options[materialDropdown.value].text;
        string selectedColor = colorDropdown.options[colorDropdown.value].text;
        string selectedType = typeDropdown.options[typeDropdown.value].text;
        string selectedBrand = brandDropdown.options[brandDropdown.value].text;
        string selectedGender = genderDropdown.options[genderDropdown.value].text;

        filteredProducts.Clear();

        foreach (var product in allProducts)
        {
            bool shapeMatch = selectedShape == "Alle" || product.Shape == selectedShape;
            bool materialMatch = selectedMaterial == "Alle" || product.Material == selectedMaterial;
            bool colorMatch = selectedColor == "Alle" || product.Color == selectedColor;
            bool typeMatch = selectedType == "Alle" || product.Type == selectedType;
            bool brandMatch = selectedBrand == "Alle" || product.Brand == selectedBrand;
            bool genderMatch = selectedGender == "Alle" || product.Gender == selectedGender || product.Gender == "Unisex";

            if (shapeMatch && materialMatch && colorMatch && typeMatch && brandMatch && genderMatch)
            {
                filteredProducts.Add(product);
            }
        }

        UpdateProductDisplay();

        if (filteredProducts.Count == 0)
        {
            ShowNoResultsMessage();
        }
        else
        {
            HideNoResultsMessage();
        }

        SaveFilters(selectedShape, selectedMaterial, selectedColor, selectedType, selectedBrand, selectedGender); // Save filter settings after applying filters
    }

    void ShowNoResultsMessage()
    {
        NoResultsMessage.SetActive(true);
    }

    void HideNoResultsMessage()
    {
        NoResultsMessage.SetActive(false);
    }

    void UpdateProductDisplay()
    {
        foreach (Transform child in productContainer)
        {
            if (child.gameObject != NoResultsMessage)
            {
                Destroy(child.gameObject);
            }
        }

        if (filteredProducts.Count == 0)
        {
            ShowNoResultsMessage();
        }
        else
        {
            // Instantiate product prefabs for each filtered product
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

    void SaveFilters(string shape, string material, string color, string type, string brand, string gender)
    {
        PlayerPrefs.SetString(ShapePrefKey, shape);
        PlayerPrefs.SetString(MaterialPrefKey, material);
        PlayerPrefs.SetString(ColorPrefKey, color);
        PlayerPrefs.SetString(TypePrefKey, type);
        PlayerPrefs.SetString(BrandPrefKey, brand);
        PlayerPrefs.SetString(GenderPrefKey, gender);
        PlayerPrefs.Save(); // Save PlayerPrefs after setting values
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

    void PopulateDropdowns()
    {
        PopulateDropdown(shapeDropdown, allProducts.Select(p => p.Shape).Distinct().ToList());
        PopulateDropdown(materialDropdown, allProducts.Select(p => p.Material).Distinct().ToList());
        PopulateDropdown(colorDropdown, allProducts.Select(p => p.Color).Distinct().ToList());
        PopulateDropdown(typeDropdown, allProducts.Select(p => p.Type).Distinct().ToList());
        PopulateDropdown(brandDropdown, allProducts.Select(p => p.Brand).Distinct().ToList());
        PopulateDropdown(genderDropdown, allProducts.Select(p => p.Gender).Distinct().ToList());
    }

    void PopulateDropdown(TMP_Dropdown dropdown, List<string> options)
    {
        if (dropdown != null)
        {
            dropdown.ClearOptions();
            options.Insert(0, "Alle");
            dropdown.AddOptions(options);
        }
        else
        {
            Debug.LogError("Dropdown reference is null.");
        }
    }

    [System.Serializable]
    public class ProductList
    {
        public List<Product> products;
    }
}
