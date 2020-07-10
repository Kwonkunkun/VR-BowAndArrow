using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using Valve.VR;

public class Grip : MonoBehaviour
{
    public GameObject gripObject = null;
    public bool isInBowSpace = false;
    public bool isInArrowSpace = false;

    Transform tr;
    SteamVR_Behaviour_Skeleton skeleton;

    private void Start()
    {
        tr = GetComponent<Transform>();
        skeleton = GetComponent<SteamVR_Behaviour_Skeleton>();
    }

    public void OnGrip(string what)
    {
        if (what == "Bow")
        {
            //steamInput의 함수호출
            SteamInput.instance.PutUpBow(gripObject.GetComponent<Bow>());

            //bow를 잡는다. gravity false, kinemetic true
            //gripObject.GetComponent<BoxCollider>().enabled = false;
            Rigidbody rb_girp = gripObject.GetComponent<Rigidbody>();
            Transform tr_girp = gripObject.GetComponent<Transform>();
            rb_girp.isKinematic = true;
            rb_girp.useGravity = false;
            tr_girp.SetParent(tr);
            tr_girp.position = tr.position;
            tr_girp.localPosition -= new Vector3(0f, -0.075f, 0f);
            tr_girp.localRotation = Quaternion.Euler(45f, 0, 0);

            gripObject.GetComponent<BowBlend>().OnGripPose(skeleton);
        }
        else if(what == "Arrow")
        {
            GameObject gameObject = Instantiate(gripObject);

            //steamInput의 함수호출
            SteamInput.instance.PutUpArrow(gameObject.GetComponent<Arrow>());
            Rigidbody rb_girp = gameObject.GetComponent<Rigidbody>();
            Transform tr_girp = gameObject.GetComponent<Transform>();
            rb_girp.isKinematic = true;
            rb_girp.useGravity = false;
            tr_girp.SetParent(tr);


        }
    }

    public void OffGrip(string what, GameObject gripObj)
    {
        if (what == "Bow")
        {
            //steamInput의 함수호출
            SteamInput.instance.PutDownBow();

            //bow를 왼손에 잡는다. gravity false, kinemetic true
            //gripObject.GetComponent<BoxCollider>().enabled = false;
            Rigidbody rb_girp = gripObj.GetComponent<Rigidbody>();
            Transform tr_girp = gripObj.GetComponent<Transform>();
            rb_girp.isKinematic = false;
            rb_girp.useGravity = true;
            tr_girp.SetParent(null);

            gripObj.GetComponent<BowBlend>().OffGripPose(skeleton);
        }
        else if (what == "Arrow")
        {
            //steamInput의 함수호출
            SteamInput.instance.PutDownArrow();

            Rigidbody rb_girp = gripObj.GetComponent<Rigidbody>();
            Transform tr_girp = gripObj.GetComponent<Transform>();
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
        if (other.CompareTag("Arrow"))
        {
            Debug.Log("In Arrow Space");
            gripObject = other.gameObject;
            isInArrowSpace = true;
        }
        if (other.CompareTag("Bow_ArrowSet") && SteamInput.instance.m_Arrow != null)
        {
            //화살이 이미 걸려있는 경우는 제외
            if (SteamInput.instance.m_Bow.m_CurrentArrow == null)
            {
                Debug.Log("Bow_ArrowSet");
                SteamInput.instance.m_Bow.CreateArrow();

                SteamInput.instance.DestroyArrow();

                //화살을 거는 사운드 넣을것

            }
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
        if (other.CompareTag("Arrow"))
        {
            Debug.Log("Out Arrow Space");
            gripObject = null;
            isInArrowSpace = false;
        }
    }
}
