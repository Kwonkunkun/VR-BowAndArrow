using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SigiTouch : MonoBehaviour
{
    
    public Canvas canvas;

    public void OnCanvas()
    {
        canvas.enabled = true;
    }
    public void OffCanvas()
    {
        canvas.enabled = false;
    }
}
