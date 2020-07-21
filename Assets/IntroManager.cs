using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("NextScene");
    }


    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(15.0f);
     SceneManager.LoadScene("Game");
    }
}
