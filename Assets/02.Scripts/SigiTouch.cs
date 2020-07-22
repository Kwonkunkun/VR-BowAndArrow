using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SigiTouch : MonoBehaviour
{
    
    public GameObject canvas;

    public void OnCanvas()
    {
        canvas.SetActive(true);
        StartCoroutine(OffCanvas());
    }
    IEnumerator OffCanvas()
    {
        yield return new WaitForSeconds(5.0f);

        canvas.SetActive(false);
    }
}
