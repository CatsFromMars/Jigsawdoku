using UnityEngine;
using System.Collections;

public class SubmitButton : MonoBehaviour {
    private BoardWrapper boardWrapper;

	// Use this for initialization
	void Start () {
        GameObject boardContainer = GameObject.FindGameObjectWithTag("Board");
        boardWrapper = boardContainer.GetComponent<BoardWrapper>();
	}

	void OnMouseDown() {
		//When one clicks on the button...
        Board board = boardWrapper.getBoard();

		if (!board.isComplete()) {
            Debug.Log("Board isn't solved yet!");
            GameOver();
        } else {
            Debug.Log("You win!");
            WinnerIsYou();
        }
	}

	void WinnerIsYou() {
		Instantiate(Resources.Load ("CelebrationStars"), transform.position, Quaternion.identity);
		//play "you win!" sting
		//display "you win! text"
		//bigText.text = "Bingo Tiger!";
	}

	void GameOver() {
		dropPieces(); //Drop the pieces
		//play "Oh no! Sound effect"
		//also maybe darken the screen a bit?
		//Camera shake?
		//Display Text
		//bigText.text = "Not quite right...";
	}

	void dropPieces() {
        /*
		GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
		foreach(GameObject piece in pieces) {

			// Rigidbody rigidbody = piece.AddComponent<Rigidbody>();
			// rigidbody.velocity = new Vector2 (0, Random.Range(-5F, -10F));
		}
        */
	}
}
