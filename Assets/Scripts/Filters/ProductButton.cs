using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ProductButton : MonoBehaviour
{
    public Button button;
    public Button selectButton; // New button for selection
    public TMP_Text productNameText;
    public Image productImage;
    public Image selectedIcon;
    public Image deselectedIcon;
    private Product product;
    private GameObject productUIPrefab;
    private Transform uiContainer;

    private bool isSelected = false; // Track selection state
    public static List<ProductButton> selectedButtons = new List<ProductButton>();

    public void Setup(Product product, GameObject productUIPrefab, Transform uiContainer)
    {
        this.product = product;
        this.productUIPrefab = productUIPrefab;
        this.uiContainer = uiContainer;

        productNameText.text = product.Name;
        productImage.sprite = product.Image; // Load image directly from Product property

        button.onClick.AddListener(OnButtonClick);
        selectButton.onClick.AddListener(OnSelectButtonClick); // Set listener for select button

        // Show or hide the select button based on is_pasbaar
        selectButton.gameObject.SetActive(product.IsPasbaar);
    }

    void OnButtonClick()
    {
        GameObject productUIGO = Instantiate(productUIPrefab, uiContainer);
        ProductUI productUI = productUIGO.GetComponent<ProductUI>();

        if (productUI != null)
        {
            productUI.Setup(product);
        }
        else
        {
            Debug.LogError("ProductUI component not found on instantiated product UI prefab.");
        }
    }

    void OnSelectButtonClick()
    {
        ToggleSelection();
    }

    void ToggleSelection()
    {
        if (isSelected)
        {
            DeselectItem();
        }
        else if (selectedButtons.Count < 3)
        {
            SelectItem();
        }
        else
        {
            Debug.Log("You can only select up to 3 items.");
        }
    }

    void SelectItem()
    {
        isSelected = true;
        selectedButtons.Add(this);
        selectButton.image = selectedIcon;
        UpdateSelectedItemsText();
    }

    void DeselectItem()
    {
        isSelected = false;
        selectedButtons.Remove(this);
        selectButton.image = deselectedIcon;
        UpdateSelectedItemsText();
    }

    void UpdateSelectedItemsText()
    {
        // Update the UI text in the ProductFilter with selected items
        ProductFilter.Instance.UpdateSelectedItemsText(selectedButtons.Select(b => b.product.Name).ToList());
    }
}
