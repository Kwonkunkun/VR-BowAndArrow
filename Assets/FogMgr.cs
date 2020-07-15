using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aura2API;
using UnityEditor;
public class FogMgr : MonoBehaviour
{
    public bool isGripBow = false;
    public bool isGripArrow = false;
    public AuraCamera auraCamera;
    void Start()
    {
    //GameObject camera =
   // auraCamera.frustumSettings.
        
    }

    void Update()
    {
        if(isGripBow && isGripArrow)
        {
            //auraCamera의 Scaterring 값을 서서히 줄인다
            for(int i = 20; i>1; i-- )
            //Scateriing 값
            {
                auraCamera.frustumSettings.BaseSettings.scattering = 0;
            }
        }
    }
}
