using UnityEngine;
using System.Collections;

public class RetryButton : MonoBehaviour {

    public Material[] sprites;
    private GameObject quad;

    void Start() {
        quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.parent = transform;
        quad.transform.localPosition = new Vector3(0, 0, 0);
        quad.transform.localScale = new Vector3(2142.0f/500, 1145.0f/500, 1);

        quad.renderer.material = sprites[0];

        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(2142.0f/550, 1145.0f/550);
    }

    void OnMouseUpAsButton() {
        Application.LoadLevel(Application.loadedLevel);
    }
    
    void OnMouseDown() {
        quad.renderer.material = sprites[2];
    }

    void OnMouseEnter() {
        quad.renderer.material = sprites[1];
    }

    void OnMouseExit() {
        quad.renderer.material = sprites[0];
    }

}
