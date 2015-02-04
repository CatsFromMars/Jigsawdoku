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
    private Piece piece;
    private GameObject numberContainer;
    private BoxCollider2D collider;
	private Vector3 screenPoint;

    private Quaternion targetRotation;

    void Start() {
        piece = Piece.fromSerializable2DIntArray(serializablePieceNumbers);

        // Adding tiles in a wrapper object
        numberContainer = new GameObject("numbers");
        numberContainer.transform.parent = transform;

        int[,] pieceNumbers = piece.to2DArray();
        int count = 0;

        Vector3 centerOffset = new Vector3(pieceNumbers.GetLength(0)/4.0f, -pieceNumbers.GetLength(1)/2.0f, 0);

        for (int i = 0; i < pieceNumbers.GetLength(0); i++) {
            for (int j = 0; j < pieceNumbers.GetLength(1); j++) {
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
        collider.size = new Vector2(pieceNumbers.GetLength(1), pieceNumbers.GetLength(0));

        targetRotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(rotatePiece());
    }
    
    void Update() {
        handleInput();
    }

    void handleInput() {
        // Placeholder controls
        if (Input.GetKeyDown(KeyCode.Z)) {
            rotateClockwise();
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            rotateCounterClockwise();
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 10);//Quaternion.Lerp(transform.rotation, targetRotation, Time.time*rotationSpeed);
            yield return 0;
        }
	}

	void translatePiece() {

	}
	
	
    void clickAndDrag() {
    
    }

	void OnMouseDrag()
	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
		transform.position = new Vector3(curPosition.x, curPosition.y, 0);
	}

    public Piece getPiece() {
        return piece;
    }
}
