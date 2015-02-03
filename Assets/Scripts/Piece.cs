﻿using UnityEngine;
using System.Collections;

public class Piece {

    private int[,] numbers;
    private int width;
    private int height;

    private Piece(int[,] nums) {
        this.numbers = nums;
        this.width = nums.GetLength(0);
        this.height = nums.GetLength(1);
    }

    public void rotateClockwise() {
        int[,] newNumbers = new int[height,width];
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                newNumbers[j,i] = numbers[width-i-1, j];
            }
        }
    }

    public void rotateCounterClockwise() {
        int[,] newNumbers = new int[height,width];
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                newNumbers[j,i] = numbers[i, height-j-1];
            }
        }
    }

    public int[,] to2DArray() {
        return numbers;
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
