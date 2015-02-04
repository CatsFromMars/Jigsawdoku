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
    private CircleCollider2D edgeCollider;
    bool move = false;

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

        edgeCollider = gameObject.AddComponent<CircleCollider2D>();
        edgeCollider.radius = 1;
        move = false;
    }
    
    void Update() {
        handleInput();
    }

    void OnMouseOver() {
        Debug.Log("hello");
        if (Input.GetMouseButtonDown(0)) { // Left Click
            rotateClockwise();
        }
        if (Input.GetMouseButtonDown(1)) { // Right Click
            rotateCounterClockwise();
        }
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
        transform.Rotate(0, 0, 90);
        piece.rotateClockwise();
    }
    
    void rotateCounterClockwise() {
        transform.Rotate(0, 0, -90);
        piece.rotateCounterClockwise();
    }
    
    void clickAndDrag() {
                
    }

    public Piece getPiece() {
        return piece;
    }
}
