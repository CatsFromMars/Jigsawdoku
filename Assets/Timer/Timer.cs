using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
	public float curTime;
	public int score;
	public int deltTime;
	public int timeBonus;
	public bool gameOver;
	public Difficulty difficulty;
	private float dif;
	private int baseScore;

	Text text;

	void Awake (){
		text = GetComponent <Text> ();
		curTime = 0;
		switch (difficulty) {
		case Difficulty.Easy:
			dif=1f;
			timeBonus=5*60;
			baseScore=10000;
			break;
		case Difficulty.Normal:
			dif=1.5f;
			timeBonus=10*60;
			baseScore=15000;
			break;
		case Difficulty.Hard:
			dif=2.5f;
			timeBonus=15*60;
			baseScore=25000;
			break;
		case Difficulty.Lunatic:
			dif=5f;
			timeBonus=20*60;
			baseScore=50000;
			break;
		case Difficulty.Extra:
			dif=8f;
			timeBonus=25*60;
			baseScore=100000;
			break;
		default:
			dif=0;
			break;
		}
		score = baseScore;
	}

	void Update (){
		if (!gameOver) {
				curTime += Time.deltaTime;
				int min = Mathf.FloorToInt (curTime) / 60;
				int sec = Mathf.FloorToInt (curTime) % 60;
				if (sec < 10)
					text.text = min + ":0" + sec;
				else
					text.text = min + ":" + sec;
		}else {
			deltTime = Mathf.RoundToInt(timeBonus-curTime);
			if(deltTime>0){
				score=baseScore+Mathf.RoundToInt(deltTime/30)*(deltTime/30)*90;
			}else{
				score=baseScore;
			}
			text.text="Score: "+score;
		}
	}
}
