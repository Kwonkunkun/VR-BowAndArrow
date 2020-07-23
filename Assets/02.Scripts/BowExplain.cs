using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowExplain : MonoBehaviour
{
    public GameObject canvas;

    public void OnCanvas()
    {
        canvas.SetActive(true);
        StartCoroutine(offCanvas());
    }

    IEnumerator offCanvas()
    {
        yield return new WaitForSeconds(3.0f);
        canvas.SetActive(false);
    }
}
