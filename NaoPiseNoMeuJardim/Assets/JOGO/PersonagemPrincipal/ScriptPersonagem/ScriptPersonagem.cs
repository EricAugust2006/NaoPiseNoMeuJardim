using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ScriptPersonagem : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        float VelX = Input.GetAxis("Horizontal");

        Vector3 Movement = new Vector3(VelX, 0f, 0f);
        transform.position += Movement * Time.deltaTime * speed;
    }
}