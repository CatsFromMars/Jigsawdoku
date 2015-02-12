using UnityEngine;
using System.Collections;

public class QuitButton : _PressableButton {
	
	public Material normalSprite;
	public Material hoverSprite;
	public Material pressedSprite;
	public int width = 512;
	public int height = 164;
	override
	public Material getNormalSprite() {
		transform.rotation = Quaternion.identity;
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
		return new Vector2(width, height);
	}
	
	override
	public float getScaleFactor() {
		return 80;
	}
	
	override
	public void onButtonPressed() {
		Application.Quit();
	}
}