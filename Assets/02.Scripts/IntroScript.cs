using Aura2API;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    public GameObject effect;
    public float timeOfPassScene;
    public AuraVolume auraVolume;
    private void Start()
    {
        //StartCoroutine(PassScene());
    }
    public void ScenePass()
    {
        Debug.Log("PassScene");
        StartCoroutine(PassScene());
    }

    IEnumerator PassScene()
    {
        effect.SetActive(true);
        auraVolume.scatteringInjection.strength = 0.0f;
        yield return new WaitForSeconds(timeOfPassScene);
        SceneManager.LoadScene("Main");
    }
}
