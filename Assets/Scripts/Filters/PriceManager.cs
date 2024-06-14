using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceManager : MonoBehaviour
{
    public GameObject priceInformation;
    public void OpenGlassPriceInfo()
    {
        priceInformation.SetActive(true);
    }

    public void CloseGlassPriceInfo()
    {
        priceInformation.SetActive(false);
    }
}
