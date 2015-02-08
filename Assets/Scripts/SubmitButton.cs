using UnityEngine;
using System.Collections;

public class SubmitButton : _PressableButton {
    public Material normalSprite;
    public Material hoverSprite;
    public Material pressedSprite;
    
    override
    public Material getNormalSprite() {
		transform.rotation = Quaternion.identity;
        return normalSprite;
    }
    
    override
    public Material getHoverSprite() {
		transform.Rotate (Vector3.forward * -90 * Time.deltaTime * 5);
        return hoverSprite;
    }
    
    override
    public Material getPressedSprite() {
        return pressedSprite;
    }
    
    override
    public Vector2 getDimensions() {
        return new Vector2(256, 256);
    }
    
    override
    public float getScaleFactor() {
        return 80;
    }
    
    override
    public void onButtonPressed() {
        BoardWrapper boardWrapper = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardWrapper>();
        Board board = boardWrapper.getBoard();
        
        if (!board.isComplete()) {
            GameOver();
        } else {
            WinnerIsYou();
        }
    }

    /*
    private GameObject quad;

	void Start () {
        GameObject boardContainer = GameObject.FindGameObjectWithTag("Board");
        boardWrapper = boardContainer.GetComponent<BoardWrapper>();

        quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.parent = transform;
        quad.transform.localPosition = new Vector3(0, 0, 0);
        quad.transform.localScale = new Vector3(2142.0f/500, 1145.0f/500, 1);
        
        quad.renderer.material = sprites[0];
        
        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(2142.0f/550, 1145.0f/550);
	}

    void OnMouseDown() {
        quad.renderer.material = sprites[2];
	}

    void OnMouseUpAsButton() {
        Board board = boardWrapper.getBoard();
        
        if (!board.isComplete()) {
            GameOver();
        } else {
            WinnerIsYou();
        }
    }
    */
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
