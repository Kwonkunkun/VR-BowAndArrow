using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OTarget : MonoBehaviour
{
    //O면 0, X면 1
    public bool rightAndFalse;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Tip"))
        {
            Debug.Log("Arow in Target : " + rightAndFalse.ToString());
            QuizManager.instance.UpdateScore(rightAndFalse);
        }
    }
}
