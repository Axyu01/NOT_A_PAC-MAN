using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //[SerializeField] GameObject toSpawn;
    private void Start()
    {
        if(tag =="pacmanSpawner")
            GameManager.Instance.RegisterPacManSpawner(this);
        else if (tag == "ghostSpawner")
            GameManager.Instance.RegisterGhostSpawner(this);
        //Instantiate(toSpawn, transform);
    }
}
