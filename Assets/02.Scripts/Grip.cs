using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class Grip : MonoBehaviour
{
    Transform tr;
    SteamVR_Behaviour_Skeleton skeleton;

    #region WwiseSoundInput
    public AK.Wwise.Event ArrowNockSound;
    public AK.Wwise.Event ArrowSpawnSound;
   // public AK.Wwise.Event PullBackSound;
    public AK.Wwise.Event ArrowAirSound;
    public AK.Wwise.Event BowGripSound;
    #endregion



    [Header("Approach")]
    public bool isApproach = false;
    public bool isInBowSpace = false;
    public bool isInBowBend = false;
    public bool isInArrowSpace = false;
    public bool isInThrowObjSpace = false;
    public bool isInReloadSpace = false;
    public GameObject approachObj = null;

    [Header("Grip")]
    public bool isGrip = false;
    public bool isGripBow = false;
    public bool isGripArrow = false;
    public bool isGripThrowObj = false;
    public GameObject gripObj = null;

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

        //approachObj가 이미 잡힌 상태라면 return
        ObjStatus objStatus = approachObj.GetComponent<ObjStatus>();
        if (objStatus.isGrip == true)
            return;
        objStatus.isGrip = true;

        //공통부분
        gripObj = approachObj;
        approachObj.GetComponent<Outline>().OutlineWidth = approachObj.GetComponent<Outline>().turnOffWidth;
        Rigidbody rb_girpObj = gripObj.GetComponent<Rigidbody>();
        Transform tr_girpObj = gripObj.GetComponent<Transform>();
        rb_girpObj.isKinematic = true;
        rb_girpObj.useGravity = false;
        tr_girpObj.SetParent(tr);

        //세부부분
        if (what == "Bow")
        {
            tr_girpObj.position = tr.position;
            tr_girpObj.localPosition = new Vector3(0f, 0f, 0f);
            tr_girpObj.localRotation = Quaternion.identity;
            BowGripSound.Post(gameObject);
            gripObj.GetComponent<BowBlend>().OnGripPose(skeleton);
            gripObj.GetComponent<Bow>().OnCollider();
            isGripBow = true;
        }

  
        else if (what == "Arrow")
        {
            ArrowSpawnSound.Post(gameObject);
            tr_girpObj.position = tr.position;
            tr_girpObj.localPosition = new Vector3(0f, 0f, 0.0f);
            tr_girpObj.localRotation = Quaternion.identity;
            gripObj.GetComponent<ArrowBlend>().OnGripPose(skeleton);        
            isGripArrow = true;
        }
        else if (what == "ThrowObj")
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

        ObjStatus objStatus = gripObj.GetComponent<ObjStatus>();
        objStatus.isGrip = false;

        //공통 부분
        Rigidbody rb_girpObj = gripObj.GetComponent<Rigidbody>();
        Transform tr_girpObj = gripObj.GetComponent<Transform>();
        rb_girpObj.isKinematic = true;
        rb_girpObj.useGravity = false;
        tr_girpObj.SetParent(null);

        //추가부분
        if (what == "Bow")
        {
            gripObj.GetComponent<BowBlend>().OffGripPose(skeleton);
            gripObj.GetComponent<Bow>().OffCollider();
            isGripBow = false;
        }
        else if (what == "Arrow")
        {
            gripObj.GetComponent<ArrowBlend>().OffGripPose(skeleton);

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
            string whatHand = null;
            PlayNockSound();
            if (SteamInput.instance.leftGrip.isGripBow == true)
            {
                bow = SteamInput.instance.leftGrip.gripObj.GetComponent<Bow>();
                whatHand = "Left";
            }
            else if (SteamInput.instance.rightGrip.isGripBow == true)
            {
                bow = SteamInput.instance.rightGrip.gripObj.GetComponent<Bow>();
                whatHand = "Right";
            }

            //화살이 이미 걸려있는 경우는 제외
            if (bow.m_CurrentArrow == null)
            {
                Debug.Log("Bow_ArrowSet");
                gripObj.GetComponent<ArrowBlend>().OffGripPose(skeleton);
                bow.CreateArrow(whatHand);

                isGripArrow = false;
                isGrip = false;
                Destroy(gripObj);
                gripObj = null;

                //화살을 거는 사운드 넣을것
            }
        }

        if (isGrip == true || isApproach == true)
            return;

        if (other.CompareTag("Bow") || other.CompareTag("Arrow") || other.CompareTag("ThrowObj") ||
            other.CompareTag("Museum") || other.CompareTag("Experience") || other.CompareTag("Lobby") ||
            other.CompareTag("Reload") || other.CompareTag("LevelChanger"))
        {
            //공통사항
            approachObj = other.gameObject;
            if (approachObj != null)
                approachObj.GetComponent<Outline>().OutlineWidth = 6f;
            isApproach = true;

            //세부사항
            if (other.CompareTag("Bow"))
            {
                Debug.Log("In Bow Space");
                isInBowSpace = true;
            }
            else if (other.CompareTag("Arrow"))
            {
                Debug.Log("In Arrow Space");
                isInArrowSpace = true;
            }
            else if (other.CompareTag("ThrowObj"))
            {
                Debug.Log("In ThrowObj Space");
                isInThrowObjSpace = true;
            }
            else if (other.CompareTag("Reload"))
            {
                Debug.Log("In Reload Space");
                isInReloadSpace = true;
            }
            else if (other.CompareTag("LevelChanger"))
            {
                Debug.Log("In LevelChanger Space");
                GameManager.instance.MoveTarget();
            }
            if (SteamInput.instance.isGoingScene == false)
            {
                if (other.CompareTag("Museum"))
                {
                    //여기서 시작 
                    Debug.Log("Approach Museum Ball");
                    approachObj.GetComponent<PassSceneScript>().ScenePass("Museum");
                    SteamInput.instance.isGoingScene = true;
                }
                else if (other.CompareTag("Experience"))
                {
                    Debug.Log("Approach Experience Ball");
                    approachObj.GetComponent<PassSceneScript>().ScenePass("Experience");
                    SteamInput.instance.isGoingScene = true;

                }
                else if (other.CompareTag("Lobby"))
                {
                    Debug.Log("Approach Lobby Ball");
                    approachObj.GetComponent<PassSceneScript>().ScenePass("Lobby");
                    SteamInput.instance.isGoingScene = true;
                }
            }
        }
        else if (other.CompareTag("BowBend"))
        {
            Debug.Log("In BowBend Space");
            approachObj = other.gameObject;
            if (approachObj != null)
                approachObj.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("ScaleUp", true);
            isInBowBend = true;
            isApproach = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bow") || other.CompareTag("Arrow") || other.CompareTag("ThrowObj") ||
            other.CompareTag("Museum") || other.CompareTag("Experience") || other.CompareTag("Lobby") ||
            other.CompareTag("Reload") || other.CompareTag("LevelChanger"))
        {
            //세부사항
            if (other.CompareTag("Bow"))
            {
                Debug.Log("Out Bow Space");
                isInBowSpace = false;
            }
            else if (other.CompareTag("Arrow"))
            {
                Debug.Log("Out Arrow Space");
                isInArrowSpace = false;
            }
            else if (other.CompareTag("ThrowObj"))
            {
                Debug.Log("Out ThrowObj Space");
                isInThrowObjSpace = false;
            }
            else if (other.CompareTag("Reload"))
            {
                Debug.Log("In Reload Space");
                isInReloadSpace = false;
            }

            //공통사항
            if (approachObj != null)
                approachObj.GetComponent<Outline>().OutlineWidth = 2f;
            isApproach = false;
            approachObj = null;
        }

        else if (other.CompareTag("BowBend"))
        {
            Debug.Log("Out BowBend Space");
            if (approachObj != null)
                approachObj.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("ScaleUp", false);
            approachObj = null;
            isInBowBend = false;
            isApproach = false;
        }
    }
    #endregion


    #region 사운드 재생 함수
    void PlayNockSound()
    {
        ArrowNockSound.Post(gameObject);



    }
    #endregion
}

