using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : MonoBehaviour
{
    public Transform tr_tong;
    public List<Transform> tr_arrow_pos;
    public GameObject arrow_grip;

    public void Reload_Arrow()
    {
        for(int i =0; i<tr_arrow_pos.Count; i++)
        {
            GameObject arrow = Instantiate(arrow_grip, tr_tong);
            arrow.transform.position = tr_arrow_pos[i].position;

            //여기서 게임 초기화 처리
            GameManager.instance.GameInit();
        }

        //리로드 사운드 자리
    }
}
