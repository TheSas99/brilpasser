using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartApp()
    {
        SceneManager.LoadScene("BrilChecker");
    }

    public void OpenFilterMenu()
    {
        SceneManager.LoadScene("Filter");
    }
}
