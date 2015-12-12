using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class Hydra : MonoBehaviour {
    [SerializeField] private GameObject m_HeadPrefab;
    [SerializeField] private GameObject m_Body;
    [SerializeField] private Text m_PeasantsEatenText;

    private List<HydraHead> m_Heads = new List<HydraHead>();
    private int m_CurrentHeadIndex;
    private int m_FoodTotal;

    private void Start () {
        GrowHead();
        GameManager.instance.HighlightHead(m_Heads[m_CurrentHeadIndex]);
    }
	
	private void Update () {
        HandleInput();
        UpdateUI();
	}

    private void HandleInput() {
        if (Input.GetMouseButtonDown(0)) {
            Chomp();
        }

        if (Input.GetMouseButtonDown(1)) {
            GrowHead();
        }
    }

    private void UpdateUI() {
        m_PeasantsEatenText.text = string.Format("Peasants eaten = {0}", m_FoodTotal);
    }

    private void Chomp() {
        if (m_Heads.Count <= 0) return;

        m_Heads[m_CurrentHeadIndex].ChompAt(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)));
        m_CurrentHeadIndex = (m_CurrentHeadIndex + 1) % m_Heads.Count;

        GameManager.instance.HighlightHead(m_Heads[m_CurrentHeadIndex]);
    }

    private void GrowHead() {
        GameObject head = Instantiate(m_HeadPrefab, transform.position, Quaternion.identity) as GameObject;
        HydraHead newHead = head.GetComponent<HydraHead>();
        newHead.AttachToBody(m_Body);
        m_Heads.Add(newHead);
    }

    public void AddFood(int food) {
        m_FoodTotal += food;
    }
}
