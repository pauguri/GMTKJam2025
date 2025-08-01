using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    [SerializeField] private MenuManager menuManager;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("StartButton requires a RectTransform component.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DORotate(new Vector3(0, 0, 10), 0.2f).SetEase(Ease.OutBack);
        rectTransform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DORotate(Vector3.zero, 0.2f).SetEase(Ease.OutBack);
        rectTransform.DOScale(0.9f, 0.2f).SetEase(Ease.OutBack);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        var sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DORotate(new Vector3(0, 0, -5), 0.15f).SetEase(Ease.OutBack));
        sequence.Insert(0, rectTransform.DOScale(0.8f, 0.2f).SetEase(Ease.OutBack));
        sequence.Append(rectTransform.DORotate(new Vector3(0, 0, 10), 0.15f).SetEase(Ease.OutBack));
        sequence.Insert(0.15f, rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
        menuManager.StartGame();
    }
}
