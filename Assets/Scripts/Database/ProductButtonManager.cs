using UnityEngine;
using UnityEngine.UI;

public class ProductButtonManager : MonoBehaviour
{
    public Button[] productButtons;
    public Button selectedButton;
    private string selectedButtonKey = "SelectedButton";

    // Start is called before the first frame update
    void Start()
    {
        // Add click listeners to all product buttons
        foreach (Button button in productButtons)
        {
            button.onClick.AddListener(() => SelectProduct(button));
        }
    }

    // Method to handle product button selection
    void SelectProduct(Button button)
    {
        // Deselect previously selected button if any
        if (selectedButton != null)
        {
            // Reset the appearance of the previously selected button
            selectedButton.GetComponent<Image>().color = Color.white;
        }

        // Select the new button
        selectedButton = button;

        // Remember which product button was pressed
        // You can store its reference or index for later use
        int selectedIndex = System.Array.IndexOf(productButtons, selectedButton);

        // Remember which product button was pressed
        SaveSelectedButton();

        // Perform any visual indication of selection
        selectedButton.GetComponent<Image>().color = Color.red;
    }

    // Method to save the selected button index
    void SaveSelectedButton()
    {
        int selectedIndex = System.Array.IndexOf(productButtons, selectedButton);
        PlayerPrefs.SetInt(selectedButtonKey, selectedIndex);
    }

}
