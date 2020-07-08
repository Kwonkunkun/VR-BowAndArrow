using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using Valve.VR;

public class Grip : MonoBehaviour
{
    public GameObject gripObject = null;
    public bool isInBowSpace = false;
    
    Transform tr;
    
    private void Start()
    {
        tr = this.GetComponent<Transform>();
    }

    public void OnGrip(string what)
    {
        if (what == "Bow")
        {
            //steamInput의 함수호출
            SteamInput.instance.PutUpBow(gripObject.GetComponent<Bow>());

            //bow를 왼손에 잡는다. gravity false, kinemetic true
            //gripObject.GetComponent<BoxCollider>().enabled = false;
            Rigidbody rb_girp = gripObject.GetComponent<Rigidbody>();
            Transform tr_girp = gripObject.GetComponent<Transform>();
            rb_girp.isKinematic = true;
            rb_girp.useGravity = false;
            tr_girp.SetParent(tr);
            tr_girp.position = tr.position;
            tr_girp.localPosition -= new Vector3(0f, -0.075f, 0f);
            tr_girp.localRotation = Quaternion.Euler(45f, 0, 0);
        }
    }

    public void OffGrip(string what)
    {
        if (what == "Bow")
        {
            //steamInput의 함수호출
            SteamInput.instance.PutDownBow();

            //bow를 왼손에 잡는다. gravity false, kinemetic true
            //gripObject.GetComponent<BoxCollider>().enabled = false;
            Rigidbody rb_girp = gripObject.GetComponent<Rigidbody>();
            Transform tr_girp = gripObject.GetComponent<Transform>();
            rb_girp.isKinematic = false;
            rb_girp.useGravity = true;
            tr_girp.SetParent(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bow"))
        {
            Debug.Log("In Bow Space");
            gripObject = other.gameObject;
            isInBowSpace = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bow"))
        {
            Debug.Log("Out Bow Space");
            gripObject = null;
            isInBowSpace = false;
        }
    }
}
