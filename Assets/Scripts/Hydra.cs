using UnityEngine;
using System.Collections;

public class Hydra : MonoBehaviour {

    [SerializeField]
    private GameObject head1;
    [SerializeField]
    private GameObject head2;
    [SerializeField]
    private GameObject head3;

    private int eatClickCounter;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	private void Update () {
        HandleInput();
	}

    private void HandleInput() {
        if (Input.GetMouseButtonDown(0)) {
            switch (eatClickCounter) {
                case 0:
                    head1.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                    break;
                case 1:
                    head2.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                    break;
                case 2:
                    head3.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                    break;
            }

            eatClickCounter = (eatClickCounter + 1) % 3;
        }
    }
}
