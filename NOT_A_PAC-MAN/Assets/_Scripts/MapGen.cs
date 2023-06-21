using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    [SerializeField] public GameObject[] objects;
    [SerializeField] public TextToMapParser parser;
    private Vector2 borderMaxXY = Vector2.zero;
    private Vector2 borderMinXY = Vector2.zero;
    void Start()
    {
        //parser = GetComponent<TextToMapParser>();
        CreateMap(parser.arrays);
    }

    void CreateMap(int[][] map)
    {
        int[][] mapDuplicate = map;
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                Instantiate(objects[map[y][x]], transform.position + new Vector3(x, -y, 0), Quaternion.identity, transform);
            }
        }
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<mapSizePositionAdjuster>().changeCam(map.Length, map[1].Length);
        setBorderPosition(mapDuplicate);
    }

    private void setBorderPosition(int[][] map)
    {
        borderMaxXY.x = map[0].Length - 0.5f;
        borderMinXY.y = -map.Length + 0.5f;
        borderMaxXY.y = 0.5f;
        borderMinXY.x = -0.5f;
    }
    public Vector4 getBorderPosition()
    {
        return new Vector4(borderMinXY.x, borderMinXY.y, borderMinXY.x, borderMinXY.y);
    }
}