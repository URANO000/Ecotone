using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PanelMusicOnEnable : MonoBehaviour
{
    [SerializeField] private bool pauseBackgroundMusic = true;

    private AudioSource audioSource;

    private void Awake() => audioSource = GetComponent<AudioSource>();

    private void OnEnable()
    {
        audioSource.Play();

        if (pauseBackgroundMusic && MusicManager.Instance != null)
            MusicManager.Instance.PauseZoneMusic();
    }

    private void OnDisable()
    {
        audioSource.Stop();

        if (pauseBackgroundMusic && MusicManager.Instance != null)
            MusicManager.Instance.UnpauseZoneMusic();
    }
}