using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(Animator))]
public class CleanerBottle : MonoBehaviour
{
    private Outline outline;
    private Animator animator;
    public int index = 0;
    private bool isSelected = false;
    private bool isHovered = false;
    [SerializeField] private CleanerPicker cleanerPicker;
    public bool IsSelected => isSelected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        outline = GetComponent<Outline>();
        animator = GetComponent<Animator>();
        if (PlayerInputHook.Instance != null)
        {
            PlayerInputHook.Instance.ClickEvent += OnClick;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cleanerPicker.canInteract && PlayerInputHook.Instance.raycastHit.transform == transform)
        {
            if (!isHovered)
            {
                isHovered = true;
                if (!isSelected)
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
                if (!isSelected)
                {
                    DOTween.To(() => outline.OutlineColor.a, x => outline.OutlineColor = new Color(1, 1, 1, x), 0f, 0.15f);
                }
            }
        }
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        if (cleanerPicker.canInteract && isHovered && context.canceled)
        {
            SetSelected(!isSelected);
        }
    }

    public void SetSelected(bool value)
    {
        if (isSelected == value) return;
        isSelected = value;
        if (isSelected)
        {
            animator.SetBool("selected", true);
            DOTween.To(() => outline.OutlineColor.a, x => outline.OutlineColor = new Color(1, 1, 1, x), 1f, 0.15f);
            cleanerPicker.HandleSelectBottle(this);
        }
        else
        {
            animator.SetBool("selected", false);
            if (isHovered)
            {
                DOTween.To(() => outline.OutlineColor.a, x => outline.OutlineColor = new Color(1, 1, 1, x), 0.2f, 0.15f);
            }
            else
            {
                DOTween.To(() => outline.OutlineColor.a, x => outline.OutlineColor = new Color(1, 1, 1, x), 0f, 0.15f);
            }
            cleanerPicker.HandleDeselectBottle(this);
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
