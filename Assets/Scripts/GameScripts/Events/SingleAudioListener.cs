using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAudioListener : MonoBehaviour
{
    private void Awake()
    {
        var listeners = FindObjectsOfType<AudioListener>();
        if (listeners.Length > 1)
        {
            GetComponent<AudioListener>().enabled = false;
        }
    }
}
