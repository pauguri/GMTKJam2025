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

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    ;
                    isHolding = true;
                    startPoint = mousePosition;
                }
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
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        float direction = (startPoint.y - mousePosition.y) * 0.02f; // Adjust sensitivity as needed
        startPoint = mousePosition; // Update startPoint to the current position for continuous dragging
        // limit rotation at 0 and 180
        position = Mathf.Clamp(position + direction, 0, 4);
        int newSnappedPosition = Mathf.RoundToInt(position);
        if (newSnappedPosition != snappedPosition)
        {
            transform.DOLocalRotate(new Vector3(0, 0, newSnappedPosition * (-180 / 4)), 0.1f).SetEase(Ease.OutBack);
            snappedPosition = newSnappedPosition;
        }
    }
}
