using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PCController : Controller
{
    [SerializeField]
    KeyCode up = KeyCode.W;
    [SerializeField]
    KeyCode down = KeyCode.S;
    [SerializeField]
    KeyCode left = KeyCode.A;
    [SerializeField]
    KeyCode right = KeyCode.D;

    IMoveable.dir dir;
    // Update is called once per frame
    void Update()
    {
        dir.up = Input.GetKey(up);
        dir.down = Input.GetKey(down);
        dir.left = Input.GetKey(left);
        dir.right = Input.GetKey(right);
        player.SetMoveDirection(dir);
    }
}
