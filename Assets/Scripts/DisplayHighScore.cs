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
		else if(level == difficulty.Normal) {
			score = PlayerPrefs.GetInt("NormalGame",0);  
			text.text = "Normal: "+score.ToString();
		}
		else if(level == difficulty.Hard) {
			score = PlayerPrefs.GetInt("HardGame",0);  
			text.text = "Hard: "+score.ToString();
		}
		else if(level == difficulty.Lunatic) {
			score = PlayerPrefs.GetInt("LunaticGame",0);  
			text.text = "Lunatic: "+score.ToString();
		}
		else if(level == difficulty.Extra) {
			score = PlayerPrefs.GetInt("ExtraGame",0);  
			text.text = "Extra: "+score.ToString();
		}

	}

}
