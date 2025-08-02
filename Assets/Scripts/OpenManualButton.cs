using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class OpenManualButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    [SerializeField] ManualHandler manualHandler;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOScale(0.9f, 0.2f).SetEase(Ease.OutBack);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        var sequence = DOTween.Sequence();
        sequence.Insert(0, rectTransform.DOScale(0.8f, 0.2f).SetEase(Ease.OutBack));
        sequence.Insert(0.15f, rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
        manualHandler.Open();
    }
}
