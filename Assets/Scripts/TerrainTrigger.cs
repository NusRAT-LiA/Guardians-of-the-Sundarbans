using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class TerrainTrigger : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    public TextMeshProUGUI messageText; // UI Text component to display messages
    public Canvas messageCanvas; // Reference to the Canvas
    public string[] tigerMessages = {
    "With fewer than 100 tigers estimated to remain in the Sundarbans, they are critically endangered due to habitat loss and poaching.",
    "The Sundarbans tigers are at constant risk from rising sea levels, which threaten their habitat due to climate change.",
    "Poaching for tiger skins and bones continues to threaten their survival, despite international bans and conservation laws.",
    "The Royal Bengal Tiger is listed as endangered on the IUCN Red List, with a global population of fewer than 2,500 mature individuals.",
    "Tigers in the Sundarbans face extreme challenges due to saltwater intrusion, reducing freshwater prey availability.",
     };
    public float messageDisplayTime = 3f; // Time each message is displayed

    private bool playerEntered = false; // To ensure messages show only once

    void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the terrain
        if (other.gameObject == player && !playerEntered)
        {
            playerEntered = true;
            StartCoroutine(DisplayMessages());
        }
    }

    IEnumerator DisplayMessages()
    {
        // Enable the canvas for messages
        messageCanvas.gameObject.SetActive(true);

        foreach (string message in tigerMessages)
    {
        // Display the current message
        messageText.text = message;

        // Wait for the message display time
        yield return new WaitForSeconds(messageDisplayTime);
    }

        // Hide the canvas after all messages are shown
        messageCanvas.gameObject.SetActive(false);
    }
}
