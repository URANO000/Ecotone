using UnityEngine;
using UnityEngine.EventSystems;

public class SingleEventSystem : MonoBehaviour
{
    private void Awake()
    {
        var systems = FindObjectsOfType<EventSystem>();
        if (systems.Length > 1)
        {
            Destroy(gameObject);
        }
    }
}