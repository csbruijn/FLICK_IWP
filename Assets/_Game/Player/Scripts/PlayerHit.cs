using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red;
    public float redDuration = 2f;   // how long they stay red

    private Color _originalColor;
    private float _redTimer = 0f;

    private void Awake()
    {
        _originalColor = spriteRenderer.color;
    }

    // Called by your GameEventListener when player is hit
    public void TurnRed()
    {
        spriteRenderer.color = hitColor;
        _redTimer = redDuration;
    }

    private void Update()
    {
        // Count down while player is red
        if (_redTimer > 0f)
        {
            _redTimer -= Time.deltaTime;

            // If time is up, revert color
            if (_redTimer <= 0f)
            {
                spriteRenderer.color = _originalColor;
            }
        }
    }
}
