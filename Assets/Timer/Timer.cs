using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
	public int score;
	private float deltTime;
	private float dif;
	private int baseScore;
	private float timeBonus;

	public int updateTime;
	public float curTime;
	public bool gameOver;
	public Difficulty difficulty;

	
	private GameObject levelSystem; 
	Text text;

	void Start (){
		text = GetComponent <Text> ();
		
		levelSystem = GameObject.FindGameObjectWithTag ("Level");
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
		score = 0;
	}

	void Update (){
		if (!gameOver) { //just display time
				curTime += Time.deltaTime;
				int min = Mathf.FloorToInt (curTime) / 60;
				int sec = Mathf.FloorToInt (curTime) % 60;
				if (sec < 10)
					text.text = min + ":0" + sec;
				else
					text.text = min + ":" + sec;
		}else { //display time on gameOver
			if(score==0){ //only calculate score once
				deltTime = (timeBonus-curTime); //timeBonus is max time allowed for a bonus
				if(deltTime>0){ //if has a time bonus
					//score=baseScore+((deltTime/30+1)*(deltTime/30+1))*90;
					float temp = (float)((Mathf.CeilToInt(deltTime)/updateTime+1)*updateTime); //this makes it so score is equal for each "updateTime" interval
					score=baseScore+Mathf.RoundToInt(((float)baseScore)*(temp*temp/timeBonus/timeBonus))/100*100; //ignores last two digits, quadratic decay
				}else{ //if user runs out of time, no time bonus
					score=baseScore;
				}
			}
			text.rectTransform.position = new Vector3(400.0f, 160.0f,0.0f);
			text.text="Base score: "+baseScore+"\nTime Bonus: "+(score-baseScore)+"\nTotal: "+score;

			// add score to highscore if need be
			if(score > PlayerPrefs.GetInt(Application.loadedLevelName)) {
				PlayerPrefs.SetInt(Application.loadedLevelName, score);
				PlayerPrefs.Save();
			}

			// add exp to player's level
			levelSystem.GetComponent<LevelUp> ().addExp (score/3);
		}
	}
}
