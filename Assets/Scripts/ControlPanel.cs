using DG.Tweening;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    public void Show()
    {
        transform.DOMoveY(0f, 0.5f).SetEase(Ease.OutExpo);
    }

    public void Hide()
    {
        transform.DOMoveY(-2.2f, 0.5f).SetEase(Ease.InExpo);
    }
}
