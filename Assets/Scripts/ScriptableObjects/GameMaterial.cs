using System;
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
}