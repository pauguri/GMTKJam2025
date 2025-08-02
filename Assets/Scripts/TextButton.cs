using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private TextMeshProUGUI text;
    [SerializeField] private string buttonText = "";
    public Action onClick;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.text = $"<u>{buttonText}</u>";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.text = buttonText;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        text.text = buttonText;
        onClick?.Invoke();
    }
}
