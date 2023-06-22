using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static IMoveable;

public class LocalController : Controller
{
    IMoveable.dir dir;
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
        NetworkManager.Instance.SendMsg($"PlayerState({dir.up},{dir.down},{dir.left},{dir.right}," +
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
