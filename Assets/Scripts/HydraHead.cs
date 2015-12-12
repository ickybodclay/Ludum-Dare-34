using UnityEngine;
using System.Collections;

public class HydraHead : MonoBehaviour {
    [SerializeField] private GameObject m_Skull;
    [SerializeField] private GameObject m_NeckStart;

    public void AttachToBody(GameObject body) {
        transform.parent = body.transform.parent;
        m_NeckStart.GetComponent<DistanceJoint2D>().connectedBody = body.GetComponent<Rigidbody2D>();
    }

    public void MoveSkull(Vector3 position) {
        m_Skull.transform.position = position;
    }
}
