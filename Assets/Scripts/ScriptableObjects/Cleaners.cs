using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Cleaners", menuName = "Scriptable Objects/Cleaners")]
public class Cleaners : ScriptableObject
{
    public Cleaner[] cleaners;
}

[Serializable]
public class Cleaner
{
    public string name;
    public Sprite icon;
    [TextArea] public string description;
}