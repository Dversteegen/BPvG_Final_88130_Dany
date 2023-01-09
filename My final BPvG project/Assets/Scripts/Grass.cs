using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grass : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        CheckPlayer();
    //    }
    //}

    //private void CheckPlayer()
    //{
    //    Debug.Log("Appel");
    //    float encounter = Random.Range(0, 7);
    //    if (encounter == 5)
    //    {
    //        SceneManager.LoadScene("BattleScene");
    //    }
    //}
}
