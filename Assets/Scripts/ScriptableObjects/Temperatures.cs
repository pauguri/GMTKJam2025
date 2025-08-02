using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Temperatures", menuName = "Scriptable Objects/Temperatures")]
public class Temperatures : ScriptableObject
{
    public Temperature[] temperatures;
}

[Serializable]
public class Temperature
{
    public string name;
    public Sprite icon;
}