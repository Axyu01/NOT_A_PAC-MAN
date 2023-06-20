using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using System;

public class MapConversion : MonoBehaviour
{
    [MenuItem("Editor/Generate map")]
    static void Generate()
    {
        float xMax = 0;
        float xMin = 0;
        float yMax = 0;
        float yMin = 0;

        Queue mapLayout = new Queue();

        GameObject[] arr = GameObject.FindGameObjectsWithTag("pacman").Concat(GameObject.FindGameObjectsWithTag("ghost").Concat(GameObject.FindGameObjectsWithTag("point").Concat(GameObject.FindGameObjectsWithTag("powerup").Concat(GameObject.FindGameObjectsWithTag("pacmanSpawner").Concat(GameObject.FindGameObjectsWithTag("ghostSpawner").Concat(GameObject.FindGameObjectsWithTag("wall").Concat(GameObject.FindGameObjectsWithTag("empty")))))))).ToArray();
        var walls = GameObject.FindGameObjectsWithTag("wall");
        var floors = GameObject.FindGameObjectsWithTag("empty");

        IEnumerable<GameObject> arr2 = arr.OrderBy(GameObject => GameObject.transform.position.x);
        arr2 = arr2.OrderBy(GameObject => GameObject.transform.position.y);
        var objects = walls.Concat(floors);

        xMax = objects.Max(obj => obj.transform.position.x);
        yMax = objects.Max(obj => obj.transform.position.y);
        xMin = objects.Min(obj => obj.transform.position.x);
        yMin = objects.Min(obj => obj.transform.position.y);

        foreach (GameObject obj in arr2)
        {

            string t = obj.tag;
            switch (t)
            {
                case "empty":
                    mapLayout.Enqueue('0');
                    break;

                case "wall":
                    mapLayout.Enqueue('1');
                    break;


                case "ghostSpawner":
                    mapLayout.Enqueue('2');
                    break;

                case "ghost":
                    mapLayout.Enqueue('2');
                    break;


                case "pacman":
                    mapLayout.Enqueue('3');
                    break;

                case "pacmanSpawner":
                    mapLayout.Enqueue('3');
                    break;


                case "point":
                    mapLayout.Enqueue('4');
                    break;

                case "powerup":
                    mapLayout.Enqueue('5');
                    break;

            }

        }

        var columns_amount = (xMax - xMin) + 1;
        var rows_amount = (yMax - yMin) + 1;
        int n = 0;
        while (File.Exists("Assets/Maps/" + "Map_" + n + ".txt"))
        {
            n++;
        }
        string name = "Map_" + n;
        string newMap = "Assets/Maps/" + name + ".txt";

        using (StreamWriter newFile = File.CreateText(newMap))
        {
            newFile.WriteLine(columns_amount + " " + rows_amount);
            for (int i = 1; i < rows_amount; i++)
            {
                for (int i = 0; i < rows_amount; i++)
                {
                    for (int j = 0; j < columns_amount; j++)
                    {
                        newFile.Write(mapLayout.Dequeue());
                        newFile.Write(" ");
                    }
                }
                newFile.Write("\n");
            }
        }
    }
}