using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class TemperatureDial : MonoBehaviour
{
    [SerializeField] private Outline outline;
    private bool isHolding = false;
    private bool isHovered = false;
    private Vector2 startPoint;
    private float position = 2f;
    private int snappedPosition = 2;
    public int Value => snappedPosition;

    private void Start()
    {
        if (PlayerInputHook.Instance != null)
        {
            PlayerInputHook.Instance.ClickEvent += OnClick;
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {

            if (isHovered)
            {
                isHolding = true;
                startPoint = PlayerInputHook.Instance.mousePosition;
                DOTween.To(() => outline.OutlineColor.a, x => outline.OutlineColor = new Color(1, 1, 1, x), 1f, 0.15f);
            }
        }
        else if (context.canceled)
        {
            isHolding = false;
            if (isHovered)
            {
                DOTween.To(() => outline.OutlineColor.a, x => outline.OutlineColor = new Color(1, 1, 1, x), 0.2f, 0.15f);
            }
            else
            {
                DOTween.To(() => outline.OutlineColor.a, x => outline.OutlineColor = new Color(1, 1, 1, x), 0f, 0.15f);
            }
        }
    }

    private void Update()
    {
        if (PlayerInputHook.Instance.raycastHit.transform == transform)
        {
            if (!isHovered)
            {
                isHovered = true;
                if (!isHolding)
                {
                    DOTween.To(() => outline.OutlineColor.a, x => outline.OutlineColor = new Color(1, 1, 1, x), 0.2f, 0.15f);
                }
            }
        }
        else
        {
            if (isHovered)
            {
                isHovered = false;
                if (!isHolding)
                {
                    DOTween.To(() => outline.OutlineColor.a, x => outline.OutlineColor = new Color(1, 1, 1, x), 0f, 0.15f);
                }
            }
        }

        if (!isHolding) return;
        float direction = (startPoint.y - PlayerInputHook.Instance.mousePosition.y) * 0.02f; // Adjust sensitivity as needed
        startPoint = PlayerInputHook.Instance.mousePosition; // Update startPoint to the current position for continuous dragging
        // limit rotation at 0 and 180
        position = Mathf.Clamp(position + direction, 0, 4);
        int newSnappedPosition = Mathf.RoundToInt(position);
        if (newSnappedPosition != snappedPosition)
        {
            transform.DOLocalRotate(new Vector3(0, 0, newSnappedPosition * (-180 / 4)), 0.1f).SetEase(Ease.OutBack);
            snappedPosition = newSnappedPosition;
        }
    }

    private void OnDestroy()
    {
        if (PlayerInputHook.Instance != null)
        {
            PlayerInputHook.Instance.ClickEvent -= OnClick;
        }
    }
}
