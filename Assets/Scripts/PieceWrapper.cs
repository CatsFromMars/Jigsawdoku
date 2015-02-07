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
    public GameObject board;
    public BoardWrapper boardWrapper;

    private Piece piece;
    private GameObject numberContainer;
    private MeshCollider collider;
    private Quaternion targetRotation;

    private int pieceRow;
    private int pieceCol;

	void Awake() {
		board = GameObject.FindGameObjectWithTag("Board");
		boardWrapper = board.GetComponent<BoardWrapper>();
	}

    void Start() {
        piece = Piece.fromSerializable2DIntArray(serializablePieceNumbers);

        // Adding tiles in a wrapper object
        numberContainer = new GameObject("numbers");
        numberContainer.transform.parent = transform;
        numberContainer.transform.localPosition = new Vector3(0, 0, 0);

        int[,] pieceNumbers = piece.to2DArray();

        Vector3 centerOffset = new Vector3((pieceNumbers.GetLength(1)-1)/2.0f, -(pieceNumbers.GetLength(0)-1)/2.0f, 0);

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

        targetRotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(rotatePiece());

        float x = transform.position.x;
        float y = transform.position.y;
        
        float newX = Mathf.Clamp(x, -13, 13);
        float newY = Mathf.Clamp(y, -10, 5);
        
        transform.position = new Vector3(newX, newY, 0);
    }
    
    void OnMouseDrag() {
        boardWrapper.selectedPiece = this.piece;
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);

        float newX = Mathf.Clamp(curPosition.x, -13, 13);
        float newY = Mathf.Clamp(curPosition.y, -10, 5);
        
        transform.position = new Vector3(newX, newY, -1);
        handleInput();

        GameObject background = GameObject.FindGameObjectWithTag("BackgroundEffect");
        BackgroundController backgroundController = background.GetComponent<BackgroundController>();

        backgroundController.setColor(ColorUtils.lightenColor(backColor, 0.2f));
    }

    void OnMouseUp() {
        snapPieces();
        
        GameObject background = GameObject.FindGameObjectWithTag("BackgroundEffect");
        BackgroundController backgroundController = background.GetComponent<BackgroundController>();

        backgroundController.setColor(Color.white);
    }

    void OnMouseEnter() {
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        transform.localScale = new Vector3(1.05f, 1.05f, 1);
    }

    void OnMouseExit() {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        transform.localScale = Vector3.one;
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
            
            Vector3 newPosition = new Vector3(x, y, z);

            transform.position = newPosition;
        }
    }
    
    void rotateClockwise() {
        targetRotation *= Quaternion.Euler(0, 0, -90);
		piece.rotateClockwise();
    }
    
    void rotateCounterClockwise() {
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
}
