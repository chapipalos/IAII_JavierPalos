using UnityEngine;

public class SphereController : MonoBehaviour
{
    public float m_Radius = 5f;
    public float m_MinSpeed = 30f;   
    public float m_MaxSpeed = 120f;  
    public bool m_Start = false;

    private float m_Angle;
    private float m_TargetSpeed;     
    private float m_Elapsed;         
    private float m_CycleDuration;   

    void Start()
    {
        m_TargetSpeed = Random.Range(m_MinSpeed, m_MaxSpeed);

        m_CycleDuration = 180f * Mathf.PI / m_TargetSpeed;
    }

    void Update()
    {
        if (!m_Start) return;

        m_Elapsed += Time.deltaTime;

        float u = Mathf.Clamp01(m_Elapsed / m_CycleDuration);

        float phase = u * Mathf.PI;

        float currentSpeed = Mathf.Sin(phase) * m_TargetSpeed;

        m_Angle += currentSpeed * Time.deltaTime;

        if (m_Elapsed >= m_CycleDuration)
        {
            m_Elapsed = 0f;
            m_Angle = 0f; 
        }

        float rad = m_Angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(rad) * m_Radius;
        float z = Mathf.Sin(rad) * m_Radius;

        transform.position = new Vector3(x, transform.position.y, z);
    }
}
