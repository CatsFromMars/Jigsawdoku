using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour {

    private Color targetColor = Color.white;
	
	void Update () {
        Color lerpColor = Color.Lerp(renderer.material.color, targetColor, Time.deltaTime);
        renderer.material.color = lerpColor;
	}

    public void setColor(Color c) {
        targetColor = c;
    }
}
