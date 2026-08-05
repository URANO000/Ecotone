using UnityEngine;

public class PuzzleZoneTrigger : MonoBehaviour
{
    [Header("Referencias de la zona")]
    [SerializeField] private CameraFocusController cameraController;
    [SerializeField] private Transform focusPoint;
    [SerializeField] private GameObject interactableZone;

    [Header("Tiempo de introducción")]
    [SerializeField] private float introHoldDuration = 1.5f;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        cameraController.FocusOnTarget(focusPoint, onComplete: () =>
        {
            interactableZone.SetActive(true);
            StartCoroutine(ReturnAfterDelay());
        });

        GetComponent<Collider2D>().enabled = false;
    }

    private System.Collections.IEnumerator ReturnAfterDelay()
    {
        yield return new WaitForSecondsRealtime(introHoldDuration);
        cameraController.ReturnToPlayer();
    }
}