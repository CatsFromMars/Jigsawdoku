using UnityEngine;
using System.Collections;


[System.Serializable]
public class ColorPair {
    public Color tileColor;
    public Color numberColor;
}

public class VoronoiGenerator : MonoBehaviour {

    public GameObject piecePrefab;
    public PuzzleDatabase database;

    public int numberOfPieces;
    public int numberOfHints;
    public int minPieceSize;
    
    public ColorPair[] colorTable;

    private int[,] solvedNumbers;
    private int[,] voronoiMask;

	void Start () {
        solvedNumbers = new int[9,9];
        voronoiMask = new int[9,9];

        int random = UnityEngine.Random.Range(0, database.puzzle.Length);

        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                solvedNumbers[i,j] = (int)char.GetNumericValue(database.puzzle[random][i*9+j]);
            }
        }

        generateVoronoiPieces(numberOfPieces, numberOfHints, minPieceSize);
	}

    private void generateVoronoiPieces(int numPoints, int numHints, int minSize) {
        if (numPoints <= 0 || numPoints > 81) {
            Debug.LogError("Invalid number of points");
        }

        int[,] voronoiPoints = new int[9,9];
        int[,] pointLocations = new int[numPoints,2];

        int pointCounter = 0;
        while (pointCounter < numPoints) {
            int r = UnityEngine.Random.Range(0, 9);
            int c = UnityEngine.Random.Range(0, 9);

            if (voronoiPoints[r,c] == 0) {
                voronoiPoints[r,c] = 1;

                pointLocations[pointCounter,0] = r;
                pointLocations[pointCounter,1] = c;

                pointCounter++;
            }
        }

        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                voronoiMask[i,j] = indexOfClosestVoronoiPoint(i, j, pointLocations);
            }
        }

        for (int i = 0; i < numPoints; i++) {
            int[,] untrimmedPiece = new int[9,9];

            for (int j = 0; j < 9; j++) {
                for (int k = 0; k < 9; k++) {
                    if (voronoiMask[j,k] == i) {
                        untrimmedPiece[j,k] = solvedNumbers[j,k];
                    }
                }
            }

            Piece p = new Piece(untrimmedPiece);

            Color numberColor = Color.black;
            Color tileColor = Color.black;

            int x = UnityEngine.Random.Range(7, 12);
            int y = UnityEngine.Random.Range(5, 8);
            if (UnityEngine.Random.value < 0.5) {
                x *= -1;
            }
            if (UnityEngine.Random.value < 0.5) {
                y *= -1;
            }
            
            GameObject t = (GameObject)GameObject.Instantiate(piecePrefab, new Vector3(x, y, 0), Quaternion.identity);
            PieceWrapper pieceWrapper = t.GetComponent<PieceWrapper>();

            if (i < numHints || p.getNumTiles() < minSize) {
                pieceWrapper.makeHint();
            }
            else {
                if (colorTable.Length > 0) {
                    ColorPair colorPair = colorTable[i % colorTable.Length];
                    numberColor = colorPair.numberColor;
                    tileColor = colorPair.tileColor;
                }

                for (int j = 0; j < 3; j++) {
                    if (UnityEngine.Random.value < 0.5) {
                        p.rotateClockwise();
                    }
                }
            }

            pieceWrapper.SetData(p, numberColor, tileColor);
        }
    }

    private int indexOfClosestVoronoiPoint(int row, int col, int[,] pointLocations) {
        int minIndex = 0;
        int dx = col - pointLocations[0,1];
        int dy = row - pointLocations[0,0];
        int distSq = dx * dx + dy * dy;
        int minDist = distSq;

        for (int i = 1; i < pointLocations.GetLength(0); i++) {
            dx = col - pointLocations[i,1];
            dy = row - pointLocations[i,0];
            distSq = dx * dx + dy * dy;

            if (distSq < minDist) {
                minDist = distSq;
                minIndex = i;
            }
        }

        return minIndex;
    }
}