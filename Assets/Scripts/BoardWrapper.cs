using UnityEngine;
using System.Collections;

public class BoardWrapper : MonoBehaviour {

    public Color boardColor;
    public Material boardSprite;
    public Material overlaySprite;

    private Board board;
    private GameObject quad;
    private GameObject overlayQuad;

    private int colorIndex;
    private Color[] colorCycle = {
        0.7f * Color.red,
        0.7f * Color.yellow,
        0.7f * Color.green,
        0.7f * Color.cyan,
        0.7f * Color.blue,
        0.7f * Color.magenta
    };

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
        quad.renderer.material.color = Color.black;

        overlayQuad.renderer.material = overlaySprite;
        //overlayQuad.renderer.material.color = Color.black * 0.8f;
        
        Destroy(quad.GetComponent<MeshCollider>());
        Destroy(overlayQuad.GetComponent<MeshCollider>());
        /*
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        board.setPieces(pieces);
        */
        StartCoroutine(cycleColors());
	}

    IEnumerator cycleColors() {
        while (true) {
            colorIndex = (colorIndex + 1) % colorCycle.Length;

            StartCoroutine(updateOverlayColor());

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator updateOverlayColor() {
        float i = 0;
        while (i < 1) {
            i += Time.deltaTime;
            overlayQuad.renderer.material.color = Color.Lerp(colorCycle[colorIndex], colorCycle[(colorIndex + 1) % colorCycle.Length], i);

            yield return null;
        }
    }

    public Board getBoard() {
        return board;
    }

    public void showOverlay() {
        //overlayQuad.renderer.material.color = Color.black * 0.8f;
    }

    public void hideOverlay() {
        overlayQuad.renderer.material.color = Color.clear;
    }
}
