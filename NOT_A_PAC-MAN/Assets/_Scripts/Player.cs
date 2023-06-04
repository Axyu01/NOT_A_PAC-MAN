using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IMoveable
{
    GameObject player;
    private float speed = 1f;
    
    void Start()
    {
        player = gameObject;
    }

    public void SetMoveDirection(Vector2 direction)
    {

    }

    public void SetPosition(Vector2 direction)
    {

    }
}
