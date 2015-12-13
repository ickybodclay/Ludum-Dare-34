using UnityEngine;
using System.Collections;

public class HydraHead : MonoBehaviour {
    [SerializeField] private GameObject m_Skull;
    [SerializeField] private GameObject m_NeckStart;

    private Hydra m_Hydra;
    private Rigidbody2D m_SkullRb;
    private SpriteRenderer m_SkullRenderer;
    private float m_Speed = 20f;
    private Vector3 m_Target;
    private bool m_IsChomping;

    private float m_Health;
    private float m_MaxHealth;

    private float m_EndChompDistance = 9.9f;
    private float m_SkullIdlePosY = 2.0f;
    private int m_StartSortingOrder;

    private void Start() {
        m_Hydra = GetComponentInParent<Hydra>();
        m_SkullRb = m_Skull.GetComponent<Rigidbody2D>();
        m_SkullRenderer = m_Skull.GetComponent<SpriteRenderer>();
        m_SkullRenderer.color = Random.ColorHSV();
        m_StartSortingOrder = m_SkullRenderer.sortingOrder;
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
            if (dist < m_EndChompDistance) {
                StartCoroutine(EndChomp());
            }
        }
        else if (m_Skull.transform.position.y < m_SkullIdlePosY) {
            m_SkullRb.AddForce(Vector3.up * m_Speed * 0.5f, ForceMode2D.Impulse);
        }
    }

    IEnumerator EndChomp() {
        yield return new WaitForSeconds(1f);

        Debug.Log("chomp complete");
        m_IsChomping = false;
        m_SkullRb.velocity = Vector3.zero;
        m_SkullRb.angularVelocity = 0f;
        m_SkullRb.inertia = 0f;
    }

    public void ChompAt(Vector3 target) {
        m_Target = target;
        m_IsChomping = true;
    }

    public void Eat(int targetCount) {
        m_Hydra.AddFood(targetCount);
    }

    public void Focus() {
        m_SkullRenderer.sortingOrder = m_StartSortingOrder + 1;
    }

    public void EndFocus() {
        m_SkullRenderer.sortingOrder = m_StartSortingOrder;
    }
}
