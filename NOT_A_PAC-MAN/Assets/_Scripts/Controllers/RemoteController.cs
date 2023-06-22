using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
//using UnityEditor.ShortcutManagement;
using UnityEngine;

public class RemoteController : Controller
{
    const float POSITION_ERROR = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Instance.RemoteMsgEvent += serverMesssage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void serverMesssage(string message)
    {
        if(RemoteFunction.GetFunctionName(message) == "RemotePlayerState")
        {
            string[] arguments = RemoteFunction.GetFunctionArguments(message);
            if (arguments.Length == 7 && arguments[0].Equals(characterID))
            {
                //Debug.Log("set dir");
                IMoveable.dir dir;
                dir.up = bool.Parse(arguments[1]);
                dir.down = bool.Parse(arguments[2]);
                dir.left = bool.Parse(arguments[3]);
                dir.right = bool.Parse(arguments[4]);
                player.SetMoveDirection(dir);
                try
                {
                    Vector2 suggestedPosition = new Vector2(float.Parse(arguments[5], CultureInfo.InvariantCulture), float.Parse(arguments[6], CultureInfo.InvariantCulture));
                    if ((suggestedPosition - player.Position).magnitude > POSITION_ERROR)
                        player.SetPosition(suggestedPosition);
                }
                catch
                {
                    Debug.LogWarning($"arguments legth {arguments.Length}");
                    foreach (var arg in arguments) { Debug.LogWarning(arg); }
                }
            }
        }
        if (RemoteFunction.GetFunctionName(message) == "Broadcast")//Depracated
        {
            string[] arguments= RemoteFunction.GetFunctionArguments(message)[0].Split('|');
            if (arguments[0] == "direction" && arguments.Length == 5)
            {
                //Debug.Log("set dir");
                IMoveable.dir dir;
                dir.up = bool.Parse(arguments[1]);
                dir.down = bool.Parse(arguments[2]);
                dir.left = bool.Parse(arguments[3]);
                dir.right = bool.Parse(arguments[4]);
                player.SetMoveDirection(dir);
            }
            else if (arguments[0] == "position" && arguments.Length==3)
            {
                //Debug.Log("set pos");
                try
                {
                    Vector2 suggestedPosition = new Vector2(float.Parse(arguments[1], CultureInfo.InvariantCulture), float.Parse(arguments[2], CultureInfo.InvariantCulture));
                    if((suggestedPosition-player.Position).magnitude>POSITION_ERROR)
                        player.SetPosition(suggestedPosition);
                }
                catch
                {
                    Debug.LogWarning($"arguments legth {arguments.Length}");
                    foreach(var arg in arguments) { Debug.LogWarning(arg); }
                }
            }
            //Broadcast logic
        }
        else if (RemoteFunction.GetFunctionName(message) == "EndGame")
        {
            //End game
        }
    }
}
