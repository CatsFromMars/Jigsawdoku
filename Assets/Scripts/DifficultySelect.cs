using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DifficultySelect : MonoBehaviour
{
		//DIFFICULTY UNLOCK
		public bool hardUnlocked = false;
		public bool lunaticUnlocked = false;
		public bool extraUnlocked = false;

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
			
			Application.LoadLevel ("HardGame");
		}

        public void InsaneMode ()
        {
        	if(lunaticUnlocked) Application.LoadLevel ("LunaticGame");
        }

        public void ExtraMode ()
        {
             if(extraUnlocked)	Application.LoadLevel ("ExtraGame");
        }

		void Awake() {
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

		}

}
