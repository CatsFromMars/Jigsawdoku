using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeBackground : MonoBehaviour
{


		//BUTTONS
		public GameObject original;
		public GameObject background1;
		public GameObject background2;
		public GameObject background3;
		public GameObject background4;
		public GameObject background5;

		public bool options;
		public Texture2D[] textures = new Texture2D[6];
		int currentBackground = 0;
		public GameObject background;
		
		// text
	public Text[] unlockText = new Text[5];
		
		void Awake ()
		{	
				if (!options) 
						return;
				if (PlayerPrefs.GetInt ("Level") > 2) {
						background1.GetComponent<Button> ().interactable = true;
			unlockText[0].enabled = false;
				}
				if (PlayerPrefs.GetInt ("Level") > 5) {
						background2.GetComponent<Button> ().interactable = true;
			unlockText[1].enabled = false;
				}
		
				if (PlayerPrefs.GetInt ("Level") > 8) {
						background3.GetComponent<Button> ().interactable = true;
			unlockText[2].enabled = false;
				}

				if (PlayerPrefs.GetInt ("Level") > 11) {
						background4.GetComponent<Button> ().interactable = true;
			unlockText[3].enabled = false;
				}

				if (PlayerPrefs.GetInt ("Level") > 14) {
						background5.GetComponent<Button> ().interactable = true;
			unlockText[4].enabled = false;
				}
		}
		// Use this for initialization
		void Start ()
		{
				if (PlayerPrefs.HasKey ("Background")) {
						currentBackground = PlayerPrefs.GetInt ("Background");
				} else {
						PlayerPrefs.SetInt ("Background", 0);
			PlayerPrefs.Save ();
		}

				background.renderer.material.mainTexture = textures [currentBackground];
				
		}

		public void Original ()
		{
				PlayerPrefs.SetInt ("Background", 0);
				PlayerPrefs.Save ();
				currentBackground = 0;
				background.renderer.material.mainTexture = textures [currentBackground];
		}
	
		public void Background1 ()
		{
				PlayerPrefs.SetInt ("Background", 1);
				PlayerPrefs.Save ();
				currentBackground = 1;
				background.renderer.material.mainTexture = textures [currentBackground];
		}
	
		public void Background2 ()
		{
				PlayerPrefs.SetInt ("Background", 2);
				PlayerPrefs.Save ();
				currentBackground = 2;
				background.renderer.material.mainTexture = textures [currentBackground];
		}
	
		public void Background3 ()
		{
				PlayerPrefs.SetInt ("Background", 3);
				PlayerPrefs.Save ();
				currentBackground = 3;
				background.renderer.material.mainTexture = textures [currentBackground];
		}
	
		public void Background4 ()
		{
				PlayerPrefs.SetInt ("Background", 4);
				PlayerPrefs.Save ();
				currentBackground = 4;
				background.renderer.material.mainTexture = textures [currentBackground];
		}

		public void Background5 ()
		{
				PlayerPrefs.SetInt ("Background", 5);
				PlayerPrefs.Save ();
				currentBackground = 5;
				background.renderer.material.mainTexture = textures [currentBackground];
		}


	public void ChangeBackgroundMenu ()
	{
		Application.LoadLevel ("change_background");
	}
	
}
