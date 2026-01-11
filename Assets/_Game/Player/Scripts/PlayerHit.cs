using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red;
    [SerializeField] private float _timeOutDuration = 2f;   // how long they stay red

    private Color _originalColor;
    private float _hitTimeOut = 0f;

    private void Awake()
    {
        _originalColor = spriteRenderer.color;
    }

    // Called by GameEventListener when player is hit
    public void RegisterHit()
    {
        spriteRenderer.color = hitColor;
        _hitTimeOut = _timeOutDuration;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hitTimeOut > 0f) return;

        if (other.CompareTag("Hazard"))
        {
            //show visual
            RegisterHit();

            //damage the player
            GetComponent<PlayerStatus>().DamagePlayer();
        }
    }

    private void Update()
    {
        // Count down while player hit
        if (_hitTimeOut > 0f)
        {
            _hitTimeOut -= Time.deltaTime;

            // If time is up, revert color
            if (_hitTimeOut <= 0f)
            {
                spriteRenderer.color = _originalColor;
            }
        }
    }
}
