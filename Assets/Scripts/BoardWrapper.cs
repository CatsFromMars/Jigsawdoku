using UnityEngine;
using System.Collections;

public class BoardWrapper : MonoBehaviour {

    public Color color;
    public Material boardSprite;

    private Board board;

	// Use this for initialization
	void Start () {
        board = new Board();

        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

        quad.transform.parent = transform;
        quad.transform.localPosition = new Vector3(0, 0, 1);
        quad.transform.localScale = new Vector3(10, 10, 1); // The board sprite is exactly 10 times as large as the tile sprites

        quad.renderer.material = boardSprite;
        quad.renderer.material.color = color;
	}
	
	void Update () {
	
	}
}
