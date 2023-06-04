using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    [SerializeField] int sizeX;
    [SerializeField] int sizeY;
    [SerializeField] public GameObject[] objects;
    TextToMapParser parser;

    // Start is called before the first frame update
    void Start()
    {
        parser = GetComponent<TextToMapParser>();
        CreateMap(parser.arrays);
    }

    void CreateMap(int[][] map)
    {
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                CreateBlock(new Vector2(i, map[i].Length-j), map[j][i]);
            }
        }
    }

    void CreateBlock(Vector2 pos, int type)
    {
        Instantiate(objects[type], transform.position - new Vector3(pos.x, pos.y, 0), Quaternion.identity);
    }

    /*private void OnDrawGizmos()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(new Vector3(i, j, 0), new Vector3(1, 1, 1));



            }
        }
    }*/


}
