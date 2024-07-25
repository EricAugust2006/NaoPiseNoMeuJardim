using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScriptMae : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveRange = 0.1f; 
    private Vector3 startPosition;
    private bool perseguir = false;


    [SerializeField] Transform target;
    public float speed = 5;

    void Start()
    {

        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * moveSpeed) * moveRange;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (perseguir)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    public void PerseguirFilho()
    {
        Debug.Log("Perseguicao");
        perseguir = true;
    }
}
