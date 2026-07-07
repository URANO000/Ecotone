using UnityEngine;

public class MusicZone : MonoBehaviour
{
    [SerializeField] private AudioClip zoneClip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        MusicManager.Instance.PlayTrack(zoneClip);
    }
}