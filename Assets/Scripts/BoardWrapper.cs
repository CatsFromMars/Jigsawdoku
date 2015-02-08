using UnityEngine;
using System.Collections;

public class BoardWrapper : MonoBehaviour {

    public Color boardColor;
    public Material boardSprite;
    public Material overlaySprite;

    private Board board;
	public Piece selectedPiece;
    private GameObject quad;
    private GameObject overlayQuad;

	void Start () {
        board = new Board();

        quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        overlayQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);

        quad.transform.parent = transform;
        overlayQuad.transform.parent = transform;

        quad.transform.localPosition = new Vector3(0, 0, 1);
        overlayQuad.transform.localPosition = new Vector3(0, 0, -5);

        quad.transform.localScale = new Vector3(10, 10, 1); // The board sprite is exactly 10 times as large as the tile sprites
        overlayQuad.transform.localScale = new Vector3(10, 10, 1);

        quad.renderer.material = boardSprite;
        quad.renderer.material.color = boardColor;

        overlayQuad.renderer.material = overlaySprite;
        overlayQuad.renderer.material.color = Color.black * 0.5f;
        
        Destroy(quad.GetComponent<MeshCollider>());
        Destroy(overlayQuad.GetComponent<MeshCollider>());

        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        board.setPieces(pieces);
	}

    public Board getBoard() {
        return board;
    }

    public void showOverlay() {
        overlayQuad.renderer.material.color = Color.black * 0.5f;
    }

    public void hideOverlay() {
        overlayQuad.renderer.material.color = Color.clear;
    }
}
