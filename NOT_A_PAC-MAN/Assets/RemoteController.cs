using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class RemoteController : Controller
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Instance.RemoteMsgEvent += GetMsg;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetMsg(string message)
    {

    }
}
