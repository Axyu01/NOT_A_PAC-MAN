using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    [SerializeField] public GameObject[] objects;
    [SerializeField] public TextToMapParser parser;

    void Start()
    {
        //parser = GetComponent<TextToMapParser>();
        CreateMap(parser.arrays);
    }

    void CreateMap(int[][] map)
    {
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                Instantiate(objects[map[j][i]], transform.position - new Vector3(i, j, 0), Quaternion.identity);
            }
        }
    }
}
