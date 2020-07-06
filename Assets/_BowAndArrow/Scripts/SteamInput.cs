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

    public Grip leftGrip = null;

    private void Update()
    {
        Inputs();
    }

    private void Inputs()
    {
        //오른손
        //화살을 땡겨서 쏜다.
        if (m_PullAction.GetStateDown(m_RightHandPose.inputSource))
        {
            Debug.Log("RightHand GetStateDown");

            if (m_Bow != null)
            {
                Debug.Log("Bow pull");
                m_Bow.Pull(m_RightHandPose.gameObject.transform);
            }
        }
        if (m_PullAction.GetStateUp(m_RightHandPose.inputSource))
        {
            Debug.Log("RightHand GetStateUp");

            if (m_Bow != null)
            {
                Debug.Log("Bow release");
                m_Bow.Release();
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
                leftGrip.OnGrip();            
            }

            ////활놓기
            //if (leftGrip.isInBowSpace == true && m_Bow != null)
            //{
            //    Debug.Log("Put down Bow");
            //}
        }
        if (m_PullAction.GetStateUp(m_LeftHandPose.inputSource))
        {
            Debug.Log("LeftHand GetStateUp");
        }
    }

    public void ReadyBow(Bow bow)
    {
        Debug.Log("ReadyBow");
        m_Bow = bow;
    }
}
