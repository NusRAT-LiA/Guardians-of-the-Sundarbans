using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

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
    private HashSet<Collider> detectedAnimals = new HashSet<Collider>(); // Tracks detected animals

    void Update()
    {
        if (detectedAnimal != null && Input.GetKeyDown(KeyCode.E))
        {
            if (!detectedAnimals.Contains(detectedAnimal))
            {
                // Add the animal to the detected list
                detectedAnimals.Add(detectedAnimal);

                // Update counters based on the tag
                switch (detectedAnimal.tag)
                {
                    case "Tiger":
                        tigerCount++;
                        UpdateCounter(tigerCounter, "X " + tigerCount, tigerCount);
                        break;

                    case "Deer":
                        deerCount++;
                        UpdateCounter(deerCounter, "X " + deerCount, deerCount);
                        break;

                    case "Boar":
                        boarCount++;
                        UpdateCounter(boarCounter, "X " + boarCount, boarCount);
                        break;

                    case "Fox":
                        foxCount++;
                        UpdateCounter(foxCounter, "X " + foxCount, foxCount);
                        break;

                    case "Rabbit":
                        rabbitCount++;
                        UpdateCounter(rabbitCounter, "X " + rabbitCount, rabbitCount);
                        break;
                }

                // Optional: Mark the animal visually or logically as detected
                MarkAnimalAsDetected(detectedAnimal.gameObject);
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
            return new Color(1.0f, 0.5f, 0.0f); // Orange again
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
        // Example: Change color or material to indicate detection
        Renderer renderer = animal.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.gray; // Change to gray as a visual marker
        }
    }
}
