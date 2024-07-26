using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAinteligente : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent agent;
    public GameObject Mensagem;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.SetDestination(target.transform.position);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Mensagem.SetActive(true);
        }
    }
}
