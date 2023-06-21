using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine;
using System.Linq;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get { return instance; } private set { instance = value; } }
    private static NetworkManager instance=null;
    TcpClient tcpClient;
    NetworkStream stream;
    const int PORT_NUMBER = 50001;
    public delegate void GetMsgThreadDelegate(string message);
    public GetMsgThreadDelegate RemoteMsgEvent;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ResetManager()
    {
        //localCharacterID = "none";
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
        StartCoroutine(getMsg());
        //getMsgThread = new Thread(getMsg);
        //getMsgThread.Start();
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
        StartCoroutine(getMsg());
        //getMsgThread = new Thread(getMsg);
        //getMsgThread.Start();
    }
    public void Stop()
    {
        StopAllCoroutines();
        //if (getMsgThread != null)
            //getMsgThread.Abort();
        tcpClient.Close();

    }
    public void SendMsg(string message)
    {
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
        stream.Write(buffer, 0, buffer.Length);
    }
    //Thread getMsgThread;
    IEnumerator getMsg()
    {
        while (true)
        {
            if (stream.DataAvailable)
            {
                byte[] buffer = new byte[1024];
                stream.Read(buffer, 0, buffer.Length);
                string buffer_message = System.Text.Encoding.UTF8.GetString(buffer);
                Debug.Log("Client: " + buffer_message);
                string[] functions=buffer_message.Split(';');
                functions =functions.Take(functions.Length - 1).ToArray();//remove last msg
                //if(functions.Length > 0)
                    //Debug.Log($"first: {functions[0]} last: {functions[functions.Length-1]}");
                foreach (string function in functions)
                {
                    RemoteMsgEvent?.Invoke(function);
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
    private void OnDestroy()
    {
        Stop();
    }
}
