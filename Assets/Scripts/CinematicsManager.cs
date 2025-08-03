using DG.Tweening;
using System;
using UnityEngine;

public class CinematicsManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup startCinematic;
    [SerializeField] private CanvasGroup startCinematicImage;
    [SerializeField] private CanvasGroup endCinematic;
    [SerializeField] private TextButton endCinematicContinue;
    [SerializeField] private TextButton endCinematicExit;

    private Action endCinematicCallback = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        startCinematic.alpha = 1f;
        startCinematic.blocksRaycasts = true;
        startCinematicImage.alpha = 0f;
        endCinematic.alpha = 0f;
        endCinematic.blocksRaycasts = false;

        endCinematicContinue.onClick += HideEndCinematic;
        endCinematicExit.onClick += QuitGame;
    }

    public void ShowStartCinematic(Action callback)
    {
        startCinematic.blocksRaycasts = true;

        var sequence = DOTween.Sequence();
        sequence.AppendInterval(1f);
        sequence.Append(startCinematicImage.DOFade(1f, 1f));
        sequence.AppendInterval(4f);
        sequence.Append(startCinematic.DOFade(0f, 1f));
        sequence.AppendCallback(() =>
        {
            startCinematic.blocksRaycasts = false;
            startCinematicImage.alpha = 0f;
            callback?.Invoke();
        });
    }

    public void ShowEndCinematic(Action callback)
    {
        endCinematic.blocksRaycasts = true;
        endCinematic.DOFade(1f, 1f);
        endCinematicCallback = callback;
    }

    public void HideEndCinematic()
    {
        endCinematic.DOFade(0f, 1f).OnComplete(() =>
        {
            endCinematic.blocksRaycasts = false;
            if (endCinematicCallback != null)
            {
                endCinematicCallback?.Invoke();
                endCinematicCallback = null;
            }
        });
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        if (endCinematicContinue != null)
        {
            endCinematicContinue.onClick -= HideEndCinematic;
        }
        endCinematicExit.onClick -= QuitGame;
    }
}
