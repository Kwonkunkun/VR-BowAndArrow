using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Valve.VR;
public class Bow : MonoBehaviour
{
    [Header("Assets")]
    public GameObject m_ArrowPrefab = null;

    [Header("Bow")]
    public float m_GrabThreshold = 0.15f;
    public Transform m_Start = null;
    public Transform m_End = null;
    public Transform m_Socket = null;

    private Transform m_PullingHand = null;
    public Arrow m_CurrentArrow = null;
    private Animator m_Animator = null;

    [Header("Arrow Set Position")]
    public Transform rightArrowSetPos;
    public Transform leftArrowSetPos;

    private float m_PulValue = 0.0f;
    private string whatIsHand = null;
    private SteamVR_Skeleton_Poser steamVR_Skeleton_Poser;

    #region
    public AK.Wwise.Event InAirSound;
    public AK.Wwise.Event PullBackSound;
    public AK.Wwise.Event RealeseSound;
    #endregion
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        steamVR_Skeleton_Poser = GetComponent<SteamVR_Skeleton_Poser>();
    }
    private void Update()
    {
        if (!m_PullingHand /*|| !m_CurrentArrow*/)
            return;

        m_PulValue = CalculaterPull(m_PullingHand);
        m_PulValue = Mathf.Clamp(m_PulValue, 0.0f, 1.0f);

        Debug.Log(m_PulValue);
        m_Animator.SetFloat("Blend", m_PulValue);

        //pose
        steamVR_Skeleton_Poser.SetBlendingBehaviourValue("ShotPose", m_PulValue);

        if (m_CurrentArrow != null)
        {
            Transform tr_arrow = m_CurrentArrow.gameObject.transform;

            if (whatIsHand == "Left")
                tr_arrow.LookAt(rightArrowSetPos);
            else if (whatIsHand == "Right")
                tr_arrow.LookAt(leftArrowSetPos);
        }
    }

    private float CalculaterPull(Transform pullHand)
    {
        Vector3 direction = m_End.position - m_Start.position;
        float magnitude = direction.magnitude;

        direction.Normalize();
        Vector3 diffrence = pullHand.position - m_Start.position;

        return Vector3.Dot(diffrence, direction) / magnitude;
    }
    public void CreateArrow(string whatHand)
    {
        //create child
        GameObject arrowObject = Instantiate(m_ArrowPrefab, m_Socket);

        //orient
        arrowObject.transform.localScale /= 6;
        arrowObject.transform.localPosition = new Vector3(0, 0, 0);
        arrowObject.transform.localEulerAngles = Vector3.zero;

        if (whatHand == "Left")
            arrowObject.transform.LookAt(rightArrowSetPos);
        else if (whatHand == "Right")
            arrowObject.transform.LookAt(leftArrowSetPos);

        whatIsHand = whatHand;

        //set
        m_CurrentArrow = arrowObject.GetComponent<Arrow>();

        ObjStatus objStatus = arrowObject.GetComponent<ObjStatus>();
        objStatus.isGrip = true;
    }
    public void Pull(Transform hand)
    {
        Debug.Log("Pull");
        float distance = Vector3.Distance(hand.position, m_Start.position);

        if (distance >= m_GrabThreshold)
            return;
        PullBackSound.Post(gameObject);
        m_PullingHand = hand;
    }
    public void Release()
    {
        Debug.Log("Release");

        if (m_PulValue > 0.25f)
        {
            if(m_CurrentArrow != null)
                FireArrow();
            RealeseSound.Post(gameObject);
        }

        m_PullingHand = null;

        m_PulValue = 0.0f;
        m_Animator.SetFloat("Blend", 0);
        //pose
        steamVR_Skeleton_Poser.SetBlendingBehaviourValue("ShotPose", m_PulValue);
    }
    private void FireArrow()
    {
        InAirSound.Post(gameObject);
        m_CurrentArrow.Fire(m_PulValue);
        m_CurrentArrow = null;
    }

    [Header("Collider")]
    public Collider bowBend;
    public Collider aroowSet;   
    public void OnCollider()
    {
        bowBend.enabled = true;
        aroowSet.enabled = true;
    }
    public void OffCollider()
    {
        bowBend.enabled = false;
        aroowSet.enabled = false;
    }
}
