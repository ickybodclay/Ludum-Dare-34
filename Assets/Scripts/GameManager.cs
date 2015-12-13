using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
#if UNITY_WEBGL
        Camera.main.GetComponent<OutlineEffect>().flipY = false;
#else
        Camera.main.GetComponent<OutlineEffect>().flipY = true;
#endif
    }

    public void HighlightHead(HydraHead head) {
        OutlineEffect effect = Camera.main.GetComponent<OutlineEffect>();
        effect.outlineRenderers.Clear();
        effect.outlineRenderers.Add(head.GetComponentInChildren<SpriteRenderer>());
    }

}
