using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PiecePosition {

    public Piece piece;

    // Location of the upper left corner of the piece
    public int row;
    public int col;

    public PiecePosition(Piece p, int r, int c) {
        piece = p;
        row = r;
        col = c;
    }
}

public class Board {

    private int[,] boardNumbers;
    private List<PiecePosition> piecesOnBoard;

    public Board() {
        piecesOnBoard = new List<PiecePosition>();
        boardNumbers = new int[9, 9];
    }

    public bool attemptAddPiece(Piece piece, int row, int col) {
        int[,] pieceNumbers = piece.to2DArray();

        for (int i = 0; i < pieceNumbers.GetLength(0); i++) {
            for (int j = 0; j < pieceNumbers.GetLength(1); j++) {
                if (boardNumbers[row + i, j + col] != 0 && pieceNumbers[i,j] != 0) {
                    return false;
                }
            }
        }

        for (int i = 0; i < pieceNumbers.GetLength(0); i++) {
            for (int j = 0; j < pieceNumbers.GetLength(1); j++) {
                if (pieceNumbers[i,j] != 0) {
                    boardNumbers[row + i, j + col] = pieceNumbers[i,j];
                }
            }
        }

        return true;
    }

    public Piece attemptRemovePiece(int row, int col) {
        if (boardNumbers[row, col] == 0) {
            return null;
        }

        foreach (PiecePosition p in piecesOnBoard) {
            int[,] pieceNumbers = p.piece.to2DArray();
            int pieceRow = p.row;
            int pieceCol = p.col;
            
            
            for (int i = 0; i < pieceNumbers.GetLength(0); i++) {
                for (int j = 0; j < pieceNumbers.GetLength(1); j++) {
                    if (pieceNumbers[i,j] != 0 && pieceRow + i == row  && pieceCol + j == col) {
                        removePieceFromBoard(pieceNumbers, pieceRow, pieceCol);
                        return p.piece;
                    }
                }
            }
        }

        return null;
    }

    public bool isComplete() {
        int completeMask =  0x000003FE; // bit 0 is 0, bit 1-9 are all 1 (From LSB)

        for (int i = 0; i < 9; i++) {
            int checkRows = 0;
            int checkCols = 0;
            int checkBlks = 0;

            for (int j = 0; j < 9; j++) {
                checkRows |= 1 << boardNumbers[j,i];
                checkCols |= 1 << boardNumbers[i,j];
            }

            if (checkRows != completeMask) {
                return false;
            }
            if (checkCols != completeMask) {
                return false;
            }

            int blockRow = i/3;
            int blockCol = i%3;
            for (int j = 0; j < 3; j++) {
                for (int k = 0; k < 3; k++) {
                    checkBlks |= 1 << boardNumbers[3*blockRow + j, 3*blockCol + k];
                }
            }

            if (checkBlks != completeMask) {
                return false;
            }
        }

        return true;
    }

    private void removePieceFromBoard(int[,] pieceNumbers, int pieceRow, int pieceCol) {
        for (int i = 0; i < pieceNumbers.GetLength(0); i++) {
            for (int j = 0; j < pieceNumbers.GetLength(1); j++) {
                if (pieceNumbers[i,j] != 0) {
                    boardNumbers[pieceRow + i, pieceCol + j] = 0;
                }
            }
        }
    }


    // Not used anymore, but may be useful for debugging
    override
    public string ToString() {
        string str = "";

        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                if (boardNumbers[i,j] != 0) {
                    str += boardNumbers[i,j] + " "; // 1 space
                }
                else {
                    str += "  "; // 2 spaces
                }
            }
            str += "\n";
        }

        return str;
    }
 
}
