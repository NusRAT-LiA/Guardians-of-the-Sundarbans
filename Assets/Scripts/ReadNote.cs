using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this line

public class ReadNote : MonoBehaviour
{
    [SerializeField] private Image noteImage;

    public bool Action = false;
    public GameObject GreetingPanel;
    public GameObject MessagePanel;

    void Start()
    {
        noteImage.enabled = false;
        MessagePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Action)
            {
                MessagePanel.SetActive(false);
                Action = false;
                noteImage.enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GreetingPanel.SetActive(false);
            MessagePanel.SetActive(true);
            Action = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GreetingPanel.SetActive(true);
            MessagePanel.SetActive(false);
            Action = false;
            noteImage.enabled = false;
        }
    }
}
