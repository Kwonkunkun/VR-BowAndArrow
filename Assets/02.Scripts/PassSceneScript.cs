using Aura2API;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassSceneScript : MonoBehaviour
{
    public GameObject effect;
    public float timeOfPassScene;

    public void ScenePass(string what)
    {
        Debug.Log("PassScene");
        StartCoroutine(PassScene(what));
    }

    IEnumerator PassScene(string what)
    {
        effect.SetActive(true);
        yield return new WaitForSeconds(timeOfPassScene);
        SceneManager.LoadScene(what);
    }
}
