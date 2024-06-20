using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpecialKeyboard : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private InputField inputField;

    void Start()
    {
        inputField = GetComponent<InputField>();
        inputField.contentType = InputField.ContentType.DecimalNumber;
        inputField.onEndEdit.AddListener(delegate { CloseKeyboard(); });
    }

    public void OnSelect(BaseEventData eventData)
    {
        OpenKeyboard();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        CloseKeyboard();
    }

    void Update()
    {
        inputField.text = SanitizeInput(inputField.text);
    }

    string SanitizeInput(string input)
    {
        System.Text.StringBuilder sanitized = new System.Text.StringBuilder();
        foreach (char c in input)
        {
            if (IsValidCharacter(c))
            {
                sanitized.Append(c);
            }
        }
        return sanitized.ToString();
    }

    bool IsValidCharacter(char c)
    {
        return c == '-' || char.IsDigit(c) || c == '.';
    }

    void OpenKeyboard()
    {
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad);
    }

    void CloseKeyboard()
    {
        if (TouchScreenKeyboard.visible)
        {
            TouchScreenKeyboard.hideInput = true;
        }
    }
}
