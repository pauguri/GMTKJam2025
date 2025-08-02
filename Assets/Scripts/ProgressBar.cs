using DG.Tweening;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform bar;
    [SerializeField] private RectTransform barTrack;
    private Tween tween = null;

    public void AnimateTo(float percent)
    {
        //tween = bar.DOFillAmount(percent, 0.5f).SetEase(Ease.InOutSine);
        tween = bar.DOSizeDelta(new Vector2(barTrack.rect.width * percent, barTrack.rect.height), 0.5f).SetEase(Ease.InOutSine);
    }

    public void SetPercent(float percent)
    {
        if (tween != null && tween.IsActive())
        {
            tween.Kill();
        }
        bar.sizeDelta = new Vector2(barTrack.rect.width * percent, barTrack.rect.height);
    }
}
