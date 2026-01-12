using System.Collections;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Animator Trigger Names")]
    [SerializeField] private string verticalTrigger = "Vertical";
    [SerializeField] private string horizontalTrigger = "Horizontal";

    [SerializeField] private float VerticalStartDelay = 0;
    [SerializeField] private float HorizontalStartDelay = 0;

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
        StartCoroutine(DelayedStartVert()); 
    }

    public void PlayHorizontal()
    {
        StartCoroutine(DelayedStartHor());
    }

    private IEnumerator DelayedStartVert()
    {

        yield return new WaitForSeconds(VerticalStartDelay);
        animator.ResetTrigger(horizontalHash);
        animator.SetTrigger(verticalHash);
    }

    private IEnumerator DelayedStartHor()
    {
        yield return new WaitForSeconds(HorizontalStartDelay);
        animator.ResetTrigger(verticalHash);
        animator.SetTrigger(horizontalHash);
    }

}
