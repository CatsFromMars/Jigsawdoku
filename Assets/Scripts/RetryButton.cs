using UnityEngine;
using System.Collections;

public class RetryButton : _PressableButton {

    public Material normalSprite;
    public Material hoverSprite;
    public Material pressedSprite;
	public int width = 256;
	public int height = 256;
    override
    public Material getNormalSprite() {
		transform.rotation = Quaternion.identity;
        return normalSprite;
    }

    override
    public Material getHoverSprite() {
		transform.Rotate (Vector3.forward * -90 * Time.deltaTime * 5);
        return hoverSprite;
    }

    override
    public Material getPressedSprite() {
        return pressedSprite;
    }
    
    override
    public Vector2 getDimensions() {
        return new Vector2(width, height);
    }
    
    override
    public float getScaleFactor() {
        return 80;
    }
    
    override
    public void onButtonPressed() {
        Application.LoadLevel(Application.loadedLevel);

    }
}
