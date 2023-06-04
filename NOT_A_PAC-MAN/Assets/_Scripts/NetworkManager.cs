using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get { return instance; } private set { instance = value; } }
    private static NetworkManager instance=null;
    TcpClient tcpClient;
    NetworkStream stream;
    const int PORT_NUMBER = 50001;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Start is called before the first frame update
    public void StartListening()
    {
        if (tcpClient != null)
        {
            tcpClient.Close();
        }
        tcpClient = new TcpClient();
        tcpClient.Connect("localhost",PORT_NUMBER);
        stream = tcpClient.GetStream();
        getMsgThread = new Thread(getMsg);
        getMsgThread.Start();
    }
    public void StartListening(string serverAdress)
    {
        if (tcpClient != null)
        {
            tcpClient.Close();
        }
        tcpClient = new TcpClient();
        tcpClient.Connect(IPAddress.Parse(serverAdress),PORT_NUMBER);
        Debug.Log("Connected to " + serverAdress);
        stream = tcpClient.GetStream();
        getMsgThread = new Thread(getMsg);
        getMsgThread.Start();
    }
    public void Stop()
    {
        if (getMsgThread != null)
            getMsgThread.Abort();
        tcpClient.Close();

    }
    public void SendMsg(string message)
    {
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
        stream.Write(buffer, 0, buffer.Length);
    }
    Thread getMsgThread;
    void getMsg()
    {
        while (true)
        {
            byte[] buffer = new byte[1024];
            stream.Read(buffer, 0, buffer.Length);
            Debug.Log("Client: " + System.Text.Encoding.UTF8.GetString(buffer));
            
            Thread.Sleep(200);
        }
    }
    public delegate void GetMsgThreadDelegate(string message);
    public GetMsgThreadDelegate RemoteMsgEvent;
    private void OnDestroy()
    {
        Stop();
    }
}
