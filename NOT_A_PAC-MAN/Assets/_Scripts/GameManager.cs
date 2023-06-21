using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score;
    public float timeLeft;
    [SerializeField]
    public string localCharacterID = "none";
    public string[] RemoteCharacterIDs = { "none", "none", "none", "none"};
    [SerializeField]
    GameObject pacmanPrefab;
    [SerializeField]
    GameObject ghostPrefab;
    [SerializeField]
    public string gameplaySceneName = "none";
    [SerializeField]
    public string lobbySceneName = "none";
    public static GameManager Instance { get { return instance; } private set { instance = value; } }
    private static GameManager instance = null;
    public delegate void GetMsgThreadDelegate(string message);
    public GetMsgThreadDelegate RemoteMsgEvent;
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
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Instance.RemoteMsgEvent += getRemoteMsg;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void getRemoteMsg(string message)
    {
        if (RemoteFunction.GetFunctionName(message) == "Started")//start character picking
        {
            SceneManager.LoadScene(gameplaySceneName);
        }
        else if (RemoteFunction.GetFunctionName(message) == "GameEnd")//for nickname repeat
        {
            SceneManager.LoadScene(lobbySceneName);
        }
    }
    void GameReset()
    {
        localCharacterID = "none";
        RemoteCharacterIDs =new string[]{ "none", "none", "none", "none"};
    }
}
