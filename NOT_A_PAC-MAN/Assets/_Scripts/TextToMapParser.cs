using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.IO;

public class TextToMapParser : MonoBehaviour
{
    public int[][] arrays;
    public TextAsset text;
    private void Awake()
    {
        arrays = LoadArraysFromFile(text, out int y, out int x);
        // Print the loaded array of arrays for verification
        for (int i = 0; i < y; i++)
        {
            string arrayStr = "";
            for (int j = 0; j < x; j++)
            {
                arrayStr += arrays[i][j] + " ";
            }
            //Debug.Log(arrayStr);
        }
    }

    static int[][] LoadArraysFromFile(TextAsset inputFile, out int numArrays, out int arraySize)
    {
        int[][] arrays = null;
        numArrays = 0;
        arraySize = 0;
        int currentArray = 0;
        string[] lines = inputFile.text.Split('\n');
        foreach (string line in lines)
        {
            if (line.StartsWith("#"))
            {
                // This line is a comment, so we can ignore it
                continue;
            }

            string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (arrays == null)
            {
                // This is the first line, which specifies the dimensions of the array of arrays
                numArrays = int.Parse(parts[1]);
                arraySize = int.Parse(parts[0]);
                arrays = new int[numArrays][];

                for (int i = 0; i < numArrays; i++)
                {
                    arrays[i] = new int[arraySize];
                }
            }
            else
            {
                // This is a line that specifies the contents of one of the internal arrays
                for (int i = 0; i < arraySize; i++)
                {
                    if (i < parts.Length)
                    {
                        arrays[currentArray][i] = int.Parse(parts[i]);
                    }
                    else
                    {
                        arrays[currentArray][i] = 0;
                    }
                }
                currentArray++;
            }
        }
        return arrays;
    }
}
