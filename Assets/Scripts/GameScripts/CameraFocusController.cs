using System;
using System.Collections;
using UnityEngine;

public class CameraFocusController : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float panDuration = 1f;
    [SerializeField] private float focusZoom = 3.5f;
    [SerializeField] private float defaultZoom = 5f;
    [SerializeField] private Transform player;

    private Camera cam;
    private Vector3 originalPosition;
    private Coroutine activeRoutine;


    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void FocusOnTarget(Transform target, Action onComplete = null)
    {
        GetComponent<CameraFollow>().enabled = false;
        if (activeRoutine != null) StopCoroutine(activeRoutine);
        activeRoutine = StartCoroutine(PanRoutine(target.position, focusZoom, onComplete));
    }

    public void ReturnToPlayer(Action onComplete = null)
    {
        if (activeRoutine != null) StopCoroutine(activeRoutine);
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);
        activeRoutine = StartCoroutine(PanRoutine(targetPos, defaultZoom, onComplete, followPlayer: true));
    }

    private IEnumerator PanRoutine(Vector3 targetPos, float targetZoom, Action onComplete, bool followPlayer = false)
    {
        targetPos.z = transform.position.z; 
        Vector3 startPos = transform.position;
        float startZoom = cam.orthographicSize;
        float t = 0f;

        while (t < panDuration)
        {
            t += Time.unscaledDeltaTime; 
            float progress = Mathf.SmoothStep(0f, 1f, t / panDuration);
            transform.position = Vector3.Lerp(startPos, targetPos, progress);
            cam.orthographicSize = Mathf.Lerp(startZoom, targetZoom, progress);
            yield return null;
        }

        transform.position = targetPos;
        cam.orthographicSize = targetZoom;
        onComplete?.Invoke();

        if (followPlayer)
        {
            GetComponent<CameraFollow>().enabled = true;
        }

    }
}