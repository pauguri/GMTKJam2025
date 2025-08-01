using UnityEngine;

[CreateAssetMenu(fileName = "GamePhase", menuName = "Scriptable Objects/Game Phase")]
public class GamePhase : ScriptableObject
{
    public GameMaterial[] materials;
    public int targetScore;
}
