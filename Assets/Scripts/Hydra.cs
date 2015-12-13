using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Hydra : MonoBehaviour {
    [SerializeField] private GameObject m_HeadPrefab;
    [SerializeField] private GameObject m_Body;
    [SerializeField] private Image m_FoodBar;

    private AudioSource m_AudioSource;

    private List<HydraHead> m_Heads = new List<HydraHead>();
    private int m_CurrentHeadIndex;
    private int m_FoodTotal;
    private int m_FoodNeededToGrow;

    private void Start () {
        m_AudioSource = GetComponent<AudioSource>();

        GrowHead();
        GameManager.instance.HighlightHead(m_Heads[m_CurrentHeadIndex]);

        m_FoodNeededToGrow = 10;
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
            if (CanGrow()) {
                GrowHead();
            }
            else {
                m_AudioSource.pitch = 1f;
                //m_AudioSource.clip = null;
                m_AudioSource.Play();
            }
        }
    }

    private void UpdateUI() {
        m_FoodBar.fillAmount = m_FoodTotal > m_FoodNeededToGrow ? 1f : (float)m_FoodTotal / m_FoodNeededToGrow;
    }

    private void Chomp() {
        if (m_Heads.Count <= 0) return;

        m_Heads[m_CurrentHeadIndex].ChompAt(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)));

        SetCurrentHead((m_CurrentHeadIndex + 1) % m_Heads.Count);
    }

    private void GrowHead() {
        m_FoodTotal -= m_FoodNeededToGrow;

        GameObject head = Instantiate(m_HeadPrefab, transform.position, Quaternion.identity) as GameObject;
        HydraHead newHead = head.GetComponent<HydraHead>();
        newHead.AttachToBody(m_Body);
        m_Heads.Add(newHead);

        SetCurrentHead(m_Heads.Count - 1);

        m_AudioSource.pitch = Random.Range(.8f, 1f);
        m_AudioSource.Play();

        m_FoodNeededToGrow *= 2;
    }

    private void LoseHead(int headIndex) {
        if (m_Heads.Count == 0) {
            Die();
            return;
        }

        m_Heads.RemoveAt(headIndex);

        if (m_Heads.Count == 0) {
            Die();
            return;
        }

        m_FoodNeededToGrow /= 2;
        if (m_FoodNeededToGrow < 10) m_FoodNeededToGrow = 10;
    }

    private void Die() {
        Debug.Log("you died!");
    }

    public void AddFood(int food) {
        m_FoodTotal += food;
    }

    public void SetCurrentHead(int index) {
        m_Heads[m_CurrentHeadIndex].EndFocus();

        m_CurrentHeadIndex = index;
        GameManager.instance.HighlightHead(m_Heads[m_CurrentHeadIndex]);

        m_Heads[m_CurrentHeadIndex].Focus();
    }

    public bool CanGrow() {
        return m_FoodTotal >= m_FoodNeededToGrow;
    }
}
