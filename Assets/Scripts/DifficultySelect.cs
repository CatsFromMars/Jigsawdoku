using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DifficultySelect : MonoBehaviour
{
		//DIFFICULTY UNLOCK
		public bool hardUnlocked = false;
		public bool lunaticUnlocked = false;
		public bool extraUnlocked = false;
		
		// unlock hard at level 5, lunatic at 10, extra at 15

		//BUTTONS
		public GameObject hard;
		public GameObject lunatic;
		public GameObject extra;

		//TEXT
		public Transform hardText;
		public Transform lunaticText;
		public Transform extraText;
	       
		public void EasyMode ()
		{
				Application.LoadLevel ("EasyGame");
		}

		public void NormalMode ()
		{
				Application.LoadLevel ("NormalGame");
		}

		public void HardMode ()
		{
				if (hardUnlocked)
						Application.LoadLevel ("HardGame");
		}

		public void InsaneMode ()
		{
				if (lunaticUnlocked)
						Application.LoadLevel ("LunaticGame");
		}

		public void ExtraMode ()
		{
				if (extraUnlocked)
					Application.LoadLevel ("ExtraGame");
		}
	

	void Awake ()
		{

		if(hardUnlocked) {
			hard.GetComponent<Button>().interactable = true;
			Destroy(hardText);
		}
		if(lunaticUnlocked) {
			hard.GetComponent<Button>().interactable = true;
			Destroy(lunaticText);
		}
		if(extraUnlocked) {
			hard.GetComponent<Button>().interactable = true;
			Destroy(extraText);
		}

		if (PlayerPrefs.GetInt ("Level") > 4) {
						hardUnlocked = true;
						hard.GetComponent<Button> ().interactable = true;
				}
				if (PlayerPrefs.GetInt ("Level") > 9) {
						lunaticUnlocked = true;
						lunatic.GetComponent<Button> ().interactable = true;
				}

				if (PlayerPrefs.GetInt ("Level") > 14) {
						extraUnlocked = true;
						extra.GetComponent<Button> ().interactable = true;
				}

		}

	public void ReturnToMenu ()
	{
		Application.LoadLevel ("title_screen");
	}

}
