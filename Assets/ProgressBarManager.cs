using UnityEngine;

public class ProgressBarManager : MonoBehaviour
{
    [SerializeField] private ProgressBar smallProgressBar;
    [SerializeField] private ProgressBar bigProgressBar;

    public void AnimateTo(float percent)
    {
        smallProgressBar.AnimateTo(percent);
        bigProgressBar.AnimateTo(percent);
    }

    public void SetPercent(float percent)
    {
        smallProgressBar.SetPercent(percent);
        bigProgressBar.SetPercent(percent);
    }

    public void ShowSmall()
    {
        smallProgressBar.Show();
    }

    public void ShowBig()
    {
        bigProgressBar.Show();
    }

    public void Hide()
    {
        smallProgressBar.Hide();
        bigProgressBar.Hide();
    }
}
