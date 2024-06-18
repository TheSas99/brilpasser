using UnityEngine;
using TMPro;

public class GlassesPriceCalculator : MonoBehaviour
{
    public TMP_Text totalPriceText;

    void Start()
    {
        CalculateTotalPrice();
    }

    void CalculateTotalPrice()
    {
        // Retrieve the selected glasses and their prices from PlayerPrefs
        string leftEyeGlasses = PlayerPrefs.GetString("LeftEyeGlasses", "");
        string rightEyeGlasses = PlayerPrefs.GetString("RightEyeGlasses", "");

        Debug.Log(leftEyeGlasses);
        Debug.Log(rightEyeGlasses);

        // Parse the prices from strings to floats
        float leftEyePrice = string.IsNullOrEmpty(leftEyeGlasses) ? 0f : float.Parse(leftEyeGlasses.Split('-')[1].Trim().Replace("€", ""));
        float rightEyePrice = string.IsNullOrEmpty(rightEyeGlasses) ? 0f : float.Parse(rightEyeGlasses.Split('-')[1].Trim().Replace("€", ""));

        // Calculate the total price
        float totalPrice = leftEyePrice + rightEyePrice;

        // Update the total price text
        totalPriceText.text = "€" + totalPrice.ToString("F2");
    }
}
