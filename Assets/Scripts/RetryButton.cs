using UnityEngine;
using System.Collections;

public class RetryButton : _PressableButton {

    public Material normalSprite;
    public Material hoverSprite;
    public Material pressedSprite;

    override
    public Material getNormalSprite() {
        return normalSprite;
    }

    override
    public Material getHoverSprite() {
        return hoverSprite;
    }

    override
    public Material getPressedSprite() {
        return pressedSprite;
    }
    
    override
    public Vector2 getDimensions() {
        return new Vector2(603, 286);
    }
    
    override
    public float getScaleFactor() {
        return 120;
    }
    
    override
    public void onButtonPressed() {
        Application.LoadLevel(Application.loadedLevel);
    }
}
