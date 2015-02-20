using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{

		public int maxLevel = 15;
		public int[] levelExp = new int[15];
		protected int currentExp = 0;
		protected int maxExp = 0;
		protected int currentLevel = 1;
		public Slider experienceBar;
		public Text level; 
		
		// Use this for initialization
		void Start ()
		{
				maxExp = levelExp [currentLevel - 1];

				// set or get experience and level
				if (PlayerPrefs.HasKey ("Experience")) {
						currentExp = PlayerPrefs.GetInt ("Experience");
				} else {
						PlayerPrefs.SetInt ("Experience", 0);
				}

				if (PlayerPrefs.HasKey ("Level")) {
						currentLevel = PlayerPrefs.GetInt ("Level");
						maxExp = levelExp [currentLevel - 1];
				} else {
						PlayerPrefs.SetInt ("Level", 1);
				}
				
				experienceBar.value = (currentExp * 1.0f) / (maxExp * 1.0f);
				if (currentLevel < maxLevel)
						level.text = "lv " + currentLevel.ToString ();
				else {
						level.text = "lv Max";
				}


		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void addExp (int score)
		{
				// also update exp and level for playerprefs
				if (currentLevel == maxLevel) 
						return;
				int temp = currentExp;
				currentExp += score; 
				if (currentExp > maxExp) {
						if (currentLevel < maxLevel - 1) {
								currentLevel += 1;
				
								PlayerPrefs.SetInt ("Level", currentLevel);
								currentExp = score - (maxExp - temp); // reset score
								maxExp = levelExp [currentLevel - 1];
						} else if (currentLevel == maxLevel - 1) {
								currentLevel += 1;
				
								PlayerPrefs.SetInt ("Level", currentLevel);
								maxExp = levelExp [currentLevel - 1];
								currentExp = maxExp;

						} else {
								currentExp = maxExp;
						}
			
				}

				
				
				PlayerPrefs.SetInt ("Experience", currentExp);
				PlayerPrefs.Save ();
				// now just need to check 
				experienceBar.value = (currentExp * 1.0f) / (maxExp * 1.0f);
				if (currentLevel < maxLevel)
						level.text = "lv " + currentLevel.ToString ();
				else {
						level.text = "lv Max";
				}
		}


}
