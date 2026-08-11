using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [Header("Mixer (control de volumen)")]
    [SerializeField] private AudioMixer sfxMixer;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    private const string SFX_VOLUME_PARAM = "SFXVolume";
    private const string SFX_VOLUME_PREF_KEY = "SFXVolume";

    private float currentPreviewVolume = 1f;


    public AudioMixerGroup SfxMixerGroup => sfxMixerGroup;

    [Header("Player - Ataque")]
    [SerializeField] private AudioClip[] attackClips;   

    [Header("Player - Daño recibido (DR)")]
    [SerializeField] private AudioClip[] damageClips;   

    [Header("Player - Salto")]
    [SerializeField] private AudioClip[] jumpClips;    

    [Header("Player - Pasos en césped")]
    [SerializeField] private AudioClip[] grassClips;   

    [Header("Boss: Serpiente")]
    [SerializeField] private AudioClip snakeHiss;      
    [SerializeField] private AudioClip snakeAttack;    

    [Header("Boss: Oso (Trif)")]
    [SerializeField] private AudioClip trifGrowlCalm;  
    [SerializeField] private AudioClip trifGrowlAttack; 

    [Header("Configuración de reproducción")]
    [SerializeField] private int maxOneShotSources = 6;
    [SerializeField, Range(0f, 0.2f)] private float pitchRandomRange = 0.05f;

    private AudioSource[] oneShotPool;
    private int poolIndex;

    private int lastAttackIndex = -1;
    private int lastDamageIndex = -1;
    private int lastGrassIndex = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        oneShotPool = new AudioSource[maxOneShotSources];
        for (int i = 0; i < maxOneShotSources; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.spatialBlend = 0f; 
            src.outputAudioMixerGroup = sfxMixerGroup;
            oneShotPool[i] = src;
        }
    }

    private void Start()
    {
        float saved = GetSavedSFXVolume();
        PreviewSFXVolume(saved);
    }

    public void PreviewSFXVolume(float normalizedVolume)
    {
        normalizedVolume = Mathf.Clamp(normalizedVolume, 0.0001f, 1f);
        currentPreviewVolume = normalizedVolume;
        float dB = Mathf.Log10(normalizedVolume) * 20f;
        sfxMixer.SetFloat(SFX_VOLUME_PARAM, dB);
    }

    public void CommitSFXVolume()
    {
        PlayerPrefs.SetFloat(SFX_VOLUME_PREF_KEY, currentPreviewVolume);
    }

    public float RevertSFXVolume()
    {
        float saved = GetSavedSFXVolume();
        PreviewSFXVolume(saved);
        return saved;
    }
    public float GetSavedSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME_PREF_KEY, 1f);
    }

    private AudioSource GetNextSource()
    {
        var src = oneShotPool[poolIndex];
        poolIndex = (poolIndex + 1) % oneShotPool.Length;
        return src;
    }

    private void PlayOneShotWithPitch(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        var src = GetNextSource();
        src.pitch = 1f + Random.Range(-pitchRandomRange, pitchRandomRange);
        src.PlayOneShot(clip, volume);
    }
    private int GetRandomIndexNoRepeat(int arrayLength, int lastIndex)
    {
        if (arrayLength <= 1) return 0;
        int index;
        do
        {
            index = Random.Range(0, arrayLength);
        } while (index == lastIndex);
        return index;
    }


    public void PlayAttack()
    {
        if (attackClips == null || attackClips.Length == 0) return;
        lastAttackIndex = GetRandomIndexNoRepeat(attackClips.Length, lastAttackIndex);
        PlayOneShotWithPitch(attackClips[lastAttackIndex]);
    }

    public void PlayDamage()
    {
        if (damageClips == null || damageClips.Length == 0) return;
        lastDamageIndex = GetRandomIndexNoRepeat(damageClips.Length, lastDamageIndex);
        PlayOneShotWithPitch(damageClips[lastDamageIndex]);
    }

    public void PlayJump()
    {
        if (jumpClips == null || jumpClips.Length == 0) return;
        int index = Random.Range(0, jumpClips.Length);
        PlayOneShotWithPitch(jumpClips[index]);
    }

    public void PlayGrassStep()
    {
        if (grassClips == null || grassClips.Length == 0) return;
        lastGrassIndex = GetRandomIndexNoRepeat(grassClips.Length, lastGrassIndex);
        PlayOneShotWithPitch(grassClips[lastGrassIndex], 0.6f);
    }


    public void PlaySnakeAttack() => PlayOneShotWithPitch(snakeAttack);

    public AudioClip GetSnakeHissClip() => snakeHiss;

    public void PlayTrifGrowlCalm() => PlayOneShotWithPitch(trifGrowlCalm);

    public void PlayTrifGrowlAttack() => PlayOneShotWithPitch(trifGrowlAttack);
}