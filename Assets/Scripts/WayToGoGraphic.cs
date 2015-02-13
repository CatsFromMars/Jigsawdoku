using UnityEngine;
using System.Collections;

public class WayToGoGraphic : MonoBehaviour {

	private float speed = 40f;

	void Update() {
		Vector3 target = new Vector3(0, 0, -5);
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, target, step);
	}

}
