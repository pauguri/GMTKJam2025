using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private GameObject tagPrefab;
    [SerializeField] private GameObject tagContainer;
    [SerializeField] private CleanerPicker cleanerPicker;
    [SerializeField] private TemperatureDial tempDial;
    [SerializeField] private ClothesManager clothesManager;

    public Cleaners cleanersData;
    public GamePhase[] phases;

    private static string[] temperatures = { "Freezing", "Cold", "Normal", "Hot", "Hell" };

    [HideInInspector] public int currentPhase = 0;
    [HideInInspector] public int score = 0;
    private List<GameMaterial> materialPool = new List<GameMaterial>();

    private bool isFirstCycle = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPhase();
    }

    private void StartPhase()
    {
        if (currentPhase >= phases.Length)
        {
            Debug.Log("Congratulations! You've completed all phases of the game!");
            return; // End the game or reset to the first phase
        }

        Debug.Log($"Starting phase {currentPhase + 1} with target score: {phases[currentPhase].targetScore}");
        score = 0;
        cleanerPicker.UnlockBottle(currentPhase);

        GenerateClothes();
    }

    private void GenerateClothes()
    {
        GamePhase phase = phases[currentPhase];
        cleanerPicker.Clear();
        clothesManager.Clear();
        clothesManager.shownClothes = phase.shownClothes;
        clothesManager.clothesState = ClothesState.CanBeSelected;

        for (int i = 0; i < phase.shownClothes; i++)
        {
            if (materialPool == null || materialPool.Count == 0)
            {
                materialPool = new List<GameMaterial>(phase.materials);

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

            var currentMaterial = materialPool[^1];
            materialPool.RemoveAt(materialPool.Count - 1);

            var availableModifiers = phases[currentPhase].modifiers.Intersect(currentMaterial.allowedModifiers).ToArray();
            ModifierType currentModifier = ModifierType.None;
            if (availableModifiers.Length > 0)
            {
                currentModifier = availableModifiers[Random.Range(0, availableModifiers.Length)];
            }

            clothesManager.CreateClothingItem(currentMaterial, currentModifier);
        }
    }

    public void SubmitWash()
    {
        if (cleanerPicker.Value.Length == 0 || tempDial.Value < 0 || clothesManager.Clothes.Length == 0)
        {
            Debug.Log("Please select at least one clothing item, one cleaner and a temperature.");
            return;
        }

        clothesManager.clothesState = ClothesState.ShowsResult;
        int correctClothes = 0;
        foreach (ClothingItem item in clothesManager.Clothes)
        {
            string[][] materialMatrix = item.gameMaterial.GetMatrix();
            string matrixCell = materialMatrix[tempDial.Value][cleanerPicker.Value[0]];
            if (matrixCell.Length == 0)
            {
                score++;

                Debug.Log($"Successfully cleaned {item.gameMaterial.name} with {cleanersData.cleaners[cleanerPicker.Value[0]]} at {temperatures[tempDial.Value]} temperature. Score: {score}");

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
                //score--;
                //if (score < 0)
                //{
                //    score = 0;
                //}
                Debug.Log($"The {item.gameMaterial.name} shirt {matrixCell}. Score: {score}");
            }
        }

        if (correctClothes > 1)
        {
            score += correctClothes - 1;
        }

        GenerateClothes();
    }
}
