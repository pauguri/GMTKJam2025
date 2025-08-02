using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WashButton : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Outline outline;
    public Action onClick;
    private bool isEnabled = false;
    private bool isHovered = false;

    private void Start()
    {
        if (PlayerInputHook.Instance != null)
        {
            PlayerInputHook.Instance.ClickEvent += OnClick;
        }
    }

    private void Update()
    {
        if (isEnabled && PlayerInputHook.Instance.raycastHit.transform == transform)
        {
            if (!isHovered)
            {
                isHovered = true;
                DOTween.To(() => outline.OutlineColor.a, x => outline.OutlineColor = new Color(1, 1, 1, x), 0.2f, 0.15f);
            }
        }
        else
        {
            if (isHovered)
            {
                isHovered = false;
                DOTween.To(() => outline.OutlineColor.a, x => outline.OutlineColor = new Color(1, 1, 1, x), 0f, 0.15f);
            }
        }
    }

    public void SetEnabled(bool value)
    {
        if (value == isEnabled) return;
        isEnabled = value;
        animator.SetBool("enabled", isEnabled);
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        if (isEnabled && isHovered && context.canceled)
        {
            animator.SetTrigger("Click");
            onClick?.Invoke();
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
