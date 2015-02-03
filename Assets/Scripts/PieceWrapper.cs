using UnityEngine;
using System.Collections;

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

    void Start() {
        piece = Piece.fromSerializable2DIntArray(serializablePieceNumbers);

        // Adding tiles in a wrapper object
        numberContainer = new GameObject("numbers");
        numberContainer.transform.parent = transform;
        
        int[,] pieceNumbers = piece.to2DArray();
        
        for (int i = 0; i < pieceNumbers.GetLength(0); i++) {
            for (int j = 0; j < pieceNumbers.GetLength(1); j++) {
                // Create quads for non-zero numbers
                if (pieceNumbers[i,j] >= 1 && pieceNumbers[i,j] <= 9) {
                    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    quad.transform.parent = numberContainer.transform;

                    quad.transform.localPosition = new Vector3(j, -i, 0);
                    // Set the texture to the corresponding number
                    quad.renderer.material = tileSprites[pieceNumbers[i,j]-1];
                    quad.renderer.material.color = color;

                    quad.AddComponent<ForceTileUpright>();
                }
            }
        }
    }
    
    void Update() {
        
    }

    void rotateClockwise() {

    }

    void rotateCounterClockwise() {

    }

    public Piece getPiece() {
        return piece;
    }
}
