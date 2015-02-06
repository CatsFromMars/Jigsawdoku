using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
	public float curTime;
	public int score;
	
	Text text;
	
	
	void Awake (){
		text = GetComponent <Text> ();
		curTime = 0;
		score = 50000;
	}

	void Update (){
		curTime += Time.deltaTime;
		score=Mathf.FloorToInt(50000f-curTime*55f);
		int min = Mathf.FloorToInt(curTime)/60;
		int sec = Mathf.FloorToInt (curTime) % 60;
		if(sec<10)
			text.text = min + ":0" + sec;
		else
			text.text = min + ":" + sec;
	}
}
