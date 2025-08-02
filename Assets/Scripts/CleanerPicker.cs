using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CleanerPicker : MonoBehaviour
{
    [SerializeField] private CleanerBottle[] clanerBottles;
    [SerializeField] private GameObject[] lockedBottles;
    [SerializeField] private Transform cleanersContainer;
    public bool canSelectMultiple = false;
    [HideInInspector] public bool canInteract = true;
    public Action<int[]> onUpdateSelected;
    private List<CleanerBottle> selectedBottles = new List<CleanerBottle>();
    public int[] Value => selectedBottles.Select((bottle) => bottle.index).ToArray();

    public void HandleSelectBottle(CleanerBottle bottle)
    {
        if (selectedBottles.Contains(bottle)) return;
        if (!canSelectMultiple)
        {
            if (selectedBottles.Count > 0)
            {
                // Deselect all previously selected bottles
                var bottlesToRemove = selectedBottles.ToList();
                foreach (var selectedBottle in bottlesToRemove)
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
        onUpdateSelected?.Invoke(Value);
    }

    public void HandleDeselectBottle(CleanerBottle bottle)
    {
        if (!selectedBottles.Contains(bottle)) return;
        selectedBottles.Remove(bottle);
        onUpdateSelected?.Invoke(Value);
    }

    public void UnlockBottle(int index)
    {
        Debug.Log(index < 1);
        Debug.Log((index - 1) > lockedBottles.Length);
        if (index < 1 || (index - 1) > lockedBottles.Length) return;
        Debug.Log($"Unlocking bottle at index {index - 1}");
        lockedBottles[index - 1].SetActive(false);
        clanerBottles[index - 1].gameObject.SetActive(true);
    }

    public void Hide()
    {
        cleanersContainer.DOMoveY(-3f, 0.5f).SetEase(Ease.InExpo);
        canInteract = false;
    }

    public void Show()
    {
        cleanersContainer.DOMoveY(0f, 0.5f).SetEase(Ease.OutExpo);
        canInteract = true;
    }

    public void Clear()
    {
        var bottlesToRemove = selectedBottles.ToList();
        foreach (var bottle in bottlesToRemove)
        {
            bottle.SetSelected(false);
        }
        selectedBottles.Clear();
        onUpdateSelected?.Invoke(Value);
    }
}
