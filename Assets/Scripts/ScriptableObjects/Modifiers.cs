using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Modifiers", menuName = "Scriptable Objects/Modifiers")]
public class Modifiers : ScriptableObject
{
    public Modifier[] modifiers;

    public Modifier GetModifier(ModifierType type)
    {
        foreach (var mod in modifiers)
        {
            if (mod.type == type)
            {
                return mod;
            }
        }
        return null;
    }
}

[Serializable]
public class Modifier
{
    public ModifierType type;
    public Sprite icon;
    [TextArea] public string description;
    public string triggerMessage;
    public bool[] freezingTriggers = new bool[5];
    public bool[] coldTriggers = new bool[5];
    public bool[] normalTriggers = new bool[5];
    public bool[] hotTriggers = new bool[5];
    public bool[] hellTriggers = new bool[5];

    public bool[][] GetMatrix()
    {
        return new bool[][]
        {
            freezingTriggers,
            coldTriggers,
            normalTriggers,
            hotTriggers,
            hellTriggers
        };
    }
}

public enum ModifierType
{
    None,
    Flamable,
    Explosive,
    Frozen,
    Electrified
}