using UnityEngine;
using UnityEngine.InputSystem;

public class TemperatureDial : MonoBehaviour
{
    private PlayerInput inputModule;
    private bool isHolding = false;
    private Vector2 startPoint;

    void Start()
    {
        //var eventSystem = EventSystem.current;
        //inputModule = eventSystem.GetComponent<PlayerInput>();
        //if (inputModule == null)
        //{
        //    Debug.LogError("InputSystemUIInputModule not found on EventSystem. Please add it to the EventSystem GameObject.");
        //    return;
        //}

        //InputAction clickAction = inputModule;
        //if (clickAction != null)
        //{
        //    clickAction.performed += OnPointerDown;
        //    clickAction.canceled += OnPointerUp;
        //}
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Pointer down on: " + hit.transform.name);
                if (hit.transform == transform)
                {
                    Debug.Log("Dial clicked: " + hit.transform.name);
                    isHolding = true;
                    startPoint = mousePosition;
                }
            }
        }
        else if (context.canceled)
        {
            isHolding = false;
            Debug.Log("Pointer released.");
        }
    }

    private void Update()
    {
        if (!isHolding) return;
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        float direction = (startPoint.y - mousePosition.y); // Adjust sensitivity as needed
        // limit rotation at 0 and 180
        float clampedXRotation = Mathf.Clamp(transform.eulerAngles.x + direction, -90, 90);
        transform.eulerAngles = new Vector3(clampedXRotation, transform.eulerAngles.y, transform.eulerAngles.z);
        startPoint = mousePosition; // Update startPoint to the new position
        Debug.Log("Holding dial... " + direction);
    }

    private void OnDestroy()
    {
        //if (inputModule != null)
        //{
        //    InputAction clickAction = inputModule.leftClick;
        //    if (clickAction != null)
        //    {
        //        clickAction.started -= OnPointerDown;
        //        clickAction.canceled -= OnPointerUp;
        //    }
        //}
    }
}
