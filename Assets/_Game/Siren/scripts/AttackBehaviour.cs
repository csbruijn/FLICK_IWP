using Unity.VisualScripting;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private GameEvent onFiveSecondsPassed;
    [SerializeField] private GameEvent onTenSecondsPassed;

    [SerializeField] private float verticalAttackInterval = 5f;
    [SerializeField] private float horizontalAttackInterval = 10f;
    [SerializeField] private float startingTime = 60f;

    [Header("Boss animation")]
    [SerializeField] private BossAnimationController bossAnim;

    private float remainingTime;
    private float verticalAttackTimer;
    private float horizontalAttackTimer;

    void Start()
    {
        remainingTime = startingTime;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        remainingTime -= dt;
        verticalAttackTimer += dt;
        horizontalAttackTimer += dt;

        // Vertical attack moment
        if (verticalAttackTimer >= verticalAttackInterval)
        {
            if (bossAnim != null) bossAnim.PlayVertical();

            // keep existing wave/event logic
            onFiveSecondsPassed.Raise(this, remainingTime);

            verticalAttackTimer -= verticalAttackInterval;
        }

        // Horizontal attack moment
        if (horizontalAttackTimer >= horizontalAttackInterval)
        {
            if (bossAnim != null) bossAnim.PlayHorizontal();

            // keep existing wave/event logic
            onTenSecondsPassed.Raise(this, remainingTime);

            horizontalAttackTimer -= horizontalAttackInterval;
        }
    }
}
