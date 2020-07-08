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

    public Bow m_Bow = null;
    public SteamVR_Behaviour_Pose m_LeftHandPose = null;
    public SteamVR_Behaviour_Pose m_RightHandPose = null;
    public SteamVR_Action_Boolean m_PullAction = null;
    public SteamVR_Action_Vibration haptic = SteamVR_Actions.default_Haptic;

    public Grip leftGrip = null;
    public Grip rightGrip = null;

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
            if (rightGrip.isInBowSpace == true && m_Bow == null)
            {
                Debug.Log("Grip Bow");
                rightGrip.OnGrip("Bow");
            }
            //활쏘기
            if (m_Bow != null)
            {
                Debug.Log("Bow pull");
                m_Bow.Pull(m_RightHandPose.gameObject.transform);
            }
        }
        if (m_PullAction.GetStateUp(m_RightHandPose.inputSource))
        {
            Debug.Log("RightHand GetStateUp");

            //활놓기
            if (rightGrip.isInBowSpace == true && m_Bow != null)
            {
                Debug.Log("Put down Bow");
                rightGrip.OffGrip("Bow");
            }
            //활쏘기
            if (m_Bow != null)
            {
                Debug.Log("Bow release");
                m_Bow.Release();
                haptic.Execute(0.2f, 0.5f, 200.0f, 1f, m_RightHandPose.inputSource); //웨이팅 타임 지속시간, 주파수, 진폭
            }
        }

        //왼손
        if (m_PullAction.GetStateDown(m_LeftHandPose.inputSource))
        {
            Debug.Log("LeftHand GetStateDown");

            //활잡기
            if (leftGrip.isInBowSpace == true && m_Bow == null)
            {
                Debug.Log("Grip Bow");
                leftGrip.OnGrip("Bow");
            }
            //활쏘기
            if (m_Bow != null)
            {
                Debug.Log("Bow pull");
                m_Bow.Pull(m_LeftHandPose.gameObject.transform);
            }
        }
        if (m_PullAction.GetStateUp(m_LeftHandPose.inputSource))
        {
            Debug.Log("LeftHand GetStateUp");

            //활놓기
            if (leftGrip.isInBowSpace == true && m_Bow != null)
            {
                Debug.Log("Put down Bow");
                leftGrip.OffGrip("Bow");
            }
            //활쏘기
            if (m_Bow != null)
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
}
