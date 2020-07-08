using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;

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

    private float m_PulValue = 0.0f;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        
    }
    private void Update()
    {
        if (!m_PullingHand || !m_CurrentArrow)
            return;

        m_PulValue = CalculaterPull(m_PullingHand);
        m_PulValue = Mathf.Clamp(m_PulValue, 0.0f, 1.0f);

        m_Animator.SetFloat("Blend", m_PulValue);
    }
    private float CalculaterPull(Transform pullHand)
    {
        Vector3 direction = m_End.position - m_Start.position;
        float magnitude = direction.magnitude;

        direction.Normalize();
        Vector3 diffrence = pullHand.position - m_Start.position;

        return Vector3.Dot(diffrence, direction) / magnitude;
    }
    public void CreateArrow()
    {
        //create child
        GameObject arrowObject = Instantiate(m_ArrowPrefab, m_Socket);

        //orient
        arrowObject.transform.localPosition = new Vector3(0, 0, 0.425f);
        arrowObject.transform.localEulerAngles = Vector3.zero;

        //set
        m_CurrentArrow = arrowObject.GetComponent<Arrow>();
    }
    public void Pull(Transform hand)
    {
        Debug.Log("Pull");
        float distance = Vector3.Distance(hand.position, m_Start.position);

        if (distance >= m_GrabThreshold)
            return;

        m_PullingHand = hand;
    }
    public void Release()
    {
        Debug.Log("Release");
        if (m_PulValue > 0.25f && m_CurrentArrow != null)
            FireArrow();

        m_PullingHand = null;

        m_PulValue = 0.0f;
        m_Animator.SetFloat("Blend", 0);
    }
    private void FireArrow()
    {
        m_CurrentArrow.Fire(m_PulValue);
        m_CurrentArrow = null;
    }
    
}
