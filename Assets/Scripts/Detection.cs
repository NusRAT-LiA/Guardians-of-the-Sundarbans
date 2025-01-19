using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Detection : MonoBehaviour
{
    public TextMeshProUGUI tigerCounter;
    public TextMeshProUGUI deerCounter;
    public TextMeshProUGUI boarCounter;
    public TextMeshProUGUI foxCounter;
    public TextMeshProUGUI rabbitCounter;

    private int tigerCount = 0;
    private int deerCount = 0;
    private int boarCount = 0;
    private int foxCount = 0;
    private int rabbitCount = 0;

    private Collider detectedAnimal;
    private HashSet<Collider> detectedAnimals = new HashSet<Collider>(); 

    void Update()
    {

        if (detectedAnimal != null && Input.GetKeyDown(KeyCode.E))
        {
            if (!detectedAnimals.Contains(detectedAnimal))
            {
                detectedAnimals.Add(detectedAnimal);

                switch (detectedAnimal.tag)
                {
                    case "Tiger":
                        tigerCount++;
                        UpdateCounter(tigerCounter, tigerCount + " X", tigerCount);
                        break;

                    case "Deer":
                        deerCount++;
                        UpdateCounter(deerCounter, deerCount + " X", deerCount);
                        break;

                    case "Boar":
                        boarCount++;
                        UpdateCounter(boarCounter, boarCount + " X", boarCount);
                        break;

                    case "Fox":
                        foxCount++;
                        UpdateCounter(foxCounter, foxCount + " X", foxCount);
                        break;

                    case "Rabbit":
                        rabbitCount++;
                        UpdateCounter(rabbitCounter, rabbitCount + " X", rabbitCount);
                        break;
                }

                MarkAnimalAsDetected(detectedAnimal.gameObject);

                // Check for level transition conditions
                CheckLevelTransition();
            }
        }
    }

    private void UpdateCounter(TextMeshProUGUI textMesh, string text, int count)
    {
        textMesh.text = text;
        textMesh.color = GetColorBasedOnCount(count);
    }

    private Color GetColorBasedOnCount(int count)
    {
        if (count <= 3)
            return Color.red;
        else if (count <= 10)
            return new Color(1.0f, 0.5f, 0.0f); // Orange
        else if (count <= 15)
            return Color.yellow;
        else
            return new Color(0.0f, 1.0f, 0.0f); // Green
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tiger") || other.CompareTag("Deer") || other.CompareTag("Boar") || 
            other.CompareTag("Fox") || other.CompareTag("Rabbit"))
        {
            detectedAnimal = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == detectedAnimal)
        {
            detectedAnimal = null;
        }
    }

    private void MarkAnimalAsDetected(GameObject animal)
    {
        Renderer renderer = animal.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.gray; 
        }
    }

    private void CheckLevelTransition()
    {
        if (tigerCount == 2 && deerCount == 10 && foxCount == 10 && boarCount == 15 && rabbitCount > 15)
        {
            TransitToLevel2();
        }
    }

    private void TransitToLevel2()
    {
        Debug.Log("Conditions met! Transitioning to Level 2.");
        // Implement your level transition logic here
        // Example: Load the next scene or trigger an animation

        // Assuming you're using Unity's SceneManager
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level 2");
    }

    private void UnlockNextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}
