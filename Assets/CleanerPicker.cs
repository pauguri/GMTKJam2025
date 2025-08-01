using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CleanerPicker : MonoBehaviour
{
    [SerializeField] private CleanerBottle[] clanerBottles;
    [SerializeField] private GameObject[] lockedBottles;
    public bool canSelectMultiple = false;
    private List<CleanerBottle> selectedBottles = new List<CleanerBottle>();
    public int[] Value => selectedBottles.Select((bottle) => bottle.index).ToArray();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void HandleSelectBottle(CleanerBottle bottle)
    {
        if (selectedBottles.Contains(bottle)) return;
        if (!canSelectMultiple)
        {
            if (selectedBottles.Count > 0)
            {
                // Deselect all previously selected bottles
                foreach (var selectedBottle in selectedBottles)
                {
                    selectedBottle.SetSelected(false);
                }
                selectedBottles.Clear();
            }
            selectedBottles.Add(bottle);
        }
        else
        {
            selectedBottles.Add(bottle);
        }
    }

    public void HandleDeselectBottle(CleanerBottle bottle)
    {
        if (!selectedBottles.Contains(bottle)) return;
    }

    public void Clear()
    {
        foreach (var bottle in selectedBottles)
        {
            bottle.SetSelected(false);
        }
        selectedBottles.Clear();
    }
}
