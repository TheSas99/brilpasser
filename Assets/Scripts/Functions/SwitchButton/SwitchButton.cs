using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
    private Button button;
    private string productName;

    void Awake()
    {
        button = GetComponent<Button>(); // Get the Button component on this GameObject
    }

    public void Setup(string productName)
    {
        this.productName = productName;
        button.onClick.AddListener(SelectProduct);
        button.GetComponentInChildren<TextMeshProUGUI>().text = productName;
    }

    void SelectProduct()
    {
        PlayerPrefs.SetString("SelectedProductName", productName);
        LoadGlassesOnFace loadGlassesOnFace = FindObjectOfType<LoadGlassesOnFace>();
        if (loadGlassesOnFace != null)
        {
            loadGlassesOnFace.UpdateGlassesSprite(); // Update glasses display
        }
        else
        {
            Debug.LogError("LoadGlassesOnFace script not found in scene.");
        }
    }
}
