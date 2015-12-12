using UnityEngine;
using UnityEngine.UI;

public class HydraHead : MonoBehaviour {
    [SerializeField] private GameObject m_Skull;
    [SerializeField] private GameObject m_NeckStart;

    private Hydra m_Hydra;
    private Rigidbody2D m_SkullRb;
    private float m_Speed = 20f;
    private Vector3 m_Target;
    private bool m_IsChomping;

    private void Start() {
        m_Hydra = GetComponentInParent<Hydra>();
        m_SkullRb = m_Skull.GetComponent<Rigidbody2D>();
        m_Skull.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
    }

    public void AttachToBody(GameObject body) {
        transform.parent = body.transform.parent;
        m_NeckStart.GetComponent<DistanceJoint2D>().connectedBody = body.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (m_IsChomping) {
            Vector2 direction = (m_Target - m_Skull.transform.position).normalized;
            m_SkullRb.AddForce(direction * m_Speed, ForceMode2D.Impulse);

            float dist = Vector3.Distance(m_Skull.transform.position, m_Target);
            if (dist < 1f) {
                m_IsChomping = false;
                m_SkullRb.velocity = Vector3.zero;
                m_SkullRb.angularVelocity = 0f;
                m_SkullRb.inertia = 0f;
                m_Target = m_Skull.transform.position;
            }
        }
    }

    public void ChompAt(Vector3 target) {
        m_Target = target;
        m_IsChomping = true;
    }

    public void Eat(int targetCount) {
        m_Hydra.AddFood(targetCount);
    }
}
