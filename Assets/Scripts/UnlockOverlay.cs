using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CanvasGroup))]
public class UnlockOverlay : MonoBehaviour
{
    private Image overlayImage;
    private CanvasGroup canvasGroup;
    private GameObject spinningCleanerInstance;
    [SerializeField] private AudioSource levelUpAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        overlayImage = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(GamePhase phase)
    {
        overlayImage.sprite = phase.unlockOverlay;
        spinningCleanerInstance = Instantiate(phase.spinningCleanerPrefab, Vector3.zero, Quaternion.identity);
        canvasGroup.DOFade(1f, 0.5f);
        levelUpAudio.Play();
    }

    public void Hide()
    {
        canvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
        {
            if (spinningCleanerInstance != null)
            {
                Destroy(spinningCleanerInstance);
                spinningCleanerInstance = null;
            }
        });
    }
}
