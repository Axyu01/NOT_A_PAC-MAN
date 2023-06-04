using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject toSpawn;
    private void Awake()
    {
        Instantiate(toSpawn, transform);
    }
}
