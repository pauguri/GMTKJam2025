using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform bar;
    [SerializeField] private RectTransform barTrack;
    private CanvasGroup canvasGroup;
    private Tween tween = null;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void AnimateTo(float percent)
    {
        //tween = bar.DOFillAmount(percent, 0.5f).SetEase(Ease.InOutSine);
        tween = bar.DOSizeDelta(new Vector2((barTrack.rect.width - 70f) * percent + 70f, barTrack.rect.height), 0.5f).SetEase(Ease.InOutSine);
    }

    public void SetPercent(float percent)
    {
        if (tween != null && tween.IsActive())
        {
            tween.Kill();
        }
        bar.sizeDelta = new Vector2((barTrack.rect.width - 70f) * percent + 70f, barTrack.rect.height);
    }

    public void Show()
    {
        canvasGroup.DOFade(1f, 0.5f);
    }
    public void Hide()
    {
        canvasGroup.DOFade(0f, 0.5f);
    }
}
