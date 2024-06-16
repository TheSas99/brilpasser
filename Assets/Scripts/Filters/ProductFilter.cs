using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
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

    private const string serverUrl = "http://localhost/brilpasser-backend/unity/getMonturen.php/"; // URL to your server-side script
    private const string localJsonFilePath = "Resources/ProductData/monturen.json"; // Path to the locally stored JSON file

    void Start()
    {
        applyFilterButton.onClick.AddListener(ApplyFilters);
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
        string filePath = Path.Combine(Application.dataPath, localJsonFilePath);

        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                Debug.Log("Loaded JSON from local file: " + json);
                ProductList productList = JsonUtility.FromJson<ProductList>(json);
                allProducts = productList.products;
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
        string filePath = Path.Combine(Application.dataPath, localJsonFilePath);
        Debug.Log(filePath);
        File.WriteAllText(filePath, jsonData);
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
            bool shapeMatch = selectedShape == "All" || selectedShape == "GEEN KEUZE" || product.Shape == selectedShape;
            bool materialMatch = selectedMaterial == "All" || selectedMaterial == "GEEN KEUZE" || product.Material == selectedMaterial;
            bool colorMatch = selectedColor == "All" || selectedColor == "GEEN KEUZE" || product.Color == selectedColor;
            bool typeMatch = selectedType == "All" || selectedType == "GEEN KEUZE" || product.Type == selectedType;
            bool brandMatch = selectedBrand == "All" || selectedBrand == "GEEN KEUZE" || product.Brand == selectedBrand;
            bool genderMatch = selectedGender == "All" || selectedGender == "GEEN KEUZE" || product.Gender == selectedGender || product.Gender == "Unisex";

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

    [System.Serializable]
    public class ProductList
    {
        public List<Product> products;
    }
}

