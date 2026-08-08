using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField] private float hoverHeight = 0.15f;
    [SerializeField] private float hoverSpeed = 2f;

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

        transform.position = startPosition + Vector3.up * offset;
    }
}
