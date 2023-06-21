using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapSizePositionAdjuster : MonoBehaviour
{
    private MapGen mg;

    void Start()
    {
        mg = GameObject.FindGameObjectWithTag("mapgen").GetComponent<MapGen>();
    }

    public void changeCam(int width,int height)
    {
        Vector3 pos = Vector3.zero;
        GetComponent<Camera>().orthographicSize = GetComponent<Camera>().orthographicSize*Mathf.Max(width,height/2)+2; 
        pos.y = mg.transform.position.x-(width/2) +1.5f;
        pos.x = mg.transform.position.y+(height/2) -0.5f;
        pos.z = -10;
        transform.position = pos;

    }
}
