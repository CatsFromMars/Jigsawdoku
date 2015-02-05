using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Serializable2DIntArray {
    public int[] array;
}

public class PieceWrapper : MonoBehaviour {

    public Color color;
    public Material[] tileSprites;
    public Serializable2DIntArray[] serializablePieceNumbers;
    public GameObject board;
    public BoardWrapper wrapper;

    private Piece piece;
    private GameObject numberContainer;
    private BoxCollider2D collider;
    private Vector3 screenPoint;
    private Quaternion targetRotation;

	void Awake() {
		board = GameObject.FindGameObjectWithTag("Board");
		wrapper = board.GetComponent<BoardWrapper>();
	}

    void Start() {
        piece = Piece.fromSerializable2DIntArray(serializablePieceNumbers);

        // Adding tiles in a wrapper object
        numberContainer = new GameObject("numbers");
        numberContainer.transform.parent = transform;

        int[,] pieceNumbers = piece.to2DArray();
        int count = 0;

        Vector3 centerOffset = new Vector3((pieceNumbers.GetLength(1)-1)/2.0f, -(pieceNumbers.GetLength(0)-1)/2.0f, 0);

        for (int i = 0; i < piece.getHeight(); i++) {
            for (int j = 0; j < piece.getWidth(); j++) {
                // Create quads for non-zero numbers
                if (pieceNumbers[i, j] >= 1 && pieceNumbers[i, j] <= 9) {
                    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    quad.transform.parent = numberContainer.transform;

                    quad.transform.localPosition = new Vector3(j, -i, 0) - centerOffset;
                    // Set the texture to the corresponding number
                    quad.renderer.material = tileSprites[pieceNumbers[i, j] - 1];
                    quad.renderer.material.color = color;
                                        

                    quad.AddComponent<ForceTileUpright>();
                    quad.collider.isTrigger = true;
                    quad.layer = 2;
                    count ++;
                }
            }
        }
                

        // Instantiates the center and edges collider at the center of the parent piece

        collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(piece.getWidth(), piece.getHeight());

        targetRotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(rotatePiece());
    }
    
    void Update() {
        handleInput();

        if (!Input.GetMouseButton(0)) {
            snapPieces();
        }
    }

    void handleInput() {
        // Placeholder controls for 
        if (Input.GetKeyDown(KeyCode.Z)) {
            rotateClockwise();
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            rotateCounterClockwise();
        }
    }

    void snapPieces() {
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
            transform.position = new Vector3(x, y, z);
        }
    }
    
    void rotateClockwise() {
        targetRotation *= Quaternion.Euler(0, 0, 90);
		piece.rotateClockwise();
    }
    
    void rotateCounterClockwise() {
        targetRotation *= Quaternion.Euler(0, 0, -90);
        piece.rotateCounterClockwise();
    }


	IEnumerator rotatePiece() {
        while (true) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 10);
            yield return 0;
        }
	}

	void OnMouseDrag()
	{
		wrapper.selectedPiece = this.piece;
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
		transform.position = new Vector3(curPosition.x, curPosition.y, 0);
	}

    public Piece getPiece() {
        return piece;
    }
}
