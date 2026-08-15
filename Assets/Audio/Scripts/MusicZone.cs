using UnityEngine;

public class MusicZone : MonoBehaviour
{
    [SerializeField] private AudioClip zoneClip;
    [SerializeField][Range(0f, 1f)] private float zoneVolume = 1f;
    [SerializeField] private Collider2D zoneCollider;

    private void Awake()
    {
        if (zoneCollider == null)
            zoneCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        CheckIfPlayerAlreadyInside();
    }

    private void CheckIfPlayerAlreadyInside()
    {
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        Collider2D[] overlaps = new Collider2D[8];
        int count = Physics2D.OverlapCollider(zoneCollider, filter, overlaps);

        for (int i = 0; i < count; i++)
        {
            if (overlaps[i] != null && overlaps[i].CompareTag("Player"))
            {
                PlayZoneMusic();
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        PlayZoneMusic();
    }

    private void PlayZoneMusic()
    {
        if (MusicManager.Instance == null)
        {
            Debug.LogWarning("MusicManager = null");
            return;
        }
        MusicManager.Instance.PlayTrack(zoneClip, zoneVolume);
    }
}