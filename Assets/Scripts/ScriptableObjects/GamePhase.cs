using UnityEngine;

[CreateAssetMenu(fileName = "GamePhase", menuName = "Scriptable Objects/Game Phase")]
public class GamePhase : ScriptableObject
{
    public GameMaterial[] materials;
    public ModifierType[] modifiers;
    public int targetScore;
    public int shownClothes = 1;
    public int correctScore = 1;
    public int wrongScore = -1;
    public int extraClothesScore = 1;
    [Space]
    public Sprite progressBarIcon;
    public Sprite progressBarGradient;
    public Sprite progressBarPattern;
    [Space]
    public Sprite unlockOverlay;
    public GameObject spinningCleanerPrefab;
}
