using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaAtrasar : MonoBehaviour
{
    private Collider2D col;
    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mae")
        {
            StartCoroutine(TimeToPass());
        }
    }        
    IEnumerator TimeToPass()
    {
        yield return new WaitForSeconds(2f);
        col.gameObject.SetActive(false);
    }
}
