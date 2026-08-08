using UnityEngine;

public class TrifBossAudio : MonoBehaviour
{
    [Header("Growl calmado (idle)")]
    [SerializeField] private float minTimeBetweenGrowls = 4f;
    [SerializeField] private float maxTimeBetweenGrowls = 9f;
    [SerializeField] private bool growlLoopActive = true;

    private float nextGrowlTime;

    private void Start()
    {
        ScheduleNextGrowl();
    }

    private void Update()
    {
        if (!growlLoopActive) return;

        if (Time.time >= nextGrowlTime)
        {
            SFXManager.Instance.PlayTrifGrowlCalm();
            ScheduleNextGrowl();
        }
    }

    private void ScheduleNextGrowl()
    {
        nextGrowlTime = Time.time + Random.Range(minTimeBetweenGrowls, maxTimeBetweenGrowls);
    }

    public void OnTrifAttack()
    {
        growlLoopActive = false; 
        SFXManager.Instance.PlayTrifGrowlAttack();
    }

    public void OnReturnToCalm()
    {
        growlLoopActive = true;
        ScheduleNextGrowl();
    }
}