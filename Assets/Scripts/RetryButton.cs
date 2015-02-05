using UnityEngine;
using System.Collections;

public class RetryButton : MonoBehaviour {

	void OnMouseDown() {
		Application.LoadLevel(Application.loadedLevel);
	}

}
