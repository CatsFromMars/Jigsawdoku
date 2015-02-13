using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
	public float curTime;
	public int score;
	public bool gameOver;
	
	Text text;
	
	
	void Awake (){
		text = GetComponent <Text> ();
		curTime = 0;
		score = 50000;
	}

	void Update (){
		if(!gameOver) curTime += Time.deltaTime;
		score=Mathf.FloorToInt(50000f-curTime*55f);
		score /= 100;
		int min = Mathf.FloorToInt(curTime)/60;
		int sec = Mathf.FloorToInt (curTime) % 60;
		if(sec<10)
			text.text = min + ":0" + sec+ "\n"+"\n"+ " score: "+score;
		else
			text.text = min + ":" + sec+"\n"+"\n"+ " score: "+score;
	}
}
