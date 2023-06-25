using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IMoveable
{
    const float PACMAN_SPEED = 4f;
    const float GHOST_SPEED = 5f;
    [SerializeField]
    Color baseColor = Color.white;
    GameObject player;
    private float speed = 1f;
    private bool pacPoweredUp = false;
    Vector2Int dir = Vector2Int.zero;
    public Vector2Int Direction { get { return dir; } private set { dir = value; } }
    public Vector2 Position { get { return transform.position; } }//moze overengineering ale co tam xd,bedzie czytelne w kontrolerze
    Rigidbody2D rb;
    Animator anim;
    MapGen mg;
    Vector4 borderPos;
    float cd = 0;
    void Start()
    {
        player = gameObject;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        mg = GameObject.FindWithTag("mapgen").GetComponent<MapGen>();
        borderPos = mg.getBorderPosition();
        GameManager.Instance.PacmanPowerUpEvent += GetPoweredUpInfo;

        if (gameObject.tag == "pacman")
        {
            speed = PACMAN_SPEED;
        }
        else if (gameObject.tag == "ghost")
        {
            if (GameManager.Instance.NumberOfPlayers == 0)
                speed = GHOST_SPEED;
            else
                speed = 1f + (GHOST_SPEED - 2f) / GameManager.Instance.NumberOfPlayers;
        }
    }

    private void FixedUpdate()
    {
        applyMovement();
        ifInBorder();
        cd -= Time.deltaTime;
        cd = Mathf.Max(0, cd);
    }

    private void applyMovement()
    {
        rb.velocity = new Vector2(dir.x, dir.y) * speed;
        //rb.AddForce(new Vector2(dir.x,dir.y)*speed);
    }

    private void ifInBorder()
    {
        if (cd == 0)
        {
            if (transform.position.x > borderPos[2])
            {
                SetPosition(new Vector2(borderPos[0], transform.position.y));
                cd = 0.7f;
            }
            else if (transform.position.x < borderPos[0])
            {
                SetPosition(new Vector2(borderPos[2], transform.position.y));
                cd = 0.7f;
            }
            else if (transform.position.y > borderPos[3])
            {
                SetPosition(new Vector2(transform.position.x, borderPos[1]));
                cd = 0.7f;
            }
            else if (transform.position.y < borderPos[1])
            {
                SetPosition(new Vector2(transform.position.x, borderPos[3]));
                cd = 0.7f;
            }
        }
    }

    public void SetMoveDirection(IMoveable.dir IDir)
    {
        dir = Vector2Int.zero;
        dir.y += (IDir.up == true ? 1 : 0);
        dir.y += (IDir.down == true ? -1 : 0);
        dir.x += (IDir.right == true ? 1 : 0);
        dir.x += (IDir.left == true ? -1 : 0);
        if (dir.x != 0)
            GetComponent<SpriteRenderer>().flipX = dir.x < 0 ? true : false;
        anim.SetFloat("IsFacingY", dir.y);
    }

    public void SetPosition(Vector2 position)
    {
        gameObject.transform.position = position;
    }
    private void GetPoweredUpInfo(bool isPacPoweredUp)
    {
        pacPoweredUp = isPacPoweredUp;
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (pacPoweredUp)
        {
            if (gameObject.tag == "pacman")
            {
                speed = PACMAN_SPEED * 1.5f;
            }
            else if (gameObject.tag == "ghost")
            {
                speed = 1f;
                spriteRenderer.color = Color.cyan;
            }
        }
        else
        {
            if (gameObject.tag == "pacman")
            {
                speed = PACMAN_SPEED;
            }
            else if (gameObject.tag == "ghost")
            {
                if (GameManager.Instance.NumberOfPlayers == 0)
                    speed = GHOST_SPEED;
                else
                    speed = 1f + (GHOST_SPEED - 2f) / GameManager.Instance.NumberOfPlayers;
                spriteRenderer.color = baseColor;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag == "pacman" && collision.gameObject.tag == "ghost")
        {
            if (pacPoweredUp)
            {
                GameManager.Instance.ScoreKill(collision.gameObject.GetComponent<Player>());
            }
            else
            {
                GameManager.Instance.RoundEnd();
            }
        }
    }
}
