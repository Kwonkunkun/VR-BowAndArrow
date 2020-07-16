using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    public float timeOfPassScene;
    private void Start()
    {
        StartCoroutine(PassScene());
    }
    public void ScenePass()
    {
        Debug.Log("PassScene");
        StartCoroutine(PassScene());
    }

    IEnumerator PassScene()
    { 
        yield return new WaitForSeconds(timeOfPassScene);
        SceneManager.LoadScene("RealMain");
    }
}
