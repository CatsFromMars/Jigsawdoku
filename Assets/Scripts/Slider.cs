using UnityEngine;
using System.Collections;

public class Slider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver() {
		Debug.Log ("MOUSEOVER");
		transform.Translate(-10.0f, 0.0f, 0.0f);
	}
}
