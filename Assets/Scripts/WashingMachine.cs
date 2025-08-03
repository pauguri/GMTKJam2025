using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WashingMachine : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AudioSource washAudio;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Wash()
    {
        animator.SetTrigger("Run");
        DOVirtual.DelayedCall(0.5f, () =>
        {
            if (washAudio != null)
            {
                washAudio.Play();
            }
        });
    }
}
