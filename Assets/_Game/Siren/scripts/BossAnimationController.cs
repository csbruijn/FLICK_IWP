using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Animator Trigger Names")]
    [SerializeField] private string verticalTrigger = "Vertical";
    [SerializeField] private string horizontalTrigger = "Horizontal";

    private int verticalHash;
    private int horizontalHash;

    private void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
        verticalHash = Animator.StringToHash(verticalTrigger);
        horizontalHash = Animator.StringToHash(horizontalTrigger);
    }

    public void PlayVertical()
    {
        animator.ResetTrigger(horizontalHash);
        animator.SetTrigger(verticalHash);
    }

    public void PlayHorizontal()
    {
        animator.ResetTrigger(verticalHash);
        animator.SetTrigger(horizontalHash);
    }
}
