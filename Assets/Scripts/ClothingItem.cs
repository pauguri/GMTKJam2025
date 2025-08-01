using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ClothingItem : MonoBehaviour
{
    [SerializeField] private TagPlaceholders tagPlaceholders;
    [SerializeField] private Modifiers modifiersData;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private TextMeshProUGUI brandText;
    [SerializeField] private Image materialIcon;
    [SerializeField] private Image extraImage1;
    [SerializeField] private Image extraImage2;
    [HideInInspector] public ModifierType modifier;
    [HideInInspector] public GameMaterial gameMaterial;
    public ClothesManager clothesManager;
    [SerializeField] private Outline outline;

    [HideInInspector] public string errorMessage;
    //private Animator animator;
    private bool isSelected = false;
    private bool isHovered = false;
    public bool IsSelected => isSelected;

    void Start()
    {
        if (PlayerInputHook.Instance != null)
        {
            PlayerInputHook.Instance.ClickEvent += OnClick;
        }
    }

    public void Init(GameMaterial gameMaterial, ModifierType modifierType)
    {
        this.gameMaterial = gameMaterial;
        meshRenderer.material = gameMaterial.material;
        materialIcon.sprite = gameMaterial.icon;

        if (modifierType == ModifierType.None)
        {
            extraImage1.sprite = tagPlaceholders.icons1[Random.Range(0, tagPlaceholders.icons1.Length)];
        }
        else
        {
            extraImage1.sprite = modifiersData.GetModifier(modifierType).icon;
        }

        brandText.text = tagPlaceholders.brands[Random.Range(0, tagPlaceholders.brands.Length)];
        extraImage2.sprite = tagPlaceholders.icons2[Random.Range(0, tagPlaceholders.icons2.Length)];
    }

    void Update()
    {
        if (PlayerInputHook.Instance.raycastHit.transform == transform)
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
        if (isHovered && context.canceled)
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
            //animator.SetBool("selected", true);
            DOTween.To(() => outline.OutlineColor.a, x => outline.OutlineColor = new Color(1, 1, 1, x), 1f, 0.15f);
            clothesManager.HandleSelectItem(this);
        }
        else
        {
            //animator.SetBool("selected", false);
            if (isHovered)
            {
                DOTween.To(() => outline.OutlineColor.a, x => outline.OutlineColor = new Color(1, 1, 1, x), 0.2f, 0.15f);
            }
            else
            {
                DOTween.To(() => outline.OutlineColor.a, x => outline.OutlineColor = new Color(1, 1, 1, x), 0f, 0.15f);
            }
            clothesManager.HandleDeselectItem(this);
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