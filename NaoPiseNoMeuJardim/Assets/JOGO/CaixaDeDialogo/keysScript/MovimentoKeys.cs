using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoKeys : MonoBehaviour
{
    public float speed = 0.4f; 
    public float distance = 0.1f;

    private Vector3 startPosition;
    private bool movingUp = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float movement = Mathf.PingPong(Time.time * speed, distance * 2) - distance;
        transform.position = startPosition + new Vector3(0, movement, 0);
    }
}
