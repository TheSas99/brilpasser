using UnityEngine;
using UnityEngine.UI;

public class CommunicationLinks : MonoBehaviour
{
    public Button phoneButton;
    public Button emailButton;

    public string phoneNumber;
    public string email;

    void Start()
    {
        phoneButton.onClick.AddListener(OpenTelephoneLink);
        emailButton.onClick.AddListener(OpenEmailLink);
    }

    void OpenTelephoneLink()
    {
        string telLink = "tel:" + phoneNumber;
        Debug.Log("Opening telephone link: " + telLink);
        Application.OpenURL(telLink);
    }

    void OpenEmailLink()
    {
        string emailLink = "mailto:" + email;
        Debug.Log("Opening email link: " + emailLink);
        Application.OpenURL(emailLink);
    }
}
