using UnityEngine;
using Valve.VR;

public class SteamInput : MonoBehaviour
{
    #region 싱글톤
    private static SteamInput m_instance;

    public static SteamInput instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 SteamInput 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<SteamInput>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    #endregion

    public Arrow m_Arrow = null;
    public Bow m_Bow = null;
    public SteamVR_Behaviour_Pose m_LeftHandPose = null;
    public SteamVR_Behaviour_Pose m_RightHandPose = null;
    public SteamVR_Action_Boolean m_PullAction = null;
    public SteamVR_Action_Vibration haptic = SteamVR_Actions.default_Haptic;

    public Grip leftGrip = null;
    public Grip rightGrip = null;

    public bool IsRight = false;
    public bool IsLeft = false;

    private void Update()
    {
        Inputs();
    }

    private void Inputs()
    {
        //오른손
        if (m_PullAction.GetStateDown(m_RightHandPose.inputSource))
        {
            Debug.Log("RightHand GetStateDown");

            //활잡기
            if (rightGrip.isInBowSpace == true && m_Bow == null && IsRight == false && IsLeft == false)
            {
                Debug.Log("Grip Bow");
                rightGrip.OnGrip("Bow");
                IsRight = true;
                IsLeft = false;

                //활을 줍는 사운드
            }
            //활놓기
            else if (rightGrip.isInBowSpace == true && m_Bow != null && IsRight == true)
            {
                Debug.Log("Put down Bow");
                rightGrip.OffGrip("Bow", m_Bow.gameObject);
                IsRight = false;

                //활을 놓는 사운드
            }

            //활쏘기
            if (m_Bow != null && IsLeft == true)
            {
                Debug.Log("Bow pull");
                m_Bow.Pull(m_RightHandPose.gameObject.transform);

                //활시위를 당기는 사운드 (bow 스크립트에서 처리하셈)
            }

            //화살 잡기
            if (rightGrip.isInArrowSpace == true && m_Arrow == null)
            {
                Debug.Log("Grip Arrow");
                rightGrip.OnGrip("Arrow");

                //화살 잡는 사운드
            }
            //화살 놓기
            else if (rightGrip.isInArrowSpace == true && m_Arrow != null)
            {
                Debug.Log("Grip Arrow");
                rightGrip.OffGrip("Arrow", m_Arrow.gameObject);

                //화살을 놓는 사운드
            }
        }
        else if (m_PullAction.GetStateUp(m_RightHandPose.inputSource))
        {
            Debug.Log("RightHand GetStateUp");
            
            //활쏘기
            if (m_Bow != null && IsLeft == true)
            {
                Debug.Log("Bow release");
                m_Bow.Release();
                haptic.Execute(0.2f, 0.5f, 200.0f, 1f, m_RightHandPose.inputSource); //웨이팅 타임 지속시간, 주파수, 진폭

                //활이 날아가는 사운드
            }  
        }

        //사운드는 왼손 동일

        //왼손
        if (m_PullAction.GetStateDown(m_LeftHandPose.inputSource))
        {
            Debug.Log("LeftHand GetStateDown");
            //활잡기
            if (leftGrip.isInBowSpace == true && m_Bow == null && IsLeft == false && IsRight == false)
            {
                Debug.Log("Grip Bow");
                leftGrip.OnGrip("Bow");
                IsLeft = true;
                IsRight = false;
            }
            //활놓기
            else if (leftGrip.isInBowSpace == true && m_Bow != null && IsLeft == true)
            {
                Debug.Log("Put down Bow");
                leftGrip.OffGrip("Bow", m_Bow.gameObject);
                IsLeft = false;
            }

            //활쏘기
            if (m_Bow != null && IsRight == true)
            {
                Debug.Log("Bow pull");
                m_Bow.Pull(m_LeftHandPose.gameObject.transform);
            }


            //화살 잡기
            if (leftGrip.isInArrowSpace == true && m_Arrow == null)
            {
                Debug.Log("Grip Arrow");
                leftGrip.OnGrip("Arrow");
            }
            //화살 놓기
            else if (leftGrip.isInArrowSpace == true && m_Arrow != null)
            {
                Debug.Log("Grip Arrow");
                leftGrip.OffGrip("Arrow", m_Arrow.gameObject);
            }
        }
        else if (m_PullAction.GetStateUp(m_LeftHandPose.inputSource))
        {
            Debug.Log("LeftHand GetStateUp");


            //활쏘기
            if (m_Bow != null && IsRight == true)
            {
                Debug.Log("Bow release");
                m_Bow.Release();
                haptic.Execute(0.2f, 0.5f, 200.0f, 1f, m_LeftHandPose.inputSource); //웨이팅 타임 지속시간, 주파수, 진폭
            }
        }
    }

    public void PutUpBow(Bow bow)
    {
        Debug.Log("Put Up Bow");
        m_Bow = bow;
    }

    public void PutDownBow()
    {
        Debug.Log("Put Down Bow");
        m_Bow = null;
    }

    public void PutUpArrow(Arrow arrow)
    {
        Debug.Log("Put Up Arrow");
        m_Arrow = arrow;
    }

    public void PutDownArrow()
    {
        Debug.Log("Put Down Arrow");
        m_Arrow = null;
    }

    public void DestroyArrow()
    {
        Debug.Log("DestroyArrow");
        Destroy(m_Arrow.gameObject);
        m_Arrow = null;
    }
}
