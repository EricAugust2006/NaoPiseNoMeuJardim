//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Chinelo : MonoBehaviour
//{
//    public float followDuration = 3f;
//    public float speed = 5f;
//    private Transform target;
//    private bool isFollowing = true;

//    public void Init(Transform targetPlayer)
//    {
//        target = targetPlayer;
//        StartCoroutine(FollowPlayer());
//    }


//    private IEnumerator FollowPlayer()
//    {
//        // Segue o jogador por um tempo determinado (followDuration)
//        yield return new WaitForSeconds(followDuration);
//        isFollowing = false; // Depois de algum tempo, para de seguir o jogador
//    }

//    void Update()
//    {
//        if (isFollowing && target != null)
//        {
//            // Enquanto estiver seguindo, persegue o jogador
//            Vector2 direction = (target.position - transform.position).normalized;
//            transform.position += (Vector3)direction * speed * Time.deltaTime;
//        }
//        else
//        {
//            // Quando parar de seguir, o chinelo continua em linha reta
//            transform.position += transform.right * speed * Time.deltaTime;
//        }
//    }
//}
