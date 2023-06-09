using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IMoveable
{
    GameObject player;
    private float speed = 1f;
    Vector2Int dir = Vector2Int.zero;
    public Vector2Int Direction { get { return dir; } private set { dir = value; } }
    public Vector2 Position { get { return transform.position; }}//moze overengineering ale co tam xd,bedzie czytelne w kontrolerze
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
        rb.velocity = new Vector2(dir.x, dir.y) * speed;
        //rb.AddForce(new Vector2(dir.x,dir.y)*speed);
    }

    public void SetMoveDirection(IMoveable.dir IDir)
    {
        dir = Vector2Int.zero;
        dir.y += (IDir.up == true ? 1 : 0);
        dir.y += (IDir.down == true ? -1 : 0);
        dir.x += (IDir.right == true ? 1 : 0);
        dir.x += (IDir.left == true ? -1 : 0);
    }

    public void SetPosition(Vector2 position)
    {
        gameObject.transform.position = position;
    }
}
