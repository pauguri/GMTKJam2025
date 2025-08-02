using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ManualHandler : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset materialCardTemplate;
    [SerializeField] private VisualTreeAsset combinationTemplate;
    [Space]
    [SerializeField] private Cleaners cleanersData;
    [SerializeField] private Temperatures temperaturesData;

    private Dictionary<GameMaterial, List<Vector2Int>> combinations = new Dictionary<GameMaterial, List<Vector2Int>>();
    private VisualElement materialsContainer;

    private void Start()
    {
        var document = GetComponent<UIDocument>();
        if (document == null)
        {
            Debug.LogError("ManualHandler requires a UIDocument component.");
            return;
        }

        materialsContainer = document.rootVisualElement.Q<VisualElement>("MaterialsArray");
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
        materialsContainer.Clear();
        foreach (var entry in combinations)
        {
            var instance = materialCardTemplate.Instantiate();
            instance.Q<VisualElement>("MaterialIcon").style.backgroundImage = new StyleBackground(entry.Key.icon);

            if (entry.Value.Count > 0)
            {
                instance.Q<Label>("MaterialText").text = entry.Key.name;
                instance.Q<Label>("Hint").text = entry.Key.description;

                var combinationsContainer = instance.Q<VisualElement>("CombinationsArray");
                foreach (var combination in entry.Value)
                {
                    var combinationInstance = combinationTemplate.Instantiate();
                    combinationInstance.Q<Label>("Detergent").style.backgroundImage = new StyleBackground(cleanersData.cleaners[combination.x].icon);
                    combinationInstance.Q<Label>("Temperature").style.backgroundImage = new StyleBackground(temperaturesData.temperatures[combination.y].icon);
                    combinationsContainer.Add(combinationInstance);
                }
            }
            else
            {
                instance.Q<Label>("MaterialText").text = "???";
                instance.Q<Label>("Hint").text = "";
            }
            materialsContainer.Add(instance);
        }
    }

    public void Open()
    {

    }
}