using DG.Tweening;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    public void Show()
    {
        canvasGroup.alpha = 1f;
    }

    public void Hide()
    {
        canvasGroup.DOFade(0f, 1f);
    }
}
