using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System;
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

    private void serverMesssage(string message)
    {
        if (RemoteFunction.GetFunctionName(message) == "RemotePlayerState")
        {
            string[] arguments = RemoteFunction.GetFunctionArguments(message);

            int i = 2;
            int numOfPlayers = Convert.ToInt32(arguments[0]); // NumOfPlayers0, ID1, dir2, posX3, posY4,     ID5, dir6, posX7, posY8      ID9, dir10, posX11, posY12
            string p1ID = arguments[1];                       //                ID13, dir14, posX15, posY16  ID17, dir18, posX19, posY20 
            string p2ID = arguments[5];
            if (p2ID.Equals(characterID))
            {
                i = 6;
            }
            if (arguments.Length > 9)
            {
                string p3ID = arguments[9];
                if (p3ID.Equals(characterID))
                {
                    i = 10;
                }
                if (arguments.Length > 13)
                {
                    string p4ID = arguments[13];
                    if (p4ID.Equals(characterID))
                    {
                        i = 14;
                    }
                    if (arguments.Length > 17)
                    {
                        string p5ID = arguments[17];
                        if (p5ID.Equals(characterID))
                        {
                            i = 18;
                        }
                    }
                }
            }

            //Debug.Log("set dir");
            IMoveable.dir dir;
            dir.up = false; // bool.Parse(arguments[1]);
            dir.down = false; //bool.Parse(arguments[2]);
            dir.left = false; // bool.Parse(arguments[3]);
            dir.right = false; // bool.Parse(arguments[4]);
            if (Convert.ToInt32(arguments[i]) == 0)
            {
                dir.up = true;
                dir.down = false;
                dir.left = false;
                dir.right = false;
            }
            else if (Convert.ToInt32(arguments[i]) == 1)
            {
                dir.up = false;
                dir.down = true;
                dir.left = false;
                dir.right = false;
            }
            else if (Convert.ToInt32(arguments[i]) == 2)
            {
                dir.up = false;
                dir.down = false;
                dir.left = true;
                dir.right = false;
            }
            else if (Convert.ToInt32(arguments[i]) == 3)
            {
                dir.up = false;
                dir.down = false;
                dir.left = false;
                dir.right = true;
            }


            player.SetMoveDirection(dir);
            try
            {
                // player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                Vector2 suggestedPosition = new Vector2(float.Parse(arguments[i + 1], CultureInfo.InvariantCulture), float.Parse(arguments[i + 2], CultureInfo.InvariantCulture));
                if ((suggestedPosition - player.Position).magnitude > POSITION_ERROR)
                    player.SetPosition(suggestedPosition);
                //player.GetComponent<Rigidbody2D>().AddForce(suggestedPosition - player.Position, ForceMode2D.Impulse);
            }
            catch
            {
                Debug.LogWarning($"arguments legth {arguments.Length}");
                foreach (var arg in arguments) { Debug.LogWarning(arg); }
            }
        }


        if (RemoteFunction.GetFunctionName(message) == "Broadcast")//Depracated
        {
            string[] arguments = RemoteFunction.GetFunctionArguments(message)[0].Split('|');
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
            else if (arguments[0] == "position" && arguments.Length == 3)
            {
                //Debug.Log("set pos");
                try
                {
                    Vector2 suggestedPosition = new Vector2(float.Parse(arguments[1], CultureInfo.InvariantCulture), float.Parse(arguments[2], CultureInfo.InvariantCulture));
                    if ((suggestedPosition - player.Position).magnitude > POSITION_ERROR)
                        player.SetPosition(suggestedPosition);
                }
                catch
                {
                    Debug.LogWarning($"arguments legth {arguments.Length}");
                    foreach (var arg in arguments) { Debug.LogWarning(arg); }
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
