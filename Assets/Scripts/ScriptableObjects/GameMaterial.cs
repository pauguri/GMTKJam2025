using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameMaterial
{
    public string name;
    public string description;
    public Sprite icon;
    public Material material;
    public string[] freezingMessages = new string[5];
    public string[] coldMessages = new string[5];
    public string[] normalMessages = new string[5];
    public string[] hotMessages = new string[5];
    public string[] hellMessages = new string[5];
    public ModifierType[] allowedModifiers;
    public string[][] GetMatrix()
    {
        return new string[][]
        {
            freezingMessages,
            coldMessages,
            normalMessages,
            hotMessages,
            hellMessages
        };
    }

    public Vector2Int[] GetCorrectCombinations()
    {
        List<Vector2Int> correctCombinations = new List<Vector2Int>();
        string[][] matrix = GetMatrix();

        for (int temperature = 0; temperature < matrix.Length; temperature++)
        {
            for (int cleaner = 0; cleaner < matrix[temperature].Length; cleaner++)
            {
                if (string.IsNullOrEmpty(matrix[temperature][cleaner]))
                {
                    Vector2Int combination = new Vector2Int(temperature, cleaner);
                    correctCombinations.Add(combination);
                }
            }
        }

        return correctCombinations.ToArray();
    }
}