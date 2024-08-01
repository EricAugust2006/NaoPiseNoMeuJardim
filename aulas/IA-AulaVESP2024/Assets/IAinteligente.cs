using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAinteligente : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent agent;
    public List<GameObject> Destinos;
    private Vector3 localDestinado;
    public int IndiceLocal = 0;
    //public GameObject Mensagem;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        localDestinado = Destinos[IndiceLocal].transform.position;
    }

    private void Update()
    {
        agent.SetDestination(localDestinado);
        if(Vector3.Distance(transform.position, localDestinado) < 5)
        {
            IndiceLocal++;

            if (IndiceLocal >= Destinos.Count)
            {
                IndiceLocal = 0;
            }

            localDestinado = Destinos[IndiceLocal].transform.position;
        }

    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.tag == "Player")
    //    {
    //        Mensagem.SetActive(true);
    //    }
    //}
}
