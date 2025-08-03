using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ManualHandler : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset materialCardTemplate;
    [SerializeField] private VisualTreeAsset combinationTemplate;
    [SerializeField] private VisualTreeAsset moreTemplate;
    [Space]
    [SerializeField] private Cleaners cleanersData;
    [SerializeField] private Temperatures temperaturesData;

    private Dictionary<GameMaterial, List<Vector2Int>> combinations = new Dictionary<GameMaterial, List<Vector2Int>>();
    private VisualElement rootElement;

    private void Start()
    {
        var document = GetComponent<UIDocument>();
        if (document == null)
        {
            Debug.LogError("ManualHandler requires a UIDocument component.");
            return;
        }

        rootElement = document.rootVisualElement;

        rootElement.Q<Button>("CloseButton").clicked += Hide;
    }

    public void AddMaterials(GameMaterial[] materials)
    {
        foreach (var material in materials)
        {
            if (!combinations.ContainsKey(material))
            {
                combinations[material] = new List<Vector2Int>();
            }
        }

        UpdateManual();
    }

    public void AddCombination(GameMaterial material, int temperatureIndex, int cleanerIndex)
    {
        if (!combinations.ContainsKey(material))
        {
            combinations[material] = new List<Vector2Int>() { new Vector2Int(temperatureIndex, cleanerIndex) };
        }
        else
        {
            combinations[material].Add(new Vector2Int(temperatureIndex, cleanerIndex));
        }

        UpdateManual();
    }

    private void UpdateManual()
    {
        var materialsContainer = rootElement.Q<VisualElement>("MaterialsArray");
        materialsContainer.Clear();
        foreach (var entry in combinations)
        {
            var instance = materialCardTemplate.Instantiate();
            instance.Q<VisualElement>("MaterialIcon").style.backgroundImage = new StyleBackground(entry.Key.icon);

            if (entry.Value.Count > 0)
            {
                Debug.Log(instance);
                Debug.Log(instance.Q<Label>("MaterialName"));
                instance.Q<Label>("MaterialName").text = entry.Key.name;
                instance.Q<Label>("Hint").text = entry.Key.description;

                var combinationsContainer = instance.Q<VisualElement>("CombinationsArray");
                foreach (var combination in entry.Value)
                {
                    var combinationInstance = combinationTemplate.Instantiate();
                    combinationInstance.Q<VisualElement>("Temperature").style.backgroundImage = new StyleBackground(temperaturesData.temperatures[combination.x].icon);
                    combinationInstance.Q<VisualElement>("Detergent").style.backgroundImage = new StyleBackground(cleanersData.cleaners[combination.y].icon);
                    combinationsContainer.Add(combinationInstance);
                }

                if (entry.Key.GetCorrectCombinations().Length > entry.Value.Count)
                {
                    var moreInstance = moreTemplate.Instantiate();
                    combinationsContainer.Add(moreInstance);
                }
            }
            else
            {
                instance.Q<Label>("MaterialName").text = "???";
                instance.Q<Label>("Hint").text = "";
            }
            materialsContainer.Add(instance);
        }
    }

    public void Show()
    {
        var manual = rootElement.Q<VisualElement>("Manual");
        manual.AddToClassList("shown");
        manual.RemoveFromClassList("hidden");
    }

    public void Hide()
    {
        var manual = rootElement.Q<VisualElement>("Manual");
        manual.AddToClassList("hidden");
        manual.RemoveFromClassList("shown");
    }

    private void OnDestroy()
    {
        if (rootElement != null)
        {
            var button = rootElement.Q<Button>("CloseButton");
            if (button != null)
            {
                button.clicked -= Hide;
            }
        }
    }
}