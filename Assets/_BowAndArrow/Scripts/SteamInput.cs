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

    #region Input 관련
    public SteamVR_Behaviour_Pose m_LeftHandPose = null;
    public SteamVR_Behaviour_Pose m_RightHandPose = null;
    public SteamVR_Action_Boolean m_PullAction = null;
    public SteamVR_Action_Vibration haptic = SteamVR_Actions.default_Haptic;
    #endregion

    #region 양손 Grip 스크립트
    public Grip leftGrip = null;
    public Grip rightGrip = null;
    #endregion

    private void Update()
    {
        Inputs();
    }

    private void Inputs()
    {
        #region 오른손
        //잡은게 없을때
        if (rightGrip.isGrip == false)
        {
            if (m_PullAction.GetStateDown(m_RightHandPose.inputSource))
            {

                //활쏘기, 왼손에 활이 있어야 됨
                if (leftGrip.isGripBow == true && rightGrip.isInBowBend == true)
                {
                    Bow bow = leftGrip.gripObj.GetComponent<Bow>();                   
                    bow.Pull(m_RightHandPose.gameObject.transform);

                    //활시위를 당기는 사운드 (bow 스크립트에서 처리하셈)
                }
                else
                {
                    //활잡기
                    if (rightGrip.isInBowSpace == true)
                    {
                        rightGrip.OnGrip("Bow");

                        //활을 줍는 사운드
                    }

                    //화살 잡기
                    else if (rightGrip.isInArrowSpace == true)
                    {
                        rightGrip.OnGrip("Arrow");

                        //화살 잡는 사운드
                    }

                    //공잡기
                    else if (rightGrip.isInThrowObjSpace == true)
                    {
                        rightGrip.OnGrip("ThrowObj");
                    }
                }
            }
            else if (m_PullAction.GetStateUp(m_RightHandPose.inputSource))
            {

                //활쏘기
                if (leftGrip.isGripBow == true)
                {
                    Bow bow = leftGrip.gripObj.GetComponent<Bow>();
                    bow.Release();
                    haptic.Execute(0.2f, 0.5f, 200.0f, 1f, m_RightHandPose.inputSource); //웨이팅 타임 지속시간, 주파수, 진폭

                    //활이 날아가는 사운드
                }         
            }
        }
        //잡은게 있을때
        else if(rightGrip.isGrip == true)
        {
            if (m_PullAction.GetStateDown(m_RightHandPose.inputSource))
            {
                //활놓기
                if (rightGrip.isGripBow == true)
                {
                    rightGrip.OffGrip("Bow");

                    //활을 놓는 사운드
                }

                //화살 놓기
                if (rightGrip.isGripArrow == true)
                {
                    rightGrip.OffGrip("Arrow");
                    //화살을 놓는 사운드
                }

                //공 던지기
                if (rightGrip.isGripThrowObj == true)
                {
                    rightGrip.OffGrip("ThrowObj");
                }
            }
        }
        #endregion

        #region 왼손
        //왼손
        //잡은게 없을때
        if (leftGrip.isGrip == false)
        {
            if (m_PullAction.GetStateDown(m_LeftHandPose.inputSource))
            {
                Debug.Log("RightHand Grip");

                //활쏘기, 왼손에 활이 있어야 됨
                if (rightGrip.isGripBow == true && leftGrip.isApproach == false)
                {
                    Debug.Log("Bow pull");
                    Bow bow = leftGrip.GetComponent<Bow>();
                    bow.Pull(m_LeftHandPose.gameObject.transform);

                    //활시위를 당기는 사운드 (bow 스크립트에서 처리하셈)
                }
                else
                {
                    //활잡기
                    if (leftGrip.isInBowSpace == true)
                    {
                        Debug.Log("Grip Bow");
                        leftGrip.OnGrip("Bow");

                        //활을 줍는 사운드
                    }

                    //화살 잡기
                    else if (leftGrip.isInArrowSpace == true)
                    {
                        Debug.Log("Grip Arrow");
                        leftGrip.OnGrip("Arrow");

                        //화살 잡는 사운드
                    }

                    //공잡기
                    else if (leftGrip.isInThrowObjSpace == true)
                    {
                        Debug.Log("OnGrip Ball");
                        leftGrip.OnGrip("ThrowObj");
                    }
                }
            }
            else if (m_PullAction.GetStateUp(m_LeftHandPose.inputSource))
            {

                //활쏘기
                if (rightGrip.isGripBow == true && leftGrip.isInBowBend == true)
                {
                    Debug.Log("Bow release");
                    Bow bow = rightGrip.GetComponent<Bow>();
                    bow.Release();
                    haptic.Execute(0.2f, 0.5f, 200.0f, 1f, m_LeftHandPose.inputSource); //웨이팅 타임 지속시간, 주파수, 진폭

                    //활이 날아가는 사운드
                }
            }
        }
        //잡은게 있을때
        else if (leftGrip.isGrip == true)
        {
            if (m_PullAction.GetStateDown(m_LeftHandPose.inputSource))
            {
                //활놓기
                if (leftGrip.isGripBow == true)
                {
                    Debug.Log(" Bow");
                    leftGrip.OffGrip("Bow");

                    //활을 놓는 사운드
                }

                //화살 놓기
                if (leftGrip.isGripArrow == true)
                {
                    //Debug.Log(" Arrow");
                    leftGrip.OffGrip("Arrow");

                    //화살을 놓는 사운드
                }

                //공 던지기
                if (leftGrip.isGripThrowObj == true)
                {
                    leftGrip.OffGrip("ThrowObj");
                }
            }
        }
        #endregion
    }
}
