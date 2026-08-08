using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb; 

    public void OnAttack()
    {
        SFXManager.Instance.PlayAttack();
    }
    public void OnDamageReceived()
    {
        SFXManager.Instance.PlayDamage();
    }

    public void OnJump()
    {
        SFXManager.Instance.PlayJump();
    }

    public void AnimEvent_Footstep()
    {
        SFXManager.Instance.PlayGrassStep();
    }
}