using UnityEngine;
using System.Collections;

public class PuzzleDatabase : MonoBehaviour {
	
	public string[] puzzle;								// prefab puzzles
	
	public string SelectPuzzle(){
		string code = "";										// puzzle code
		if(puzzle.Length > 0){
			int index = Random.Range(0,puzzle.Length);					// random puzzle number
			code = puzzle[index];																	// select puzzle
		}
		return code;																						// return puzzle
	}
}
