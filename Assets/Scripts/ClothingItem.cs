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
    [Space]
    [SerializeField] private GameObject smallLabelCanvas;
    [SerializeField] private TextMeshProUGUI brandText;
    [SerializeField] private Image materialIcon;
    [SerializeField] private Image extraImage1;
    [SerializeField] private Image extraImage2;
    [Space]
    [SerializeField] private int meshMaterialIndex = 0;
    [HideInInspector] public ModifierType modifier;
    [HideInInspector] public GameMaterial gameMaterial;
    public ClothesManager clothesManager;
    [SerializeField] private Outline outline;
    [Space]
    [SerializeField] private GameObject largeLabelCanvas;
    [SerializeField] private Image largeMaterialIcon;
    [SerializeField] private TextMeshProUGUI errorText;
    [Space]
    [SerializeField] private GameObject dirtySpotCanvas;

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
        meshRenderer.material = gameMaterial.materials[meshMaterialIndex];
        materialIcon.sprite = gameMaterial.icon;
        largeMaterialIcon.sprite = gameMaterial.icon;
        largeLabelCanvas.gameObject.SetActive(false);

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

        // Animate in
        transform.position = new Vector3(0, 0, 50f);
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutExpo).SetDelay(clothesManager.Clothes.Length * 0.2f);
        transform.DOLocalMove(Vector3.zero, 0.8f).SetEase(Ease.OutExpo).SetDelay(clothesManager.Clothes.Length * 0.2f);
    }

    void Update()
    {
        if (clothesManager == null) return;
        if (clothesManager.clothesState == ClothesState.CanBeSelected && PlayerInputHook.Instance.raycastHit.transform == transform)
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
        if (clothesManager == null) return;
        if (clothesManager.clothesState == ClothesState.CanBeSelected && isHovered && context.canceled)
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
            transform.DOScale(Vector3.one * 1.1f, 0.15f).SetEase(Ease.OutBack);
            DOTween.To(() => outline.OutlineColor.a, x => outline.OutlineColor = new Color(1, 1, 1, x), 1f, 0.15f);
            clothesManager.HandleSelectItem(this);
        }
        else
        {
            transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack);
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

    public void Hide()
    {
        transform.DOLocalMoveY(-10f, 0.5f).SetEase(Ease.InExpo);
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InExpo);
    }

    public void Show()
    {
        if (clothesManager == null || clothesManager.clothesState == ClothesState.CanBeSelected)
        {
            meshRenderer.enabled = true;
            smallLabelCanvas.SetActive(true);
            dirtySpotCanvas.SetActive(true);

            largeLabelCanvas.SetActive(false);
        }
        else
        {
            if (string.IsNullOrEmpty(errorMessage))
            {
                meshRenderer.enabled = true;
                smallLabelCanvas.SetActive(true);
                // TODO: sparkles particle system

                largeLabelCanvas.SetActive(false);
            }
            else
            {
                errorText.text = errorMessage;
                largeLabelCanvas.SetActive(true);

                meshRenderer.enabled = false;
                smallLabelCanvas.SetActive(false);
            }

            dirtySpotCanvas.SetActive(false);
        }
        transform.DOLocalMoveY(0f, 0.5f).SetEase(Ease.OutExpo);
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
    }

    public void Discard()
    {
        var sequence = DOTween.Sequence();
        sequence.Insert(0f, transform.DOMove(new Vector3(0, 0, 50f), 0.8f).SetEase(Ease.InExpo));
        sequence.Insert(0f, transform.DOScale(Vector3.zero, 0.8f).SetEase(Ease.InExpo));
        //sequence.AppendCallback(() => Destroy(gameObject));
    }

    private void OnDestroy()
    {
        if (PlayerInputHook.Instance != null)
        {
            PlayerInputHook.Instance.ClickEvent -= OnClick;
        }
    }
}