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
        StartCoroutine("offvideo1");
    }

    public void OnVideo2()
    {
        video2.SetActive(true);
        StartCoroutine("offvideo2");
    }
    public void OnVideo3()
    {
        video3.SetActive(true);
        StartCoroutine("offvideo3");
    }

    IEnumerator offvideo1()
    {
        yield return new WaitForSeconds(5.0f);
        video1.SetActive(false);
    }
    IEnumerator offvideo2()
    {
        yield return new WaitForSeconds(5.0f);
        video2.SetActive(false);
    }
    IEnumerator offvideo3()
    {
        yield return new WaitForSeconds(5.0f);
        video3.SetActive(false);
    }

}
