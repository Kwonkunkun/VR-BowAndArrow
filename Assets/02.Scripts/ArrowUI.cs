using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowUI : MonoBehaviour
{
    //
    public GameObject video1;
    public GameObject video2;
    public GameObject video3;
    public void OnVideo1()
    {
        video1.SetActive(true);
        StartCoroutine("offvideo");
    }

    public void OnVideo2()
    {
        video2.SetActive(true);
        StartCoroutine("offvideo");
    }
    public void OnVideo3()
    {
        video3.SetActive(true);
        StartCoroutine("offvideo");
    }

    IEnumerator offvideo()
    {
        yield return new WaitForSeconds(5);
        video1.SetActive(false);
        video2.SetActive(false);
        video3.SetActive(false);
    }


}
