using TMPro;
using UnityEngine;

public class TotalPriceCalculator : MonoBehaviour
{
    public TMP_Text framePriceText;
    public TMP_Text totalGlassesPriceText;
    public TMP_Text totalPriceText;

    void Start()
    {
        CalculateTotalPrice();
    }

    void CalculateTotalPrice()
    {
        // Get the text from the TextMeshPro text objects
        string framePriceString = framePriceText.text;
        string totalGlassesPriceString = totalGlassesPriceText.text;

        // Parse the numerical values from the texts
        float framePriceValue = ParsePriceValue(framePriceString);
        float totalGlassesPriceValue = ParsePriceValue(totalGlassesPriceString);

        // Calculate the total price
        float totalPrice = framePriceValue + totalGlassesPriceValue;

        // Display the total price
        totalPriceText.text = "€" + totalPrice.ToString("F2");
    }

    float ParsePriceValue(string priceString)
    {
        // Remove non-numeric characters (like the euro sign) and parse the float value
        float priceValue;
        float.TryParse(priceString.Replace("€", "").Trim(), out priceValue);
        return priceValue;
    }
}
