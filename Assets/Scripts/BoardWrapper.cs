using UnityEngine;
using System.Collections;

public class BoardWrapper : MonoBehaviour {

    public Color boardColor;
    public Material boardSprite;
    public Material overlaySprite;

    private Board board;
    private GameObject quad;
    private GameObject overlayQuad;

    private GameObject errorPrefab;

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
        
        Destroy(quad.GetComponent<MeshCollider>());
        Destroy(overlayQuad.GetComponent<MeshCollider>());

        errorPrefab = (GameObject) Resources.Load("ErrorPrefab");

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

    public void updateErrors() {
        destroyErrors();

        bool[,] conflicts = board.getConflicts();

        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                if (conflicts[i,j]) {
                    GameObject.Instantiate(errorPrefab, new Vector3(j-4, 4-i, 0.9f), Quaternion.identity);
                }
            }
        }
    }

    public void destroyErrors() {
        foreach (GameObject error in GameObject.FindGameObjectsWithTag("Error")) {
            Destroy(error);
        }
    }

    public Board getBoard() {
        return board;
    }
}
