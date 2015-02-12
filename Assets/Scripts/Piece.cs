﻿using UnityEngine;
using System.Collections;

public class AABB {

    public int minX, maxX, minY, maxY;

    public AABB(int minX, int maxX, int minY, int maxY) {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
    }
}

public class Piece {

    private int[,] numbers;
    private int width;
    private int height;
    private int numTiles;
    private Vector2 correctPosition;

    public Piece(int[,] nums) {
        this.numbers = trimPaddingZeros(nums);
        this.width = this.numbers.GetLength(1);
        this.height = this.numbers.GetLength(0);

        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                if (numbers[i,j] != 0) {
                    numTiles++;
                }
            }
        }
    }

    public void rotateClockwise() {
        int[,] newNumbers = new int[width, height];
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                newNumbers[j,i] = numbers[height-i-1, j];
            }
        }
        numbers = newNumbers;

        int swap = width;
        width = height;
        height = swap;
    }

    public void rotateCounterClockwise() {
        int[,] newNumbers = new int[width, height];
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                newNumbers[j,i] = numbers[i, width-j-1];
            }
        }
        numbers = newNumbers;
        
        int swap = width;
        width = height;
        height = swap;
    }

    public int[,] to2DArray() {
        return numbers;
    }

    public int getWidth() {
        return width;
    }

    public int getHeight() {
        return height;
    }

    public int getNumTiles() {
        return numTiles;
    }

    private int[,] trimPaddingZeros(int[,] a) {
        AABB aabb = getAABB(a);

        int[,] trimmed = new int[aabb.maxY - aabb.minY + 1, aabb.maxX - aabb.minX + 1];

        for (int i = 0; i < trimmed.GetLength(0); i++) {
            for (int j = 0; j < trimmed.GetLength(1); j++) {
                trimmed[i,j] = a[i + aabb.minY, j + aabb.minX];
            }
        }

        correctPosition = new Vector2(aabb.minX, aabb.minY);

        return trimmed;
    }

    private AABB getAABB(int[,] a) {
        int minX = 0, minY = 0, maxX = 0, maxY = 0;
        
        int width = a.GetLength(1);
        int height = a.GetLength(0);

        // Get minX (inclusive)
        bool flag = true;
        for (int i = 0; i < width && flag; i++) {
            for (int j = 0; j < height; j++) {
                if (a[j,i] != 0) {
                    flag = false;
                    break;
                }
            }
            
            if (!flag) {
                minX = i;
            }
        }
        
        // Get minY (inclusive)
        flag = true;
        for (int i = 0; i < height && flag; i++) {
            for (int j = 0; j < width; j++) {
                if (a[i,j] != 0) {
                    flag = false;
                    break;
                }
            }
            
            if (!flag) {
                minY = i;
            }
        }
        
        // Get maxX (inclusive)
        flag = true;
        for (int i = width - 1; i >= minX && flag; i--) {
            for (int j = minY; j < height; j++) {
                if (a[j,i] != 0) {
                    flag = false;
                    break;
                }
            }
            
            if (!flag) {
                maxX = i;
            }
        }
        
        // Get maxY (inclusive)
        flag = true;
        for (int i = height - 1; i >= minY && flag; i--) {
            for (int j = minX; j < width; j++) {
                if (a[i,j] != 0) {
                    flag = false;
                    break;
                }
            }
            
            if (!flag) {
                maxY = i;
            }
        }

        return new AABB(minX, maxX, minY, maxY);
    }

    public Vector2 getCorrectPosition() {
        return correctPosition;
    }

    // Not used anymore, but may be useful for debugging
    override
    public string ToString() {
        string str = "";

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (numbers[i,j] != 0) {
                    str += numbers[i,j] + " "; // 1 space
                }
                else {
                    str += "  "; // 2 spaces
                }
            }
            str += "\n";
        }

        return str;
    }

    public static Piece fromSerializable2DIntArray(Serializable2DIntArray[] arrays) {
        int width = arrays.Length;
        int height = arrays[0].array.Length;
        int[,] nums = new int[width, height];
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                nums[i,j] = arrays[i].array[j];
            }
        }

        return new Piece(nums);
    }
}
