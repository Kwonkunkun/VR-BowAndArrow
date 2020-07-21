using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    //타격 이펙트
    public GameObject hit_effect_prefab;
    public AK.Wwise.Event HitSoundEvent;

    void Start()
    {
        //프리팹 로드
        // hit_effect_prefab=Resources.Load<GameObject>("EnergyNovaBlue");
    }


    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("OnCollisionEnter");
        //만약 화살이면
        if (other.collider.tag == "Tip")
        {
            Debug.Log("arrow in Target");
            GameManager.instance.UpdateScore(true);
            Vector3 pos = other.contacts[0].point;
            //접촉한 부위의 위치에 파티클을 뿌려준다.
            GameObject hit_effect = Instantiate(hit_effect_prefab, pos, Quaternion.identity);
            other.gameObject.GetComponent<Arrow>().Stop();
            HitSoundEvent.Post(gameObject);
            Destroy(hit_effect, 0.5f);
            Destroy(other.gameObject);
        }
        //Hit(other.contacts[0].point);
    }
}
