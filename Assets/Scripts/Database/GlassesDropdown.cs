using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;

public class GlassesDropdown : MonoBehaviour
{
    public string phpScriptURL = "https://thunderleafstudios.nl/brilpasser-backend/unity/getBrillenglazen.php";
    public TMP_InputField leftEyeStrengthInputField;
    public TMP_InputField rightEyeStrengthInputField;
    public TMP_Dropdown leftEyeDropdown;
    public TMP_Dropdown rightEyeDropdown;

    private const string localJsonFilePath = "Resources/ProductData/brillenglazen.json";

    IEnumerator GetGlassesData(string strength, TMP_Dropdown dropdown)
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            string url = phpScriptURL + "?sterkte=" + strength;
            UnityWebRequest www = UnityWebRequest.Get(url);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success && !string.IsNullOrWhiteSpace(www.downloadHandler.text) && !www.downloadHandler.text.StartsWith("<br />"))
            {
                string jsonResponse = www.downloadHandler.text;
                SaveLocalJson(jsonResponse);
                ProcessGlassesData(jsonResponse, dropdown);
            }
            else
            {
                Debug.LogWarning("Failed to get glasses data from server. Loading locally stored JSON data.");
                LoadLocalJson(strength, dropdown);
            }
        }
        else
        {
            Debug.LogWarning("No internet connection. Loading locally stored JSON data.");
            LoadLocalJson(strength, dropdown);
        }
    }

    void LoadLocalJson(string strength, TMP_Dropdown dropdown)
    {
        string filePath = Path.Combine(Application.dataPath, localJsonFilePath);

        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                ProcessGlassesData(json, dropdown);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to parse local JSON file: " + ex.Message);
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
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Local JSON file updated.");
    }

    void ProcessGlassesData(string jsonResponse, TMP_Dropdown dropdown)
    {
        try
        {
            GlassesData glassesData = JsonUtility.FromJson<GlassesData>(jsonResponse);
            UpdateDropdownOptions(glassesData.products, dropdown);
        }
        catch (ArgumentException ex)
        {
            Debug.LogError("JSON parse error: " + ex.Message);
            Debug.LogError("Problematic JSON: " + jsonResponse);
        }
    }

    void UpdateDropdownOptions(List<GlassesProduct> products, TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (GlassesProduct product in products)
        {
            options.Add(new TMP_Dropdown.OptionData(product.glasnaam + " - " + product.glasprijs));
            PlayerPrefs.SetString(product.glasnaam, product.glasprijs);
        }

        dropdown.options = options;
    }

    public void OnDropdownValueChanged(int index)
    {
        string leftEyeSelection = leftEyeDropdown.options[leftEyeDropdown.value].text;
        PlayerPrefs.SetString("LeftEyeGlasses", leftEyeSelection);

        string rightEyeSelection = rightEyeDropdown.options[rightEyeDropdown.value].text;
        PlayerPrefs.SetString("RightEyeGlasses", rightEyeSelection);
    }

    public void OnSaveButtonClick()
    {
        string leftEyeStrength = leftEyeStrengthInputField.text;
        string rightEyeStrength = rightEyeStrengthInputField.text;

        if (!string.IsNullOrEmpty(leftEyeStrength) && !string.IsNullOrEmpty(rightEyeStrength))
        {
            PlayerPrefs.SetString("LeftEyeStrength", leftEyeStrength);
            PlayerPrefs.SetString("RightEyeStrength", rightEyeStrength);
            PlayerPrefs.Save();

            StartCoroutine(GetGlassesData(leftEyeStrength, leftEyeDropdown));
            StartCoroutine(GetGlassesData(rightEyeStrength, rightEyeDropdown));
        }
        else
        {
            Debug.LogWarning("Please enter eye strengths before saving.");
        }
    }

    void Start()
    {
        string savedLeftEyeStrength = PlayerPrefs.GetString("LeftEyeStrength", "");
        string savedRightEyeStrength = PlayerPrefs.GetString("RightEyeStrength", "");

        if (!string.IsNullOrEmpty(savedLeftEyeStrength) && !string.IsNullOrEmpty(savedRightEyeStrength))
        {
            leftEyeStrengthInputField.text = savedLeftEyeStrength;
            rightEyeStrengthInputField.text = savedRightEyeStrength;

            StartCoroutine(GetGlassesData(savedLeftEyeStrength, leftEyeDropdown));
            StartCoroutine(GetGlassesData(savedRightEyeStrength, rightEyeDropdown));
        }
    }
}

[System.Serializable]
public class GlassesData
{
    public List<GlassesProduct> products;
}

[System.Serializable]
public class GlassesProduct
{
    public string glas_id;
    public string glasnaam;
    public string sterkte;
    public string glasprijs;  // Changed to string to match the database type
}
