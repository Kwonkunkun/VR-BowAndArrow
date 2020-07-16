using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoIntro : MonoBehaviour
{
    public float timeOfPassScene;
    public GameObject effect;
    public void ScenePass()
    {
        StartCoroutine(PassScene());
    }

    IEnumerator PassScene()
    {
        Debug.Log("PassScene");
        effect.SetActive(true);
        yield return new WaitForSeconds(timeOfPassScene);
        SceneManager.LoadScene("Intro");
    }
}
