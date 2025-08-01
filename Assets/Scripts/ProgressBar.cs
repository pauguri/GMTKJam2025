using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ProgressBar : MonoBehaviour
{
    private Image image;
    private Tween tween;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void AnimateTo(float percent)
    {
        tween = image.DOFillAmount(percent, 0.5f).SetEase(Ease.InOutSine);
    }

    public void SetPercent(float percent)
    {
        if (tween != null && tween.IsActive())
        {
            tween.Kill();
        }
        image.fillAmount = percent;
    }
}
