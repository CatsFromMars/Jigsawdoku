using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Serializable2DIntArray
{
		public int[] array;
}

public class PieceWrapper : MonoBehaviour
{

		public Color color;
		public Material[] tileSprites;
		public Serializable2DIntArray[] serializablePieceNumbers;
		private Piece piece;
		private GameObject numberContainer;
		public GameObject centerCollider;
		public GameObject edgesCollider;
		bool move = false;
	public int pieceSize = 9;
	GameObject[] numbers;


		void Start ()
		{
				piece = Piece.fromSerializable2DIntArray (serializablePieceNumbers);

				// Adding tiles in a wrapper object
				numberContainer = new GameObject ("numbers");
				numberContainer.transform.parent = transform;
		numbers = new GameObject[pieceSize];

				int[,] pieceNumbers = piece.to2DArray ();
				int count = 0;
				for (int i = 0; i < pieceNumbers.GetLength(0); i++) {
						for (int j = 0; j < pieceNumbers.GetLength(1); j++) {
								// Create quads for non-zero numbers
								if (pieceNumbers [i, j] >= 1 && pieceNumbers [i, j] <= 9) {
										GameObject quad = GameObject.CreatePrimitive (PrimitiveType.Quad);
										quad.transform.parent = numberContainer.transform;

										quad.transform.localPosition = new Vector3 (j, -i, 0);
										// Set the texture to the corresponding number
										quad.renderer.material = tileSprites [pieceNumbers [i, j] - 1];
										quad.renderer.material.color = color;
										

										quad.AddComponent<ForceTileUpright> ();
										quad.collider.isTrigger = true;
										quad.layer = 2;
										numbers [count] = quad;
										count ++;
								}
						}
				}
				

				// instantiates the center and edges collider with same transform coordiantes as this parent piece
				
				GameObject temp;
				temp = GameObject.Instantiate (centerCollider, transform.position, Quaternion.identity) as GameObject; 
				temp.transform.position = new Vector3 (temp.transform.position.x, temp.transform.position.y, -2.0f);
				temp = GameObject.Instantiate (edgesCollider, transform.position, Quaternion.identity) as GameObject;
				temp.transform.position = new Vector3 (temp.transform.position.x, temp.transform.position.y, 0.0f);
				move = false;
		}
    
		void Update ()
		{
				rotateClockwise ();
		}
	
		void rotateClockwise ()
		{
				if (Input.GetMouseButtonDown (0)) {
						RaycastHit hit; 
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						if (Physics.Raycast (ray, out hit, 20.0f)) {

								if (hit.collider.CompareTag ("Edges") && hit.collider.transform.position.x == transform.position.x &&
										hit.collider.transform.position.y == transform.position.y) {
										// rotate piece clockwise
										Debug.Log ("Rotate");
										transform.Rotate (new Vector3 (0.0f, 0.0f, 90.0f));
										foreach (GameObject number in numbers) {
												number.transform.Rotate (new Vector3 (0.0f, 0.0f, -90.0f));
										}
								}
						}  
				} 
		}
	
		void rotateCounterClockwise ()
		{

		}
	
		void clickAndDrag ()
		{
				
		}

		public Piece getPiece ()
		{
				return piece;
		}
}
