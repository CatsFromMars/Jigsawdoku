using UnityEngine;
using System.Collections;

public class BoardWrapper : MonoBehaviour {

    public Color boardColor;
    public Material boardSprite;
    public Color outlineColor;
    public Material outlineSprite;
	public Piece selectedPiece;

    private Board board;

	void Start () {
        board = new Board();

        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

        quad.transform.parent = transform;
        quad.transform.localPosition = new Vector3(0, 0, 1);
        quad.transform.localScale = new Vector3(10, 10, 1); // The board sprite is exactly 10 times as large as the tile sprites

        quad.renderer.material = boardSprite;
        quad.renderer.material.color = boardColor;
		quad.collider.isTrigger = true;
		quad.layer = 2;

        // The outline makes the board stand out a bit more from the background
        GameObject outlineQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);

        outlineQuad.transform.parent = transform;
        outlineQuad.transform.localPosition = new Vector3(0, 0, 1.1f);
        outlineQuad.transform.localScale = new Vector3(10.1f, 10.1f, 1);
        outlineQuad.renderer.material = outlineSprite;
        outlineQuad.renderer.material.color = outlineColor;

        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        board.setPieces(pieces);
	}

    public Board getBoard() {
        return board;
    }
}
