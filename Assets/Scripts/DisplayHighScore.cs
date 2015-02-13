using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayHighScore : MonoBehaviour {
	public enum difficulty {
		Easy, Normal, Hard, Lunatic, Extra
	}

	public difficulty level;

	int score;
	Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();

		if(level == difficulty.Easy) {
			score = PlayerPrefs.GetInt("EasyGame",0);  
			text.text = "Easy: "+score.ToString();
		}

	}

}
