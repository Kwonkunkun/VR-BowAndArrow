using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light : MonoBehaviour
{
  public Transform sun;
    public int RotationScale;

    // Start is called before the first frame update
    void Start()
    {
        RotationScale = 20;    
    }

    // Update is called once per frame
    void Update()
    {
        sun.Rotate(RotationScale * Time.deltaTime, 0, 0);
    }
}
