using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float m_Speed = 2000.0f;
    public Transform m_Tip = null;

    private Rigidbody m_Rigidbody = null;
    private bool m_IsStopped = true;
    private Vector3 m_LstPosition = Vector3.zero;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (m_IsStopped)
            return;

        // Rotate
        m_Rigidbody.MoveRotation(Quaternion.LookRotation(m_Rigidbody.velocity, transform.up));

        //// Collision
        //if (Physics.Linecast(m_LstPosition, m_Tip.position, 1 << 9))
        //{
        //    Stop();
        //}

        // Store position
        m_LstPosition = m_Tip.position;
    }

    private void Stop()
    {
        Debug.Log("Arrow Stop");
        m_IsStopped = true;

        m_Rigidbody.isKinematic = true;
        m_Rigidbody.useGravity = false;
    }

    public void Fire(float pulValue)
    {
        Debug.Log("Arrow Fire");
        m_IsStopped = false;
        transform.parent = null;

        m_Rigidbody.isKinematic = false;
        m_Rigidbody.useGravity = true;
        m_Rigidbody.AddForce(transform.forward * (pulValue * m_Speed));

        Destroy(gameObject, 5.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.CompareTag("Environment"))
        //{
        //    Debug.Log("Environment");
        //    Stop();
        //}
    }
}
