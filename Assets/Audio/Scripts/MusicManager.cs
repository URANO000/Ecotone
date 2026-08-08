using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayTrack(AudioClip clip, float volume = 1f)
    {
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.volume = Mathf.Clamp01(volume);
        musicSource.Play();
    }
}