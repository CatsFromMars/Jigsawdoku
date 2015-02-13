using UnityEngine;
using System.Collections;

public class BackgroundAnimation : MonoBehaviour {
    public float speedX;
    public float speedY;

	void Update () {
        Vector2 mouseMovement = new Vector2(0, 0);

        if (Input.GetMouseButton(0)) {
            mouseMovement = new Vector2(Input.GetAxis("Mouse X"), 0) * 0.05f;
        }

        transform.Rotate(new Vector3(0, 0, 0.001f));
        renderer.material.mainTextureOffset += (new Vector2(speedX, speedY) + mouseMovement) * Time.deltaTime;
	}
}
