using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private CleanerPicker cleanerPicker;
    [SerializeField] private TemperatureDial tempDial;
    [SerializeField] private ClothesManager clothesManager;
    [SerializeField] private ControlPanel controlPanel;
    [SerializeField] private ProgressBarManager progressBarManager;
    [Space]
    [SerializeField] private LightIndicator clothesIndicator;
    [SerializeField] private LightIndicator cleanerIndicator;
    [SerializeField] private WashButton washButton;
    [SerializeField] private Animator washingMachine;
    [SerializeField] private TextButton continueButton;
    [SerializeField] private UnlockOverlay unlockOverlay;
    [Space]

    public Cleaners cleanersData;
    public GamePhase[] phases;

    private static string[] temperatures = { "Freezing", "Cold", "Normal", "Hot", "Hell" };

    [HideInInspector] public int currentPhase = 0;
    [HideInInspector] public int score = 0;
    private List<GameMaterial> materialPool = new List<GameMaterial>();

    private bool isClothesReady = false;
    private bool isCleanerReady = false;
    private bool canSubmit = false;

    private bool isFirstMaterialPoolCycle = true;
    private bool isFirstWash = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cleanerPicker.onUpdateSelected += UpdateReadyCleaner;
        clothesManager.onUpdateSelected += UpdateReadyClothes;
        washButton.onClick += SubmitWash;

        progressBarManager.SetPercent(0f);
        progressBarManager.Hide();
        continueButton.gameObject.SetActive(false);
        DOVirtual.DelayedCall(2f, StartRound);
    }

    private void StartRound()
    {
        GamePhase phase = phases[currentPhase];
        controlPanel.Show();
        cleanerPicker.Show();
        progressBarManager.ShowSmall();
        clothesManager.clothesState = ClothesState.CanBeSelected;

        int amountToGenerate = isFirstWash ? 1 : phase.shownClothes;
        clothesManager.shownClothes = amountToGenerate;

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
                if (currentPhase == 0 && isFirstMaterialPoolCycle)
                {
                    isFirstMaterialPoolCycle = false;
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

    public void UpdateReadyCleaner(int[] newSelection)
    {
        isCleanerReady = newSelection.Length > 0;
        cleanerIndicator.SetState(isCleanerReady);
        TryEnableWashButton();
    }

    public void UpdateReadyClothes(ClothingItem[] newSelection)
    {
        isClothesReady = newSelection.Length > 0;
        clothesIndicator.SetState(isClothesReady);
        TryEnableWashButton();
    }

    public void TryEnableWashButton()
    {
        if (isCleanerReady && isClothesReady && tempDial.Value >= 0)
        {
            Debug.Log("Wash button enabled.");
            washButton.SetEnabled(true);
            canSubmit = true;
        }
        else
        {
            Debug.Log("Wash button disabled.");
            washButton.SetEnabled(false);
        }
    }

    public void SubmitWash()
    {
        if (!isCleanerReady || tempDial.Value < 0 || !isClothesReady)
        {
            Debug.Log("Please select at least one clothing item, one cleaner and a temperature.");
            return;
        }

        if (!canSubmit)
        {
            Debug.Log("Cannot submit wash, conditions not met.");
            return;
        }
        canSubmit = false; // Prevent multiple submissions
        if (isFirstWash) { isFirstWash = false; }

        cleanerPicker.Hide();
        progressBarManager.Hide();
        clothesManager.Hide();
        controlPanel.Hide();

        washingMachine.SetTrigger("Run");
        DOVirtual.DelayedCall(3f, ShowResults);
    }

    private void ShowResults()
    {
        int correctClothes = 0;
        foreach (ClothingItem item in clothesManager.SelectedClothes)
        {
            string[][] materialMatrix = item.gameMaterial.GetMatrix();
            string matrixCell = materialMatrix[tempDial.Value][cleanerPicker.Value[0]];
            if (string.IsNullOrEmpty(matrixCell))
            {
                score++;
                item.errorMessage = "";
                // manualHandler.AddCombination(item.gameMaterial, tempDial.Value, cleanerPicker.Value[0]);
                Debug.Log($"Successfully cleaned {item.gameMaterial.name} with {cleanersData.cleaners[cleanerPicker.Value[0]]} at {temperatures[tempDial.Value]} temperature. Score: {score}");
            }
            else
            {
                //score--;
                //if (score < 0)
                //{
                //    score = 0;
                //}
                item.errorMessage = $"It {matrixCell}.";
                Debug.Log($"The {item.gameMaterial.name} shirt {matrixCell}. Score: {score}");
            }
        }

        if (correctClothes > 1)
        {
            // Combo for washing multiple clothes at once
            score += correctClothes - 1;
        }

        clothesManager.clothesState = ClothesState.ShowsResult;
        clothesManager.Show();
        progressBarManager.ShowBig();
        progressBarManager.AnimateTo(Mathf.Min((float)score / phases[currentPhase].targetScore, 1f));

        if (score >= phases[currentPhase].targetScore)
        {
            // Move to the next phase
            Debug.Log($"Congratulations! You've reached the target score of {phases[currentPhase].targetScore} for phase {currentPhase + 1}. Moving to the next phase.");
            score -= phases[currentPhase].targetScore;
            currentPhase++;
            if (currentPhase >= phases.Length)
            {
                Debug.Log("Congratulations! You've completed all phases of the game!");
                return; // End the game or reset to the first phase
            }

            DOVirtual.DelayedCall(2f, UnlockCleaner);
        }
        else
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                continueButton.gameObject.SetActive(true);
                continueButton.onClick += EndRound;
            });
        }
    }

    public void UnlockCleaner()
    {
        unlockOverlay.Show(phases[currentPhase]);
        progressBarManager.Hide();
        clothesManager.Discard();
        cleanerPicker.Clear();
        cleanerPicker.UnlockBottle(currentPhase);

        DOVirtual.DelayedCall(1f, () =>
        {
            continueButton.gameObject.SetActive(true);
            continueButton.onClick += EndPhase;
        });
    }

    public void EndPhase()
    {
        continueButton.onClick -= EndPhase;
        continueButton.gameObject.SetActive(false);

        unlockOverlay.Hide();
        progressBarManager.ChangeSprites(phases[currentPhase]);
        progressBarManager.SetPercent((float)score / phases[currentPhase].targetScore);

        DOVirtual.DelayedCall(2f, StartRound);
    }

    public void EndRound()
    {
        continueButton.onClick -= EndRound;
        continueButton.gameObject.SetActive(false);

        progressBarManager.Hide();
        clothesManager.Discard();
        cleanerPicker.Clear();

        DOVirtual.DelayedCall(2f, StartRound);
    }

    private void OnDestroy()
    {
        if (cleanerPicker != null)
        {
            cleanerPicker.onUpdateSelected -= UpdateReadyCleaner;
        }
        if (clothesManager != null)
        {
            clothesManager.onUpdateSelected -= UpdateReadyClothes;
        }
        if (washButton != null)
        {
            washButton.onClick -= SubmitWash;
        }
    }
}
