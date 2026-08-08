using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SnakeBossAudio : MonoBehaviour
{
    private AudioSource loopSource;

    [SerializeField, Range(0f, 1f)] private float hissVolume = 0.5f;

    private void Awake()
    {
        loopSource = GetComponent<AudioSource>();
        loopSource.loop = true;
        loopSource.playOnAwake = false;
        loopSource.spatialBlend = 1f;
    }

    private void Start()
    {
        loopSource.outputAudioMixerGroup = SFXManager.Instance.SfxMixerGroup;
        StartHiss();
    }

    public void StartHiss()
    {
        loopSource.clip = SFXManager.Instance.GetSnakeHissClip();
        loopSource.volume = hissVolume;
        if (!loopSource.isPlaying)
            loopSource.Play();
    }

    public void StopHiss()
    {
        loopSource.Stop();
    }

    public void OnSnakeAttack()
    {
        SFXManager.Instance.PlaySnakeAttack();
    }
    public void OnReturnToIdle()
    {
        StartHiss();
    }
}