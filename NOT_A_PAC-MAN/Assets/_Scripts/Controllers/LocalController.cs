using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static IMoveable;

public class LocalController : Controller
{
    IMoveable.dir dir;
    int currDir = 0;
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Instance.RemoteMsgEvent += endGame;
    }

    // Update is called once per frame
    void Update()
    {
        dir.up = Input.GetKey(KeyCode.W);
        dir.down = Input.GetKey(KeyCode.S);
        dir.left = Input.GetKey(KeyCode.A);
        dir.right = Input.GetKey(KeyCode.D);
        player.SetMoveDirection(dir);
        if (Input.GetKeyDown(KeyCode.Escape))
            NetworkManager.Instance.SendMsg("EndGame();");

    }
    private void FixedUpdate()
    {
        if (dir.up)
        {
            currDir = 0;
        }
        else if (dir.down)
        {
            currDir = 1;
        }
        else if (dir.left)
        {
            currDir = 2;
        }
        else if (dir.right)
        {
            currDir = 3;
        }
        NetworkManager.Instance.SendMsg($"PlayerState({currDir}," +
            $"{player.Position.x.ToString(CultureInfo.InvariantCulture)},{player.Position.y.ToString(CultureInfo.InvariantCulture)});");
        //NetworkManager.Instance.SendMsg($"Broadcast(direction|{dir.up}|{dir.down}|{dir.left}|{dir.right});");
        //NetworkManager.Instance.SendMsg($"Broadcast(position|{player.Position.x.ToString(CultureInfo.InvariantCulture)}|{player.Position.y.ToString(CultureInfo.InvariantCulture)});");
    }
    private void endGame(string message)
    {
        if (RemoteFunction.GetFunctionName(message) == "EndGame")
        {
            Debug.Log("Game Ended!");
        }
    }
}
