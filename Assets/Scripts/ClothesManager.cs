using System.Collections.Generic;
using UnityEngine;

public class ClothesManager : MonoBehaviour
{
    [SerializeField] private GameObject[] clothingItemPrefabs;
    [SerializeField] private Transform[] spawnPoints2;
    [SerializeField] private Transform[] spawnPoints3;
    [SerializeField] private Transform[] spawnPoints4;
    private Transform[][] indexedSpawnPoints;
    [HideInInspector] public int shownClothes = 1;
    [HideInInspector] public ClothesState clothesState = ClothesState.CanBeSelected;
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

    public void CreateClothingItem(GameMaterial gameMaterial, ModifierType modifierType)
    {
        GameObject itemObject = Instantiate(clothingItemPrefabs[Random.Range(0, clothingItemPrefabs.Length)], indexedSpawnPoints[shownClothes - 1][clothes.Count]);
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
    }

    public void HandleDeselectItem(ClothingItem item)
    {
        if (!selectedClothes.Contains(item)) return;
        selectedClothes.Remove(item);
    }

    public void Clear()
    {
        foreach (ClothingItem item in clothes)
        {
            item.Discard();
        }
        clothes.Clear();
        selectedClothes.Clear();
    }
}

public enum ClothesState
{
    CanBeSelected,
    ShowsResult,
}