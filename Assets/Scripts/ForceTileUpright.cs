using UnityEngine;
using System.Collections;

public class ForceTileUpright : MonoBehaviour {
	void Update () {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
	}
}
