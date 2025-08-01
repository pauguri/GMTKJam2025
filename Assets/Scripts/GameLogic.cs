using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private CleanerPicker cleanerPicker;
    [SerializeField] private TemperatureDial tempDial;
    [SerializeField] private ClothesManager clothesManager;
    [SerializeField] private ProgressBar progressBar;

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
        progressBar.SetPercent(0f);
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
        progressBar.AnimateTo(0f);
        cleanerPicker.UnlockBottle(currentPhase);
        clothesManager.shownClothes = phases[currentPhase].shownClothes;

        GenerateClothes();
    }

    private void GenerateClothes()
    {
        GamePhase phase = phases[currentPhase];
        cleanerPicker.Clear();
        clothesManager.clothesState = ClothesState.CanBeSelected;

        int amountToGenerate = phase.shownClothes;
        if (clothesManager.Clothes.Length > 0)
        {
            var recycledClothes = clothesManager.Clothes.Except(clothesManager.SelectedClothes).ToArray();
            clothesManager.Clear();
            foreach (ClothingItem item in recycledClothes)
            {
                clothesManager.CreateClothingItem(item.gameMaterial, item.modifier);
            }
            amountToGenerate -= Mathf.Min(recycledClothes.Length, amountToGenerate);
        }
        else
        {
            clothesManager.Clear();
        }

        for (int i = 0; i < amountToGenerate; i++)
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

            var availableModifiers = phase.modifiers.Intersect(currentMaterial.allowedModifiers).ToArray();
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
        if (cleanerPicker.Value.Length == 0 || tempDial.Value < 0 || clothesManager.SelectedClothes.Length == 0)
        {
            Debug.Log("Please select at least one clothing item, one cleaner and a temperature.");
            return;
        }

        clothesManager.clothesState = ClothesState.ShowsResult;
        int correctClothes = 0;
        foreach (ClothingItem item in clothesManager.SelectedClothes)
        {
            string[][] materialMatrix = item.gameMaterial.GetMatrix();
            string matrixCell = materialMatrix[tempDial.Value][cleanerPicker.Value[0]];
            if (matrixCell.Length == 0)
            {
                score++;

                Debug.Log($"Successfully cleaned {item.gameMaterial.name} with {cleanersData.cleaners[cleanerPicker.Value[0]]} at {temperatures[tempDial.Value]} temperature. Score: {score}");
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

        progressBar.AnimateTo(Mathf.Min((float)score / phases[currentPhase].targetScore, 1f));
        if (score >= phases[currentPhase].targetScore)
        {
            Debug.Log($"Congratulations! You've reached the target score of {phases[currentPhase].targetScore} for phase {currentPhase + 1}. Moving to the next phase.");
            score = score - phases[currentPhase].targetScore;
            currentPhase++;
            StartPhase();
            return;
        }

        GenerateClothes();
    }
}
