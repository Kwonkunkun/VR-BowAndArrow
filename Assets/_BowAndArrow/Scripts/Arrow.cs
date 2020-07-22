using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float m_Speed = 2000.0f;
    public Transform m_Tip = null;

    private Rigidbody m_Rigidbody = null;
    private bool m_IsStopped = true;
    private Vector3 m_LstPosition = Vector3.zero;
    
    //Trail 껐다 키기에 필요한 아이들
    private TrailRenderer line;
    

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        line = gameObject.GetComponent<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        if (m_IsStopped)
            return;

        // Rotate
        m_Rigidbody.MoveRotation(Quaternion.LookRotation(m_Rigidbody.velocity, transform.up));
    }

    public void Stop()
    {
        Debug.Log("Arrow Stop");
        m_IsStopped = true;

        m_Rigidbody.isKinematic = true;
        m_Rigidbody.useGravity = false;
        
       line.enabled = false;
    }

    public void Fire(float pulValue)
    {
        Debug.Log("Arrow Fire");
        m_IsStopped = false;
        transform.parent = null;

        m_Rigidbody.isKinematic = false;
        m_Rigidbody.useGravity = true;
        m_Rigidbody.AddForce(transform.forward * (pulValue * m_Speed));

        line.enabled = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Environment"))
        {
            Debug.Log("Environment");
            Destroy(gameObject);
        }
    }
}
