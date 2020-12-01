using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OXTarget : MonoBehaviour
{
    //O면 0, X면 1
    public bool rightAndFalse;
    public bool isShot;
    public GameObject hit_effect_prefab;

    private void Update()
    {
        if(isShot == true)
        {
            QuizManager.instance.UpdateScore(rightAndFalse);
            isShot = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("OnCollisionEnter");
        if (other.collider.tag == "Tip")
        {
            isShot = true;
            Debug.Log("in Tip OnCollisionEnter");
            //만약 화살이면
            Vector3 pos = other.contacts[0].point;
            //접촉한 부위의 위치에 파티클을 뿌려준다.
            GameObject hit_effect = Instantiate(hit_effect_prefab, pos, Quaternion.identity);
            other.gameObject.GetComponent<Arrow>().Stop();
            Destroy(hit_effect, 0.5f);
            Destroy(other.gameObject);
        }
    }
}
