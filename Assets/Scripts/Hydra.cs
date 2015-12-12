using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hydra : MonoBehaviour {
    [SerializeField] private GameObject m_HeadPrefab;
    [SerializeField] private GameObject m_Body;

    private Rigidbody2D rb;

    private List<HydraHead> m_Heads = new List<HydraHead>();
    private int m_CurrentHeadIndex;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	private void Update () {
        HandleInput();
	}

    private void HandleInput() {
        if (Input.GetMouseButtonDown(0)) {
            Chomp();
        }

        if (Input.GetMouseButtonDown(1)) {
            GrowHead();
        }
    }

    private void Chomp() {
        if (m_Heads.Count <= 0) return;

        m_Heads[m_CurrentHeadIndex].ChompAt(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)));
        m_CurrentHeadIndex = (m_CurrentHeadIndex + 1) % m_Heads.Count;
    }

    private void GrowHead() {
        GameObject head = Instantiate(m_HeadPrefab, transform.position, Quaternion.identity) as GameObject;
        HydraHead newHead = head.GetComponent<HydraHead>();
        newHead.AttachToBody(m_Body);
        m_Heads.Add(newHead);
    }
}
