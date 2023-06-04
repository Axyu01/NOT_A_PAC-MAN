using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IMoveable
{
    GameObject player;
    private float speed = 1f;
    int dirX, dirY = 0;
    Rigidbody2D rb;
    void Start()
    {
        player = gameObject;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        applyMovement();
    }

    private void applyMovement()
    {
        rb.AddForce(new Vector2(dirX,dirY)*speed);
    }

    public void SetMoveDirection(IMoveable.dir dir)
    {
        dirX = (dir.up == true ? 1 : 0);
        dirX = (dir.down == true ? -1 : 0);
        dirY = (dir.right == true ? 1 : 0);
        dirY = (dir.left == true ? -1 : 0);
    }

    public void SetPosition(Vector2 direction)
    {

    }
}
