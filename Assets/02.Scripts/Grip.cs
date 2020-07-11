using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using Valve.VR;

public class Grip : MonoBehaviour
{   
    [Header ("Approach")]
    public bool isApproach = false;
    public bool isInBowSpace = false;
    public bool isInBowBend = false;
    public bool isInArrowSpace = false;
    public bool isInThrowObjSpace = false;
    public GameObject approachObj = null;

    [Header("Grip")]
    public bool isGrip = false;
    public bool isGripBow = false;
    public bool isGripArrow = false;
    public bool isGripThrowObj = false;
    public GameObject gripObj = null;

    Transform tr;
    SteamVR_Behaviour_Skeleton skeleton;

    private void Start()
    {
        tr = GetComponent<Transform>();
        skeleton = GetComponent<SteamVR_Behaviour_Skeleton>();
    }

    #region 잡기 관련
    public void OnGrip(string what)
    {
        //이미 잡은게 있다면
        if (isGrip == true && gripObj == null)
            return;

        //공통부분
        gripObj = approachObj;
        Rigidbody rb_girpObj = gripObj.GetComponent<Rigidbody>();
        Transform tr_girpObj = gripObj.GetComponent<Transform>();
        rb_girpObj.isKinematic = true;
        rb_girpObj.useGravity = false;
        tr_girpObj.SetParent(tr);

        //추가부분
        if (what == "Bow")
        {
            tr_girpObj.position = tr.position;
            tr_girpObj.localPosition -= new Vector3(0f, -0.075f, 0f);
            tr_girpObj.localRotation = Quaternion.Euler(45f, 0, 0);

            gripObj.GetComponent<BowBlend>().OnGripPose(skeleton);

            isGripBow = true;
        }
        else if (what == "Arrow")
        {
            //attachObj.GetComponent<ArrowBlend>().OnGripPose(skeleton);

            isGripArrow = true;
        }
        else if(what == "ThrowObj")
        {
            //여기에 grip pose 애니매이션 ㄲ

            isGripThrowObj = true;
        }
        isGrip = true;
    }

    public void OffGrip(string what)
    {
        //잡은게 없다면
        if (isGrip == false && gripObj != null)
            return;

        //공통 부분
        Rigidbody rb_girpObj = gripObj.GetComponent<Rigidbody>();
        Transform tr_girpObj = gripObj.GetComponent<Transform>();
        rb_girpObj.isKinematic = false;
        rb_girpObj.useGravity = true;
        tr_girpObj.SetParent(null);
        
        //추가부분
        if (what == "Bow")
        {
            gripObj.GetComponent<BowBlend>().OffGripPose(skeleton);

            isGripBow = false;
        }
        else if (what == "Arrow")
        {
            //gripObj.GetComponent<ArrowBlend>().OffGripPose(skeleton);

            isGripArrow = false;
        }
        else if (what == "ThrowObj")
        {
            isGripThrowObj = false;
        }

        isGrip = false;
        gripObj = null;
    }
    #endregion

    #region Approach관련
    private void OnTriggerEnter(Collider other)
    {
        //화살걸기
        if (other.CompareTag("Bow_ArrowSet") && isGripArrow == true 
            && (SteamInput.instance.leftGrip.isGripBow == true || SteamInput.instance.rightGrip.isGripBow == true))
        {
            Bow bow = null;
            if (SteamInput.instance.leftGrip.isGripBow == true)
                bow = SteamInput.instance.leftGrip.gripObj.GetComponent<Bow>();
            else if(SteamInput.instance.rightGrip.isGripBow == true)
                bow = SteamInput.instance.rightGrip.gripObj.GetComponent<Bow>();

            //화살이 이미 걸려있는 경우는 제외
            if (bow.m_CurrentArrow == null)
            {
                Debug.Log("Bow_ArrowSet");
                bow.CreateArrow();

                isGripArrow = false;
                isGrip = false;
                Destroy(gripObj);
                gripObj = null;

                //화살을 거는 사운드 넣을것
            }
        }

        //이미 근처에 다가간것이 있다면? return
        if (isGrip == true)
            return;
        
        if (isApproach == true)
            return;

        if (other.CompareTag("Bow"))
        {
            Debug.Log("In Bow Space");
            approachObj = other.gameObject;
            isInBowSpace = true;
            isApproach = true;
        }
        else if (other.CompareTag("Arrow"))
        {
            Debug.Log("In Arrow Space");
            approachObj = other.gameObject;
            isInArrowSpace = true;
            isApproach = true;
        }
        else if (other.CompareTag("ThrowObj"))
        {
            Debug.Log("In ThrowObj Space");
            approachObj = other.gameObject;
            isInThrowObjSpace = true;
            isApproach = true;
        }      
        else if(other.CompareTag("BowBend"))
        {
            isInBowBend = true;
            isApproach = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bow"))
        {
            Debug.Log("Out Bow Space");
            approachObj = null;
            isInBowSpace = false;
            isApproach = false;
        }
        else if (other.CompareTag("Arrow"))
        {
            Debug.Log("Out Arrow Space");
            approachObj = null;
            isInArrowSpace = false;
            isApproach = false;
        }
        else if (other.CompareTag("ThrowObj"))
        {
            Debug.Log("Out ThrowObj Space");
            approachObj = null;
            isInThrowObjSpace = false;
            isApproach = false;
        }
        else if (other.CompareTag("BowBend"))
        {
            isInBowBend = false;
            isApproach = false;
        }
    }
    #endregion
}
