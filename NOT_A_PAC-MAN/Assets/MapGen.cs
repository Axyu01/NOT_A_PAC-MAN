using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    [SerializeField] int sizeX;
    [SerializeField] int sizeY;
    [SerializeField] GameObject wall;


    // Start is called before the first frame update
    void Start()
    {
        CreateMap(sizeX, sizeY, wall);
    }

    void CreateMap(int x, int y, GameObject wall)
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                CreateBlock(new Vector2(i, j), 1);
            }
        }
    }

    void CreateBlock(Vector2 pos, int type)
    {
        if (type == 0)//empty
        {
        //pass
        }
        else if (type == 1)//wall
        {
            Instantiate(wall, new Vector3(pos.x, pos.y, 0), Quaternion.identity);

        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(new Vector3(i, j, 0), new Vector3(1, 1, 1));



            }
        }


    }


}
