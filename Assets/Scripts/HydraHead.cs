using UnityEngine;
using System.Collections;

public class HydraHead : MonoBehaviour {
    [SerializeField] private GameObject m_Skull;
    [SerializeField] private GameObject m_NeckStart;

    private Hydra m_Hydra;
    private Rigidbody2D m_SkullRb;
    private SpriteRenderer m_SkullRenderer;
    private Animator m_SkullAnimator;
    private float m_Speed = 20f;
    private Vector3 m_Target;
    private bool m_IsChomping;

    private float m_Health;
    private float m_MaxHealth;

    private float m_EndChompDistance = 9.9f;
    private float m_SkullIdlePosY = 2.0f;
    private int m_StartSortingOrder;

    private Color[] m_SkullColors = new Color[] {
        Color.blue,
        Color.cyan,
        Color.red,
        Color.magenta,
        Color.green,
        Color.yellow
    };

    private void Awake() {
        m_SkullRb = m_Skull.GetComponent<Rigidbody2D>();
        m_SkullRenderer = m_Skull.GetComponent<SpriteRenderer>();
        m_SkullAnimator = m_Skull.GetComponent<Animator>();
        m_SkullRenderer.color = RandomColor();
        m_StartSortingOrder = m_SkullRenderer.sortingOrder;
    }

    private Color RandomColor() {
        Color col = m_SkullColors[Random.Range(0, m_SkullColors.Length)];
        col.r = Random.Range(col.r == 0f ? 0f : col.r * 0.5f, col.r);
        col.g = Random.Range(col.g == 0f ? 0f : col.g * 0.5f, col.g);
        col.b = Random.Range(col.b == 0f ? 0f : col.b * 0.5f, col.b);
        return col;
    }

    public void AttachToBody(GameObject body) {
        transform.parent = body.transform.parent;
        m_NeckStart.GetComponent<DistanceJoint2D>().connectedBody = body.GetComponent<Rigidbody2D>();
        m_Hydra = GetComponentInParent<Hydra>();
    }

    private void FixedUpdate() {
        if (m_IsChomping) {
            Vector2 direction = (m_Target - m_Skull.transform.position).normalized;
            m_SkullRb.AddForce(direction * m_Speed, ForceMode2D.Impulse);

            float dist = Vector3.Distance(m_Skull.transform.position, m_Target);
            if (dist < m_EndChompDistance && !m_IsEndingChomp) {
                StartCoroutine(EndChomp());
            }
        }
        else if (m_Skull.transform.position.y < m_SkullIdlePosY) {
            m_SkullRb.AddForce(Vector3.up * m_Speed * 0.5f, ForceMode2D.Impulse);
        }
    }

    private bool m_IsEndingChomp = false;
    IEnumerator EndChomp() {
        m_IsEndingChomp = true;

        yield return new WaitForSeconds(1f);

        Debug.Log("chomp complete");
        m_IsChomping = false;
        m_SkullRb.velocity = Vector3.zero;
        m_SkullRb.angularVelocity = 0f;
        m_SkullRb.inertia = 0f;
        m_SkullAnimator.SetBool("Chomping", m_IsChomping);
        m_IsEndingChomp = false;
    }

    public void ChompAt(Vector3 target) {
        m_Target = target;
        m_IsChomping = true;
        m_SkullAnimator.SetBool("Chomping", m_IsChomping);
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
