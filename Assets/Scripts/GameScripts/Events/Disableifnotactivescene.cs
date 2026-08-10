using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Disableifnotactivescene : MonoBehaviour
{
    private void Awake()
    {
        if (gameObject.scene != SceneManager.GetActiveScene())
        {
            gameObject.SetActive(false);
        }
    }
}
