using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

class Square
{
		public int weight = 0;
		public int number = 0;
		public int max = 9;
		public int row = 0;
		public int col = 0;
};

//[System.Serializable]
//public class ColorPair {
//    public Color tileColor;
//    public Color numberColor;
//}

public class RandomPieceGenerator : MonoBehaviour
{ 

		public GameObject piecePrefab;
		public PuzzleDatabase database;
		public ColorPair[] colorTable;
		Square[,] board = new Square[9, 9];
		int maxPieceSize = 9;
		int minPieceSize = 6;
		List<int[,]> jigsawPieces = new List<int[,]> ();
		ArrayList jigsawPiecesPlaceholder = new ArrayList ();

		void Start ()
		{
				// get a random board from database;
				int random = 0;
				for (int i = 0; i < 9; i++) {
						for (int j = 0; j < 9; j++) {
								board [i, j] = new Square ();
								board [i, j].weight = 0;
								board [i, j].number = (int)Char.GetNumericValue (database.puzzle [random] [i * 9 + j]);
								board [i, j].row = i;
								board [i, j].col = j;
						}
				}

				GenerateRandomPieces ();
				MakePieces (jigsawPieces);

		}

		void GenerateRandomPieces ()
		{
				// pick x arbitrary starting points
				int[] starting_points = new int[9];
				for (int i = 0; i < 9; i++) {
						int random = UnityEngine.Random.Range (0, 9);
						starting_points [i] = i * 9 + random;
				}
                
				int counter = 0;
				// assign each starting point a weight of 1
				foreach (int x in starting_points) {
						int row = x / 9;
						int col = x % 9;
						board [row, col].weight = 1; 
						board [row, col].max = UnityEngine.Random.Range (minPieceSize, maxPieceSize);
                        
				}

				// now loop through each starting point again and generate piece for each

				foreach (int x in starting_points) {
						int row = x / 9;
						int col = x % 9;
						counter++; // for starting point 
						// also increment it for each of the squares in the piece
						counter += GeneratePiece (row, col, board [row, col].max);
				}

				// now if counter < 81, we loop through all squares again and run generatepiece
				
				if (counter < 81) { //I MADE A CHANGE HERE! BEWARB!
					
						for (int row = 0; row < 9; row++) {
								for (int col = 0; col <9; col++) {
										if (board [row, col].weight == 0) {
												board [row, col].weight = 1;
												int max = UnityEngine.Random.Range (minPieceSize, maxPieceSize);
												board [row, col].max = max;
												counter ++;
												counter += GeneratePiece (row, col, max);
										}
								}
						}

						// now, every square is part of a piece between size 1 and maxPieceSize
				}
		
				Debug.Log ("Final Count: " + counter);


		}

		int GeneratePiece (int row, int col, int max)
		{
				// keep a track of piece, which is just an array of squares

				List<Square> piece = new List<Square> ();
				int[,] jaggedArray = new int[9, 9];
				for (int i = 0; i < 9; i++) {
						for (int j = 0; j < 9; j++) {
								jaggedArray [i, j] = 0;
						}
				}
				jaggedArray [row, col] = board [row, col].number;
				piece.Add (board [row, col]);
				int found = 1;
				Square adjacent = FindRandomAdjacentPiece (row, col);
				while (adjacent != null && found < max) {
						// add adjacent to array;
						piece.Add (adjacent);
						jaggedArray [adjacent.row, adjacent.col] = adjacent.number;
						//piece [found] = adjacent;
						found ++;
						// find next adjacent piece, if piece still needs to be built
						if (found < max) {  
								adjacent = FindRandomAdjacentPiece (adjacent.row, adjacent.col);
						}
				}
                

				// we now have a piece of some length < max
				// change its format to [][,] of ints
				// for each int


                    
				jigsawPieces.Add (jaggedArray);
                

				// now make a new gameobject for this piece. 
				//PrintPiece (piece);
				return found - 1;


		}

		void PrintPiece (List<Square> piece)
		{
				Debug.Log ("A Jigsaw Piece: ");
				foreach (Square cell in piece) {
						Debug.Log ("Row: " + cell.row + ", Col: " + cell.col + "|");
                
				}
		}
		

		/**
	 * should return a valid adjacent piece to connect with. Must be in grid, and have a 
	 * weight of 0 - meaning its not in any other piece yet. 
	 **/
		Square FindRandomAdjacentPiece (int row, int col)
		{
				// valid piece is next to this piece, and has weight 0
				Square[] adjacent = new Square[4];
				for (int i = 0; i < adjacent.Length; ++i) {
						adjacent [i] = new Square ();
				}
				// look at each possible adjacent square
				// top

				int count = 0;
				if (row == 0) {
						adjacent [0] = null;
				} else if (board [row - 1, col].weight == 0) {
						adjacent [0] = board [row - 1, col];
						count ++;
				} else {
						adjacent [0] = null;
				}

				// bottom

				if (row == 8) {
						adjacent [1] = null;
				} else if (board [row + 1, col].weight == 0) {
						adjacent [1] = board [row + 1, col];
						count ++;
				} else {
						adjacent [1] = null;
				}

				// left

				if (col == 0) {
						adjacent [2] = null;
				} else if (board [row, col - 1].weight == 0) {
						adjacent [2] = board [row, col - 1];
						count ++;
				} else {
						adjacent [2] = null;
				}

				// right 

				if (col == 8) {
						adjacent [3] = null;
				} else if (board [row, col + 1].weight == 0) {
						adjacent [3] = board [row, col + 1];
						count ++;
				} else {
						adjacent [3] = null;
				}

				// pick an adjacent square

				if (count == 0) 
						return null;
				else {
						int choose = UnityEngine.Random.Range (1, count + 1);
						// go through adjacent array and find the look at the square in choose index, while only counting non-null values
						for (int i = 0; i<4; i++) {
								if (adjacent [i] == null) {
										choose++;
								} else {
										if (i == choose - 1) {
												// increment weight of chosen square
												Square chosen = new Square ();
												chosen = adjacent [choose - 1];
												board [chosen.row, chosen.col].weight = 1;
                                
												return chosen;
										}
								}
						}
						// should never get here according to my algorithm
						return null;
				}
		}

		public void MakePieces (List<int[,]> piecesList)
		{           
				foreach (int[,] piece in piecesList) {
						Piece p = new Piece (piece); 
						jigsawPiecesPlaceholder.Add (p);
				}
				int counter = 0;
				foreach (Piece p in jigsawPiecesPlaceholder) {
						// Do not use random colors; they look bad. Instead, pull predetermined color pairs from an array.

						Color numberColor = Color.black; // Default colors
						Color tileColor = Color.black;

						if (colorTable.Length > 0) {
								ColorPair colorPair = colorTable [counter % colorTable.Length];
								numberColor = colorPair.numberColor;
								tileColor = colorPair.tileColor;
						}

						int x = UnityEngine.Random.Range (7, 12);
						int y = UnityEngine.Random.Range (5, 8);
						if (UnityEngine.Random.value < 0.5) {
								x *= -1;
						}
						if (UnityEngine.Random.value < 0.5) {
								y *= -1;
						}

						GameObject t = (GameObject)GameObject.Instantiate (piecePrefab);//, new Vector3(x, y, 0), Quaternion.identity);
						PieceWrapper pieceWrapper = t.GetComponent<PieceWrapper> ();

						pieceWrapper.SetData (p, numberColor, tileColor);
						pieceWrapper.makeHint ();

						//No need to call awake/start (They are automatically called after this.)
						counter++;
				}
		}
}
