using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartApp()
    {
        SceneManager.LoadScene("Filter");
    }

    public void OpenFilterMenu()
    {
        SceneManager.LoadScene("Filter");
    }

    public void OpenProductsMenu()
    {
        SceneManager.LoadScene("Producten");
    }

    public void OpenSettingsMenu()
    {
        SceneManager.LoadScene("Settings");
    }
}
