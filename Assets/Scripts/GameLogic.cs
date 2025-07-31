using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private GameObject tagPrefab;
    [SerializeField] private GameObject tagContainer;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject cleanerContainer;
    [SerializeField] private GameObject tempContainer;

    public Cleaners cleanersData;
    public GamePhase[] phases;

    private static string[] temperatures = { "Freezing", "Cold", "Normal", "Hot", "Hell" };

    [HideInInspector] public int currentPhase = 0;
    [HideInInspector] public int score = 0;
    private List<Material> materialPool;
    private Material currentMaterial = null;
    private int currentCleaner = -1;
    private int currentTemperature = -1;

    private bool isFirstCycle = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < temperatures.Length; i++)
        {
            GameObject button = Instantiate(buttonPrefab, tempContainer.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = temperatures[i];
            button.GetComponent<Button>().onClick.AddListener(() => currentTemperature = Array.IndexOf(temperatures, button.GetComponentInChildren<TextMeshProUGUI>().text));
        }

        StartPhase();
    }

    private void StartPhase()
    {
        if (currentPhase >= phases.Length)
        {
            Debug.Log("Congratulations! You've completed all phases of the game!");
            return; // End the game or reset to the first phase
        }

        if (cleanerContainer.transform.childCount > 0)
        {
            foreach (Transform child in cleanerContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }
        for (int i = 0; i <= currentPhase; i++)
        {
            GameObject button = Instantiate(buttonPrefab, cleanerContainer.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = cleanersData.cleaners[i];
            button.GetComponent<Button>().onClick.AddListener(() => currentCleaner = Array.IndexOf(cleanersData.cleaners, button.GetComponentInChildren<TextMeshProUGUI>().text));
        }

        GenerateClothes();
    }

    private void GenerateClothes()
    {
        GamePhase phase = phases[currentPhase];

        if (materialPool == null || materialPool.Count == 0)
        {
            materialPool = new List<Material>(phase.materials);

            // Don't shuffle on the first cycle, it acts as a tutorial
            if (currentPhase == 0 && isFirstCycle)
            {
                isFirstCycle = false;
                materialPool.Reverse();
            }
            else
            {
                materialPool.Shuffle();
            }
        }

        currentMaterial = materialPool[^1];
        materialPool.RemoveAt(materialPool.Count - 1);

        currentCleaner = -1;
        currentTemperature = -1;

        if (tagContainer.transform.childCount > 0)
        {
            foreach (Transform child in tagContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }
        GameObject instance = Instantiate(tagPrefab, tagContainer.transform);
        instance.GetComponentInChildren<TextMeshProUGUI>().text = currentMaterial.name;
    }

    public void SubmitWash()
    {
        if (currentCleaner < 0 || currentTemperature < 0)
        {
            Debug.Log("Please select a cleaner and a temperature.");
            return;
        }
        string[][] materialMatrix = currentMaterial.GetMatrix();
        string matrixCell = materialMatrix[currentTemperature][currentCleaner];
        if (matrixCell.Length == 0)
        {
            score++;
            Debug.Log($"Successfully cleaned {currentMaterial} with {cleanersData.cleaners[currentCleaner]} at {temperatures[currentTemperature]} temperature. Score: {score}");

            if (score >= phases[currentPhase].targetScore)
            {
                Debug.Log($"Congratulations! You've reached the target score of {phases[currentPhase].targetScore} for phase {currentPhase + 1}. Moving to the next phase.");
                currentPhase++;
                StartPhase();
                return;
            }
        }
        else
        {
            score--;
            if (score < 0)
            {
                score = 0; // Prevent negative score
            }
            Debug.Log($"The {currentMaterial.name} shirt {matrixCell}. Score: {score}");
        }

        GenerateClothes();
    }
}
