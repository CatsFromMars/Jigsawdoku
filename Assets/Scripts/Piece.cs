using UnityEngine;
using System.Collections;

public class Piece {

    private int[,] numbers;
    private int width;
    private int height;

    private Piece(int[,] nums) {
        this.numbers = nums;
        this.width = nums.GetLength(1);
        this.height = nums.GetLength(0);
    }

    public void rotateClockwise() {
        int[,] newNumbers = new int[width, height];
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                Debug.Log("clockwise " + width + " " + height + " " + (height - i - 1));
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
                Debug.Log("counter clockwise " + width + " " + height + " " + (width - j - 1));
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
