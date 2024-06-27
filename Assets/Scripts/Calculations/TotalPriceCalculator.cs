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
        string framePriceString = framePriceText.text;
        string totalGlassesPriceString = totalGlassesPriceText.text;

        float framePriceValue = ParsePriceValue(framePriceString);
        float totalGlassesPriceValue = ParsePriceValue(totalGlassesPriceString);

        float totalPrice = framePriceValue + totalGlassesPriceValue;

        totalPriceText.text = "€" + totalPrice.ToString("F2");
    }

    float ParsePriceValue(string priceString)
    {
        float priceValue;
        float.TryParse(priceString.Replace("€", "").Trim(), out priceValue);
        return priceValue;
    }
}
