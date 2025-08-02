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
}

public enum ModifierType
{
    None,
    Flamable,
    Explosive,
    Frozen,
    Electrified
}