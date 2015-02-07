using UnityEngine;
using System.Collections;

public abstract class _PressableButton : MonoBehaviour {

    private GameObject quad;

    public abstract Material getNormalSprite();
    
    public abstract Material getHoverSprite();
    
    public abstract Material getPressedSprite();
    
    public abstract Vector2 getDimensions();
    
    public abstract float getScaleFactor();
    
    public abstract void onButtonPressed();

    void Start () {
        quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

        quad.transform.parent = transform;
        quad.transform.localPosition = new Vector3(0, 0, 0);

        Vector2 dim = getDimensions();
        quad.transform.localScale = new Vector3(dim.x/getScaleFactor(), dim.y/getScaleFactor(), 1);

        quad.renderer.material = getNormalSprite();

        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = dim/(getScaleFactor()*1.1f);
    }
    
    void OnMouseUpAsButton() {
        onButtonPressed();
        quad.renderer.material = getNormalSprite();
    }

    void OnMouseOver() {
        if (!Input.GetMouseButton(0)) {
            quad.renderer.material = getHoverSprite();
        } else {
            quad.renderer.material = getPressedSprite();
        }
    }
    
    void OnMouseExit() {
        quad.renderer.material = getNormalSprite();
    }
}
