//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PrenderPersonagem : MonoBehaviour
//{

//    private ScriptPersonagem player;
//    public bool podeSeMover;

//    private void Start()
//    {
//        player = GetComponent<ScriptPersonagem>();
//    }

//    public void Update()
//    {
//        if (podeSeMover = false)
//        {
//            player.speed = 0f;
//        }
//    }

//    public void OnTri/*g*/gerEnter2D(Collider2D collision)
//    {
//        if(collision.gameObject.tag == "Player")
//        {
//            Debug.Log("taPreso");
//            podeSeMover = false;
//        }
//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.gameObject.tag == "Player")
//        {

//            Debug.Log("Ta solto");
//            podeSeMover = true;
//        }
//    }
//}
