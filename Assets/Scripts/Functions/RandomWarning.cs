using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomWarning : MonoBehaviour
{
    public TextMeshProUGUI displayText; // Reference to the UI Text component
    public string[] messages; // Array to hold the different messages

    void Start()
    {
        if (messages.Length > 0)
        {
            // Pick a random message from the array
            int randomIndex = Random.Range(0, messages.Length);
            displayText.text = messages[randomIndex];
        }
        else
        {
            Debug.LogWarning("Messages array is empty. Please add some messages.");
        }
    }
}
