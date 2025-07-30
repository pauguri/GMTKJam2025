using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private GameObject tagPrefab;
    [SerializeField] private GameObject tagContainer;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject cleanerContainer;
    [SerializeField] private GameObject tempContainer;

    private static string[] cleaners = { "Normal", "Oil", "Sea water", "Anti-magic", "Acid" };
    private static string[] temperatures = { "Freezing", "Cold", "Normal", "Hot", "Hell" };
    private static Dictionary<string, bool[,]> materials = new() {
        { "Synthetic", new bool[,] {
            { true, false, false, false, false },
            { true, false, false, false, false },
            { true, false, false, false, false },
            { true, false, false, false, false },
            { true, false, false, false, false },
        } },
        { "Wool", new bool[,] {
            { true, false, false, false, false },
            { true, false, false, false, false },
            { true, false, false, false, false },
            { false, false, false, false, false },
            { false, false, false, false, false },
        } },
        { "Bacon", new bool[,] {
            { false, false, false, false, false },
            { false, false, false, false, false },
            { false, false, false, false, false },
            { false, true, false, false, false },
            { false, true, false, false, false },
        } },
        { "Perfectite", new bool[,] {
            { true, true, true, true, true },
            { true, true, true, true, true },
            { true, true, true, true, true },
            { true, true, true, true, true },
            { true, true, true, true, true },
        } },
    };

    private string currentMaterial = null;
    private int currentCleaner = -1;
    private int currentTemperature = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < cleaners.Length; i++)
        {
            GameObject button = Instantiate(buttonPrefab, cleanerContainer.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = cleaners[i];
            button.GetComponent<Button>().onClick.AddListener(() => currentCleaner = Array.IndexOf(cleaners, button.GetComponentInChildren<TextMeshProUGUI>().text));
        }

        for (int i = 0; i < temperatures.Length; i++)
        {
            GameObject button = Instantiate(buttonPrefab, tempContainer.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = temperatures[i];
            button.GetComponent<Button>().onClick.AddListener(() => currentTemperature = Array.IndexOf(temperatures, button.GetComponentInChildren<TextMeshProUGUI>().text));
        }

        GenerateClothes();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GenerateClothes()
    {
        int materialCount = materials.Count;
        int index = Random.Range(0, materialCount);
        currentMaterial = new List<string>(materials.Keys)[index];
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
        instance.GetComponentInChildren<TextMeshProUGUI>().text = currentMaterial;
    }

    public void SubmitWash()
    {
        if (currentCleaner < 0 || currentTemperature < 0)
        {
            Debug.Log("Please select a cleaner and a temperature.");
            return;
        }
        bool[,] materialMatrix = materials[currentMaterial];
        if (materialMatrix[currentTemperature, currentCleaner])
        {
            Debug.Log($"Successfully cleaned {currentMaterial} with {cleaners[currentCleaner]} at {temperatures[currentTemperature]} temperature.");
            GenerateClothes();
        }
        else
        {
            Debug.Log($"Failed to clean {currentMaterial} with {cleaners[currentCleaner]} at {temperatures[currentTemperature]} temperature.");
        }
    }
}
