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

public class Modifier
{
    public ModifierType type;
    public Sprite icon;
}

public enum ModifierType
{
    None,
    Flamable,
    Explosive,
    Frozen,
    Electrified
}