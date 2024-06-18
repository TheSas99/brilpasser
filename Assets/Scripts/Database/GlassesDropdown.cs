using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class GlassesDropdown : MonoBehaviour
{
    public string phpScriptURL = "http://localhost/brilpasser-backend/unity/getBrillenglazen.php";
    public TMP_InputField leftEyeStrengthInputField;
    public TMP_InputField rightEyeStrengthInputField;
    public TMP_Dropdown leftEyeDropdown;
    public TMP_Dropdown rightEyeDropdown;
    private bool dataLoaded = false;

    IEnumerator GetGlassesData(string strength, TMP_Dropdown dropdown)
    {
        string url = phpScriptURL + "?sterkte=" + strength;
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to get glasses data: " + www.error);
        }
        else
        {
            string jsonResponse = www.downloadHandler.text;
            GlassesData glassesData = JsonUtility.FromJson<GlassesData>(jsonResponse);
            UpdateDropdownOptions(glassesData.products, dropdown);
        }
    }

    void UpdateDropdownOptions(List<GlassesProduct> products, TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (GlassesProduct product in products)
        {
            options.Add(new TMP_Dropdown.OptionData(product.glasnaam + " - " + product.glasprijs.ToString("C")));
        }

        dropdown.options = options;
    }

    public void OnDropdownValueChanged(int index)
    {
        Debug.Log("Selected glasses: " + leftEyeDropdown.options[index].text);
        Debug.Log("Selected glasses: " + rightEyeDropdown.options[index].text);
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

            dataLoaded = true;
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

            dataLoaded = true;
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
    public float glasprijs;
}
