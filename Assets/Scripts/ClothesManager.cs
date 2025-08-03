using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClothesManager : MonoBehaviour
{
    [SerializeField] private GameObject[] clothingItemPrefabs;
    [SerializeField] private Transform[] spawnPoints2;
    [SerializeField] private Transform[] spawnPoints3;
    [SerializeField] private Transform[] spawnPoints4;
    [SerializeField] AudioSource selectClothesSound;
    private Transform[][] indexedSpawnPoints;
    [HideInInspector] public int shownClothes = 1;
    [HideInInspector] public ClothesState clothesState = ClothesState.CanBeSelected;
    public Action<ClothingItem[]> onUpdateSelected;
    private List<ClothingItem> clothes = new List<ClothingItem>();
    private List<ClothingItem> selectedClothes = new List<ClothingItem>();
    public ClothingItem[] Clothes => clothes.ToArray();
    public ClothingItem[] SelectedClothes => selectedClothes.ToArray();

    private void Start()
    {
        indexedSpawnPoints = new Transform[4][];
        indexedSpawnPoints[0] = new Transform[] { transform };
        indexedSpawnPoints[1] = spawnPoints2;
        indexedSpawnPoints[2] = spawnPoints3;
        indexedSpawnPoints[3] = spawnPoints4;
    }

    public void CreateClothingItem(GameMaterial gameMaterial, ModifierType modifierType, int clothingIndex = -1)
    {
        int clothingItemIndex = clothingIndex < 0 ? Random.Range(0, clothingItemPrefabs.Length) : clothingIndex;
        GameObject itemObject = Instantiate(clothingItemPrefabs[clothingItemIndex], indexedSpawnPoints[shownClothes - 1][clothes.Count]);
        if (itemObject.TryGetComponent<ClothingItem>(out var clothingItem))
        {
            clothingItem.clothesManager = this;
            clothingItem.Init(gameMaterial, modifierType);
            clothes.Add(clothingItem);
        }
    }

    public void HandleSelectItem(ClothingItem item)
    {
        if (selectedClothes.Contains(item)) return;
        selectedClothes.Add(item);
        onUpdateSelected?.Invoke(SelectedClothes);
        selectClothesSound.Play();
    }

    public void HandleDeselectItem(ClothingItem item)
    {
        if (!selectedClothes.Contains(item)) return;
        selectedClothes.Remove(item);
        onUpdateSelected?.Invoke(SelectedClothes);
    }

    public void HideSelectedAndDiscardRest()
    {
        foreach (ClothingItem item in clothes)
        {
            if (selectedClothes.Contains(item))
            {
                item.Hide();
            }
            else
            {
                item.Discard();
            }
        }
    }

    public void ShowSelected()
    {
        foreach (ClothingItem item in selectedClothes)
        {
            item.Show();
        }
    }

    public void DiscardAll()
    {
        foreach (ClothingItem item in clothes)
        {
            item.Discard();
        }
    }

    public void Clear()
    {
        foreach (ClothingItem item in clothes)
        {
            Destroy(item.gameObject);
        }

        clothes.Clear();
        selectedClothes.Clear();
        onUpdateSelected?.Invoke(SelectedClothes);
    }
}

public enum ClothesState
{
    CanBeSelected,
    ShowsResult,
}