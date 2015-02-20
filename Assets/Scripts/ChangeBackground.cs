using UnityEngine;
using System.Collections;

public class ChangeBackground : MonoBehaviour {

	public Texture2D[] backgrounds = new Texture2D[5];

	int currentBackground = 0;
	// Use this for initialization
	void Start () {
		if (PlayerPrefs.HasKey ("Background")) {
			currentBackground = PlayerPrefs.GetInt ("Background");
		} else {
			PlayerPrefs.SetInt ("Background", 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
