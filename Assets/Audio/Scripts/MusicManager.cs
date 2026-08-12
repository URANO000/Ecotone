using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;

    [Header("Mixer (control de volumen)")]
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    private const string MUSIC_VOLUME_PARAM = "MusicVolume";
    private const string MUSIC_VOLUME_PREF_KEY = "MusicVolume";

    [Header("Música de pausa")]
    [SerializeField] private AudioClip pauseMenuMusic;
    [SerializeField, Range(0f, 1f)] private float pauseMenuVolume = 0.6f;
    private AudioClip clipBeforePause;
    private float timeBeforePause;
    private bool isPausedMusic;
    private float currentPreviewVolume = 1f;
    private float volumeBeforePause;
    private bool hasPausedTrack;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource.loop = true;
        musicSource.outputAudioMixerGroup = musicMixerGroup;
    }

    private void Start()
    {
        PreviewMusicVolume(GetSavedMusicVolume());
    }

    public void PlayTrack(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.volume = Mathf.Clamp01(volume);
        musicSource.Play();
    }

    public void PreviewMusicVolume(float normalizedVolume)
    {
        normalizedVolume = Mathf.Clamp(normalizedVolume, 0.0001f, 1f);
        currentPreviewVolume = normalizedVolume;
        float dB = Mathf.Log10(normalizedVolume) * 20f;
        musicMixer.SetFloat(MUSIC_VOLUME_PARAM, dB);
    }
    public void CommitMusicVolume()
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_PREF_KEY, currentPreviewVolume);
    }

    public float RevertMusicVolume()
    {
        float saved = GetSavedMusicVolume();
        PreviewMusicVolume(saved);
        return saved;
    }

    public float GetSavedMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_PREF_KEY, 1f);
    }

    public void PlayPauseMenuMusic()
    {
        if (isPausedMusic) return;
        isPausedMusic = true;

        clipBeforePause = musicSource.clip;
        timeBeforePause = musicSource.time;
        volumeBeforePause = musicSource.volume;

        if (pauseMenuMusic == null) return; 

        musicSource.clip = pauseMenuMusic;
        musicSource.volume = pauseMenuVolume;
        musicSource.time = 0f;
        musicSource.Play();
    }

    public void ResumeZoneMusic()
    {
        if (!isPausedMusic) return;
        isPausedMusic = false;

        if (clipBeforePause == null) return;

        musicSource.clip = clipBeforePause;
        musicSource.volume = volumeBeforePause;
        musicSource.time = Mathf.Min(timeBeforePause, Mathf.Max(0f, clipBeforePause.length - 0.05f));
        musicSource.Play();
    }

    public void PauseZoneMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Pause();
    }

    public void UnpauseZoneMusic()
    {
        if (!musicSource.isPlaying)
            musicSource.UnPause();
    }
}