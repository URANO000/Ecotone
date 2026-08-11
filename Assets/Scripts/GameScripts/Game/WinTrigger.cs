using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (!other.CompareTag("Player")) return;

        hasTriggered = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.WinLevel();
        }
    }
}