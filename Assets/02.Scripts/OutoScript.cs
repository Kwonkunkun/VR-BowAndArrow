using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutoScript : MonoBehaviour
{
    public Light light;
    public float minAngle;
    public float maxAngle;
    public float spotAngleValue;
    private void FixedUpdate()
    {
        if (light.spotAngle < maxAngle)
        {
            light.spotAngle += spotAngleValue;
        }
    }
    
}
