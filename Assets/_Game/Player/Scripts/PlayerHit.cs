using System.Collections;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red;
    public float flashTime = 0.2f;

    private Color _originalColor;

    private void Awake()
    {
        _originalColor = spriteRenderer.color;
    }

    public void FlashRed()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.color = _originalColor;
    }
}
