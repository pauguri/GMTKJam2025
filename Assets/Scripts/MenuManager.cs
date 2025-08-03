using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private RectTransform slidingMenu;
    [SerializeField] private RectTransform mainBubbles;
    [SerializeField] private RectTransform secondaryBubbles;
    [SerializeField] private AudioSource bubblesMenu;
    private bool starting = false;

    private void Start()
    {
        slidingMenu.DOAnchorPosY(1230, 2.5f).SetEase(Ease.InOutSine).SetDelay(3f);
    }

    public void StartGame()
    {
        if (starting) return; // Prevent multiple clicks while starting
        starting = true;
        bubblesMenu.Play();

        var sequence = DOTween.Sequence();
        sequence.Insert(0, mainBubbles.DOAnchorPosY(0, 2f));
        sequence.Insert(0, secondaryBubbles.DOAnchorPosY(0, 2f));
        sequence.AppendCallback(() =>
        {
            SceneManager.LoadScene("MainScene");
        });
    }
}
