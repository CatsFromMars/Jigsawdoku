using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Serializable2DIntArray {
    public int[] array;
}

public class PieceWrapper : MonoBehaviour {

    public Color numberColor;
    public Color backColor;
    public Material[] numberSprites;
    public Material backSprite;
    public Material silhouette;
    public Serializable2DIntArray[] serializablePieceNumbers;

    private GameObject boardContainer;
    private BoardWrapper boardWrapper;
    private Piece piece;
    private GameObject numberContainer;
    private new MeshCollider collider;
    private Quaternion targetRotation;
    private Vector2 localMouseXY;
    private int rotateDelay;
    private GameObject mainAudio;
    private AudioSource mainAudioSource;
    private Piece selectedPiece;
    private GameObject rotIcon;
    private GameObject rotIconBottom;
    private GameObject displayedRotationIconTop;
    private GameObject displayedRotationIconBottom;
    private bool snapped;
    private bool hint;

	//for touch
	private bool rotating;
	private bool isHolding;
	private Vector2 startVector;

    public void Awake() {
        boardContainer = GameObject.FindGameObjectWithTag("Board");
        boardWrapper = boardContainer.GetComponent<BoardWrapper>();
        mainAudio = GameObject.FindGameObjectWithTag("Audio");
        mainAudioSource = mainAudio.GetComponent<AudioSource>();
        rotIcon = Resources.Load("XIcon") as GameObject;
        rotIconBottom = Resources.Load("ZIcon") as GameObject;
    }

    public void Start() {
        if (serializablePieceNumbers != null && serializablePieceNumbers.Length > 0) {
            piece = Piece.fromSerializable2DIntArray(serializablePieceNumbers);
        }

        if (piece == null) {
            Destroy(gameObject);
        }

        // Adding tiles in a wrapper object
        numberContainer = new GameObject("numbers");
        numberContainer.transform.parent = transform;
        numberContainer.transform.localPosition = new Vector3(0, 0, 0);

        int[,] pieceNumbers = piece.to2DArray();

        Vector3 centerOffset = new Vector3((pieceNumbers.GetLength(1) - 1) / 2.0f, -(pieceNumbers.GetLength(0) - 1) / 2.0f, 0);

        for (int i = 0; i < piece.getHeight(); i++) {
            for (int j = 0; j < piece.getWidth(); j++) {
                // Create quads for valid numbers
                if (pieceNumbers[i, j] >= 1 && pieceNumbers[i, j] <= 9) {

                    GameObject numberTile = new GameObject(pieceNumbers[i, j] + "");
                    numberTile.transform.parent = numberContainer.transform;
                    numberTile.transform.localPosition = new Vector3(j, -i, 0) - centerOffset;

                    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    GameObject backQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    GameObject outlineQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    quad.transform.parent = numberTile.transform;
                    backQuad.transform.parent = numberTile.transform;
                    outlineQuad.transform.parent = numberTile.transform;

                    quad.transform.localPosition = Vector3.zero;
                    backQuad.transform.localPosition = Vector3.zero;
                    outlineQuad.transform.localPosition = new Vector3(0, 0, 0.1f);
                    outlineQuad.transform.localScale = new Vector3(1.05f, 1.05f, 1);

                    // Set the texture to the corresponding number
                    quad.renderer.material = numberSprites[pieceNumbers[i, j] - 1];
                    quad.renderer.material.color = numberColor;

                    backQuad.renderer.material = backSprite;
                    backQuad.renderer.material.color = backColor;

                    outlineQuad.renderer.material = silhouette;
                    outlineQuad.renderer.material.color = numberColor;

                    Destroy(quad.GetComponent<MeshCollider>());
                    Destroy(backQuad.GetComponent<MeshCollider>());
                    Destroy(outlineQuad.GetComponent<MeshCollider>());

                    numberTile.AddComponent<ForceTileUpright>();
                }
            }
        }

        if (!hint) {
            Vector3 savePosition = transform.position;
            Quaternion saveRotation = transform.rotation;
        
            transform.position = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);

            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            for (int i = 0; i < meshFilters.Length; i++) {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            }

            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh.CombineMeshes(combine);
        
            collider = gameObject.AddComponent<MeshCollider>();
            collider.sharedMesh = meshFilter.mesh;

            transform.position = savePosition;
            transform.rotation = saveRotation;
        }

        targetRotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(rotatePiece());

        float x = transform.position.x;
        float y = transform.position.y;
        
        float newX = Mathf.Clamp(x, -13, 13);
        float newY = Mathf.Clamp(y, -10, 5);
        
        transform.position = new Vector3(newX, newY, 0);

        if (hint) {
            float xOffset = (piece.getWidth()-1)/2.0f;
            float yOffset = (piece.getHeight()-1)/2.0f;

            int correctRow = (int)(piece.getCorrectPosition().y);
            int correctCol = (int)(piece.getCorrectPosition().x);
            
            float correctY = 4 - correctRow - yOffset;
            float correctX = correctCol - 4 + xOffset;

            transform.position = new Vector3(correctX, correctY, 0);

            snapPiece();
        }
    }

    void Update() {
        if (rotateDelay > 0) {
            rotateDelay--;
        }

		if (Input.touchCount > 0) {
			if (Input.touchCount == 1) {
				if(Input.GetTouch(0).phase == TouchPhase.Began){
					//check for intersection, will also need to see if top piece, also global is holding
					if(false)
					{
						snapped = false;
						
						Vector3 curScreenPoint = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0);
						Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
						
						float newX = Mathf.Clamp(curPosition.x - localMouseXY.x, -13, 13);
						float newY = Mathf.Clamp(curPosition.y - localMouseXY.y, -10, 5);
						
						transform.position = new Vector3(newX, newY, -1);

						
						GameObject background = GameObject.FindGameObjectWithTag("BackgroundEffect");
						BackgroundController backgroundController = background.GetComponent<BackgroundController>();
						
						backgroundController.setColor(ColorUtils.lightenColor(backColor, 0.2f));
						
						if(rotateDelay != 0) DestroyIcons();
						else if(displayedRotationIconBottom == null && displayedRotationIconTop == null) DisplayIcons();
					}
				}
				else if(Input.GetTouch(0).phase == TouchPhase.Ended && isHolding){

				}
				else if(Input.GetTouch(0).phase == TouchPhase.Moved && isHolding){
					gameObject.SendMessage("OnMouseUp");
					isHolding = false;
				}

				rotating = false;

			} else if (Input.touchCount == 2) {
				if(isHolding){
					if(!rotating){
						startVector = Input.GetTouch(1).position - Input.GetTouch (0).position;
					}
					else{
						var currVector = Input.GetTouch(1).position - Input.GetTouch(0).position;
						var angleOffset = Vector2.Angle(startVector, currVector);
						var LR = Vector3.Cross(startVector, currVector);

						if (angleOffset > 30) {
							if (LR.z > 0) {
								// Anticlockwise turn equal to angleOffset.
								rotateCounterClockwise();
							} else if (LR.z < 0) {
								// Clockwise turn equal to angleOffset.
								rotateClockwise();
							}
						}
					}
				}

			}

		} else {
			rotating = false;
			isHolding = false;
		}
    }

	void DisplayIcons() {
		//Spawn Rotation Icon
		float top = (piece.getHeight()) - (piece.getHeight() / 3);
		Vector2 offset = new Vector2(0, top);
		Vector2 offset2 = new Vector2(0, top * -1);
		Vector2 pos = new Vector2(transform.position.x, transform.position.y);
		
		//Display X
		displayedRotationIconTop = Instantiate(rotIcon, pos + offset, Quaternion.identity) as GameObject;
		//Display Z
		displayedRotationIconBottom = Instantiate(rotIconBottom, pos + offset2, Quaternion.identity) as GameObject;
		
		//parent icon to piece
		displayedRotationIconTop.transform.parent = this.gameObject.transform;
		displayedRotationIconBottom.transform.parent = this.gameObject.transform;
	}

	void DestroyIcons() {
		Destroy(displayedRotationIconTop);
		Destroy(displayedRotationIconBottom);
	}

    void OnMouseDown() {
        //Play audio
        mainAudioSource.clip = Resources.Load("Audio/PickupPiece") as AudioClip;
        mainAudioSource.Play();

		DisplayIcons();

        boardWrapper.destroyErrors();
    }

    void OnMouseDrag() {
        snapped = false;

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        
        float newX = Mathf.Clamp(curPosition.x - localMouseXY.x, -13, 13);
        float newY = Mathf.Clamp(curPosition.y - localMouseXY.y, -10, 5);
        
        transform.position = new Vector3(newX, newY, -1);

        float mouseWheel = Input.GetAxisRaw("Mouse ScrollWheel");

        if (mouseWheel > 0 && rotateDelay == 0) {
            rotateCounterClockwise();
        } else if (mouseWheel < 0 && rotateDelay == 0) {
            rotateClockwise();
        }

        handleInput();

        GameObject background = GameObject.FindGameObjectWithTag("BackgroundEffect");
        BackgroundController backgroundController = background.GetComponent<BackgroundController>();

        backgroundController.setColor(ColorUtils.lightenColor(backColor, 0.2f));

		if(rotateDelay != 0) DestroyIcons();
		else if(displayedRotationIconBottom == null && displayedRotationIconTop == null) DisplayIcons();

    }

    void OnMouseUp() {
        snapPiece();

        if (snapped) {
            // play snapped audio
            mainAudioSource.clip = Resources.Load("Audio/SnapPiece") as AudioClip;
            mainAudioSource.Play();
        } else {
            // play put down audio
            mainAudioSource.clip = Resources.Load("Audio/PutDownPiece") as AudioClip;
            mainAudioSource.Play();
        }

        GameObject background = GameObject.FindGameObjectWithTag("BackgroundEffect");
        BackgroundController backgroundController = background.GetComponent<BackgroundController>();

        backgroundController.setColor(Color.white);

        //Destroy rotation icon
        Destroy(displayedRotationIconTop);
        Destroy(displayedRotationIconBottom);

        boardWrapper.updateErrors();
    }

    void OnMouseEnter() {
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        transform.localScale = new Vector3(1.05f, 1.05f, 1);
    }

    void OnMouseOver() {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);

        if (!Input.GetMouseButton(0)) {
            localMouseXY = new Vector2(curPosition.x - transform.position.x, curPosition.y - transform.position.y);
        }
    }

    void OnMouseExit() {
        if (snapped) {
            transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        } else {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        transform.localScale = Vector3.one;
    }

    void handleInput() {
        // Placeholder controls
        if (Input.GetKeyDown(KeyCode.Z) && rotateDelay == 0) {
            rotateClockwise();
        }
		if (Input.GetKeyDown(KeyCode.X) && rotateDelay == 0) {
            rotateCounterClockwise();
        }
    }

    void snapPiece() {
        float adjustX = (piece.getWidth() - 1) / 2.0f;
        float adjustY = (piece.getHeight() - 1) / 2.0f;

        float minX = -4.5f + adjustX;
        float maxX = 4.5f - adjustX;

        float minY = -4.5f + adjustY;
        float maxY = 4.5f - adjustY;

        if (transform.position.x > minX && transform.position.x < maxX && transform.position.y > minY && transform.position.y < maxY) {
            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;

            if (piece.getWidth() % 2 == 1) {
                x = Mathf.Round(transform.position.x);
            } else {
                x = Mathf.Round(transform.position.x - 0.5f) + 0.5f;
            }

            if (piece.getHeight() % 2 == 1) {
                y = Mathf.Round(transform.position.y);
            } else {
                y = Mathf.Round(transform.position.y - 0.5f) + 0.5f;
            }
            
            Vector3 newPosition = new Vector3(x, y, 1);

            if (boardWrapper.getBoard().canPlacePiece(getPiece(), newPosition)) {
                transform.position = newPosition;
                snapped = true;
            }
        }
    }
    
    void rotateClockwise() {
		//play audio
		mainAudioSource.clip = Resources.Load("Audio/Rotate") as AudioClip;
		mainAudioSource.Play();

		rotateDelay = 20;
        targetRotation *= Quaternion.Euler(0, 0, -90);
        piece.rotateClockwise();
    }
    
    void rotateCounterClockwise() {
		//play audio
		mainAudioSource.clip = Resources.Load("Audio/Rotate") as AudioClip;
		mainAudioSource.Play();

		rotateDelay = 20;
        targetRotation *= Quaternion.Euler(0, 0, 90);
        piece.rotateCounterClockwise();
    }

    IEnumerator rotatePiece() {
        while (true) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 10);
            yield return 0;
        }
    }

    public Piece getPiece() {
        return piece;
    }

    public bool isSnapped() {
        return snapped;
    }

    public bool isHint() {
        return hint;
    }
    
    public void SetData(Piece p, Color c1, Color c2) {
        piece = p;
        numberColor = c1;
        backColor = c2;
    }

    public void makeHint() {
        hint = true;
    }
}
