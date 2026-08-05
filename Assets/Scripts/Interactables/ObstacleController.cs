using System.Collections;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private ParticleSystem unlockParticles; 

    public void Unlock()
    {
        StartCoroutine(FadeAndDisable());
    }

    private IEnumerator FadeAndDisable()
    {
        if (unlockParticles != null) unlockParticles.Play();

        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            foreach (var r in renderers)
            {
                Color c = r.color;
                c.a = alpha;
                r.color = c;
            }
            yield return null;
        }

        GetComponent<Collider2D>().enabled = false; 
        gameObject.SetActive(false); 
    }
}