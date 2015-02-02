using UnityEngine;
using System.Collections;

[System.Serializable]
public class Serializable2DIntArray {
    public int[] array;
}

public class PieceWrapper : MonoBehaviour {
    
    public Font font;
    public Color color;

    public Serializable2DIntArray[] serializablePieceNumbers;

    private Piece piece;
    private TextMesh text;
    private GameObject numberContainer;
    private GameObject backContainer;

    void Start() {
        piece = Piece.fromSerializable2DIntArray(serializablePieceNumbers);

        // Adding Text in a wrapper object
        numberContainer = new GameObject("numbers");
        numberContainer.transform.parent = transform;

        text = numberContainer.AddComponent<TextMesh>();
        text.font = font;

        color.a = 1;
        text.color = color;

        text.alignment = TextAlignment.Left;
        text.text = piece.ToString();
        
        // Prevents blurry text
        text.fontSize = 100;
        numberContainer.transform.localScale = new Vector3(0.1f, 0.1f, 1); // Total font size is 100 * 0.1 = 10
        numberContainer.transform.localPosition = new Vector3(0, 0, -1); // Bring text to front
        numberContainer.renderer.material = font.material;

        // Adding background to a wrapper object
        backContainer = new GameObject("back");
        backContainer.transform.parent = transform;

        int[,] pieceNumbers = piece.to2DArray();
        for (int i = 0; i < pieceNumbers.GetLength(0); i++) {
            for (int j = 0; j < pieceNumbers.GetLength(1); j++) {
                // Create quads under non-zero numbers
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
