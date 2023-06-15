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
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                Instantiate(objects[map[y][x]], transform.position + new Vector3(x, -y, 0), Quaternion.identity);
            }
        }
    }
}
