using UnityEngine;

public class MusicZone : MonoBehaviour
{
    [SerializeField] private AudioClip zoneClip;
    [SerializeField][Range(0f, 1f)] private float zoneVolume = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (MusicManager.Instance == null)
        {
            Debug.LogWarning("MusicManager = null");
            return;
        }

        MusicManager.Instance.PlayTrack(zoneClip, zoneVolume);
    }
}