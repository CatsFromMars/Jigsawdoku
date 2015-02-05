using UnityEngine;
using System.Collections;

public class SubmitButton : MonoBehaviour {
	public bool boardSolved = false; //Boolean determining whether or not the player has solved the board.

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		//When one clicks on the button...
		if(!boardSolved) GameOver();
		else WinnerIsYou();
	}

	void WinnerIsYou() {
		Instantiate(Resources.Load ("CelebrationStars"), transform.position, Quaternion.identity);
		//play "you win!" sting
		//display "you win! text"
	}

	void GameOver() {
		dropPieces(); //Drop the pieces
		//play "Oh no! Sound effect"
		//also maybe darken the screen a bit?
		//Camera shake?
	}

	void dropPieces() {
		GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
		foreach(GameObject piece in pieces) {
			piece.AddComponent("RigidBody2D");
			piece.rigidbody2D.velocity = new Vector2 (0, Random.Range(-5F, -10F));
		}
	}
}
