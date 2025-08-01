using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class TemperatureDial : MonoBehaviour
{
    private bool isHolding = false;
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

            if (PlayerInputHook.Instance.raycastHit.transform == transform)
            {
                ;
                isHolding = true;
                startPoint = PlayerInputHook.Instance.mousePosition;
            }
        }
        else if (context.canceled)
        {
            isHolding = false;
        }
    }

    private void Update()
    {
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
