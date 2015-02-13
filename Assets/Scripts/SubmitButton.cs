using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SubmitButton : _PressableButton {
	public int width = 768;
	public int height = 246;
    public Material normalSprite;
    public Material hoverSprite;
    public Material pressedSprite;
	private GameObject boardContainer;
	private BoardWrapper boardWrapper;
	private GameObject mainAudio;
	private AudioSource mainAudioSource;
	GameObject errorObject;
	Text errorText;

	void Awake() {
		boardContainer = GameObject.FindGameObjectWithTag("Board");
		boardWrapper = boardContainer.GetComponent<BoardWrapper>();
		mainAudio = GameObject.FindGameObjectWithTag("Audio");
		mainAudioSource = mainAudio.GetComponent<AudioSource>();
		errorObject = GameObject.FindGameObjectWithTag("ErrorText");
		//errorText = errorObject.GetComponent<Text>();
	}
    
    override
    public Material getNormalSprite() {
        return normalSprite;
    }
    
    override
    public Material getHoverSprite() {
        return hoverSprite;
    }
    
    override
    public Material getPressedSprite() {
        return pressedSprite;
    }
    
    override
    public Vector2 getDimensions() {
        return new Vector2(width, height);
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
		mainAudioSource.clip = Resources.Load ("Audio/Yay") as AudioClip;
		mainAudioSource.Play();

		StartCoroutine(PieceConfetti());
		Instantiate(Resources.Load ("Waytogo") as GameObject, new Vector3(23,0,-5), Quaternion.identity);
		//errorText.text = "";

		GameObject timerContainer = GameObject.FindGameObjectWithTag("timer");
        Timer timer = timerContainer.GetComponentInChildren<Timer>();

        StartCoroutine(PieceConfetti());

		timer.gameOver=true;

		//SET PLAYER HIGH SCORE IN DIFFICULTY
		if(timer.score > PlayerPrefs.GetInt(Application.loadedLevelName)) {
			PlayerPrefs.SetInt(Application.loadedLevelName, timer.score);
			PlayerPrefs.Save();
		}

		//Instantiate(Resources.Load ("CelebrationStars"), transform.position, Quaternion.identity);

		//play "you win!" sting
		//display "you win! text"
		//bigText.text = "Bingo Tiger!";
	}

	IEnumerator PieceConfetti() {

		GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
		for(int i = 0; i < pieces.Length; i++) {
			GameObject bit = pieces[i];
			PieceWrapper wrapper = bit.GetComponent<PieceWrapper>();
			Color pieceColor = wrapper.backColor;
			pieceColor.a = 255;
			Destroy(bit.gameObject);
			GameObject confetti = Resources.Load("Confetti") as GameObject;
			ParticleSystem confettiParticles = confetti.GetComponent<ParticleSystem>();
			confettiParticles.startColor = pieceColor;
			Instantiate(confetti, bit.transform.position, Quaternion.Euler(-90,0,0));
			yield return new WaitForSeconds(0.3f);
			yield return 0;
		}

		//WHEN THIS COROUTINE IS OVER, RESTART THE GAME!
		yield return new WaitForSeconds(0.5f);
		Application.LoadLevel(Application.loadedLevel);

	}

	void GameOver() {
		//dropPieces(); //Drop the pieces
		mainAudioSource.clip = Resources.Load ("Audio/Wrong") as AudioClip;
		mainAudioSource.Play();
		//Board board = boardWrapper.getBoard();
		//errorText.text = board.getMostRecentError();
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
