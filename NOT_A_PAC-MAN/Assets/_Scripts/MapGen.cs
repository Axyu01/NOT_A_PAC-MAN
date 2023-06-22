using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapGen : MonoBehaviour
{
    [SerializeField] public GameObject[] objects;
    [SerializeField] public TextToMapParser parser;
    [SerializeField] TextAsset textAsset;
    private Vector2 borderMaxXY = Vector2.zero;
    private Vector2 borderMinXY = Vector2.zero;
    void Start()
    {
        GameManager.Instance.RegisterMapGen(this);
    }

    public void CreateMap()
    {
        int[][] map = TextToMapParser.LoadArraysFromFile(textAsset, out int numArrays, out int arraySize);
        for (int y = 0; y < numArrays; y++)
        {
            for (int x = 0; x < arraySize; x++)
            {
                Instantiate(objects[map[y][x]], transform.position + new Vector3(x, -y, 0), Quaternion.identity, transform);
            }
        }
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<mapSizePositionAdjuster>().changeCam(map.Length, map[1].Length);
        setBorderPosition(map);
    }

    private void setBorderPosition(int[][] map)
    {
        borderMaxXY.x = map[0].Length;
        borderMinXY.y = -map.Length;
        borderMaxXY.y = 0;
        borderMinXY.x = 0;
    }
    public Vector4 getBorderPosition()
    {
        return new Vector4(borderMinXY.x, borderMinXY.y, borderMaxXY.x, borderMaxXY.y);
    }
}