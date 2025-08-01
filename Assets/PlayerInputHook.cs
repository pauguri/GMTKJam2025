using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHook : MonoBehaviour
{
    public static PlayerInputHook Instance { get; private set; }
    public Action<InputAction.CallbackContext> ClickEvent;
    public Vector2 mousePosition;
    public RaycastHit raycastHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private void Update()
    {
        mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        Physics.Raycast(ray, out raycastHit);
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        ClickEvent?.Invoke(context);
    }
}
