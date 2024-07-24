using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptMae : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveRange = 0.1f; 
    private Vector3 startPosition; 

    void Start()
    {

        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * moveSpeed) * moveRange;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
