﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board {
    private int[,] boardNumbers;
    private GameObject[] pieceContainers;

    public Board() {
        boardNumbers = new int[9, 9];
    }
    
    public bool isComplete() {
        updateBoardInts();
        
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
    
    public bool[,] getConflicts() {
        bool[,] conflicts = new bool[9, 9];
        
        resetBoard();
        addSnappedPiecesToBoard();

        for (int i = 0; i < 9; i++) {
            int rowConflicts = 0;
            int colConflicts = 0;
            int blkConflicts = 0;

            for (int j = 0; j < 9; j++) {
                int oldRowConflicts = rowConflicts;
                int oldColConflicts = colConflicts;

                if (boardNumbers[i,j] != 0) {
                    rowConflicts |= 1 << boardNumbers[i,j];
                    if (oldRowConflicts == rowConflicts) {
                        conflicts[i,j] = true;
                    }
                }

                if (boardNumbers[j,i] != 0) {
                    colConflicts |= 1 << boardNumbers[j,i];
                    if (oldColConflicts == colConflicts) {
                        conflicts[j,i] = true;
                    }
                }
            }

            rowConflicts = 0;
            colConflicts = 0;
            for (int j = 8; j >= 0; j--) {
                int oldRowConflicts = rowConflicts;
                int oldColConflicts = colConflicts;
                
                if (boardNumbers[i,j] != 0) {
                    rowConflicts |= 1 << boardNumbers[i,j];
                    if (oldRowConflicts == rowConflicts) {
                        conflicts[i,j] = true;
                    }
                }
                
                if (boardNumbers[j,i] != 0) {
                    colConflicts |= 1 << boardNumbers[j,i];
                    if (oldColConflicts == colConflicts) {
                        conflicts[j,i] = true;
                    }
                }
            }
            
            int blockRow = i/3;
            int blockCol = i%3;

            for (int j = 0; j < 3; j++) {
                for (int k = 0; k < 3; k++) {
                    int oldBlkConflicts = blkConflicts;

                    if (boardNumbers[3*blockRow + j, 3*blockCol + k] != 0) {
                        blkConflicts |= 1 << boardNumbers[3*blockRow + j, 3*blockCol + k];
                        if (oldBlkConflicts == blkConflicts) {
                            conflicts[3*blockRow + j, 3*blockCol + k] = true;
                        }
                    }
                }
            }

            blkConflicts = 0;
            for (int j = 2; j >= 0; j--) {
                for (int k = 2; k >= 0; k--) {
                    int oldBlkConflicts = blkConflicts;
                    
                    if (boardNumbers[3*blockRow + j, 3*blockCol + k] != 0) {
                        blkConflicts |= 1 << boardNumbers[3*blockRow + j, 3*blockCol + k];
                        if (oldBlkConflicts == blkConflicts) {
                            conflicts[3*blockRow + j, 3*blockCol + k] = true;
                        }
                    }
                }
            }
        }
        
        string debug = "";
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                if(conflicts[i,j]) {
                    debug += "1";
                }
                else {
                    debug += "_";
                }
            }
            debug += "\n";
        }
        Debug.Log(debug);

        return conflicts;
    }

    private void updateBoardInts() {
        resetBoard();

        addSnappedPiecesToBoard();

        Debug.Log(ToString());
    }

    public bool canPlacePiece(Piece piece, Vector3 position) {
        resetBoard();

        addSnappedPiecesToBoard();

        int[,] pieceNumbers = piece.to2DArray();
        float xOffset = (piece.getWidth()-1)/2.0f;
        float yOffset = (piece.getHeight()-1)/2.0f;
        
        int pieceRow = 4 - (int)(position.y + yOffset);
        int pieceCol = 4 + (int)(position.x - xOffset);

        
        for (int i = 0; i < piece.getHeight(); i++) {
            for (int j = 0; j < piece.getWidth(); j++) {
                try {
                    if (pieceNumbers[i,j] != 0) {
                        if (boardNumbers[pieceRow + i, pieceCol + j] != 0) {
                            return false;
                        }
                    }
                }
                catch {
                    return false;
                }
            }
        }

        return true;
    }
    
    private void resetBoard() {
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                boardNumbers[i,j] = 0;
            }
        }
    }

    private void addSnappedPiecesToBoard() {
        pieceContainers = GameObject.FindGameObjectsWithTag("Piece");

        foreach (GameObject obj in pieceContainers) {
            PieceWrapper pieceWrapper = obj.GetComponent<PieceWrapper>();

            if (pieceWrapper.isSnapped()) {
                Piece piece = pieceWrapper.getPiece();
                int[,] pieceNumbers = piece.to2DArray();
                
                float xOffset = (piece.getWidth()-1)/2.0f;
                float yOffset = (piece.getHeight()-1)/2.0f;

                int pieceRow = 4 - (int)(obj.transform.position.y + yOffset);
                int pieceCol = 4 + (int)(obj.transform.position.x - xOffset);
                
                for (int i = 0; i < piece.getHeight(); i++) {
                    for (int j = 0; j < piece.getWidth(); j++) {
                        try {
                            if (pieceNumbers[i,j] != 0) {
                                boardNumbers[pieceRow + i, pieceCol + j] = pieceNumbers[i,j];
                            }
                        }
                        catch {
                            // Do nothing
                        }
                    }
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
                    str += boardNumbers[i,j] + " ";
                }
                else {
                    str += "_ ";
                }
            }
            str += "\n";
        }

        return str;
    }
 
}
