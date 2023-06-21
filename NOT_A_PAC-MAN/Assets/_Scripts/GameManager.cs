using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const int POINT_SCORE = 25;
    const float ONE_POINT_TIME = 0.3f;
    const float POWER_UP_TIME = 5.0f;
    const float GAME_TIME = 20f;
    //for end of round detection
    private float maxPoints;
    private float currentPoints;

    public float gameTimeLeft;
    public float notConvertedTime;
    private float powerUpTimeLeft;

    private int score;
    private bool pacmanPoweredUp;

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
    public delegate void ScoreChangeDelegate(int newScore);
    public ScoreChangeDelegate ScoreChangeEvent;
    public delegate void ChaseChangeDelegate(bool isPacmanPoweredUp);
    public ChaseChangeDelegate PacmanPowerUpEvent;
    public delegate void TimeLeftChangeDelegate(float timeLeft);
    public TimeLeftChangeDelegate TimeLeftChangeEvent;
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
        RoundReset();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameTimeLeft-=Time.deltaTime;
        TimeLeftChangeEvent?.Invoke(gameTimeLeft);
        //if(gameTimeLeft < 0)
            //RoundReset();
        notConvertedTime += Time.deltaTime;
        int additionalScore = (int)(notConvertedTime/ONE_POINT_TIME);
        notConvertedTime -= additionalScore * ONE_POINT_TIME;
        score += additionalScore;
        ScoreChangeEvent?.Invoke(score);
        if (powerUpTimeLeft > 0f)
        {
            powerUpTimeLeft -= Time.deltaTime;
            if (powerUpTimeLeft <= 0f)
            {
                pacmanPoweredUp= false;
                PacmanPowerUpEvent?.Invoke(false);
            }
        }
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
    public void GameReset()
    {
        localCharacterID = "none";
        RemoteCharacterIDs = new string[]{ "none", "none", "none", "none"};
        RoundReset();
}
    public void RoundReset()
    {
        maxPoints = 0;
        currentPoints = 0;
        pacmanPoweredUp = false;
        notConvertedTime = 0;
        gameTimeLeft = GAME_TIME;
    }
    private void addScore(int dScore)
    {
        score += dScore;
        ScoreChangeEvent?.Invoke(score);
    }
    public void RegisterPoint()
    {
        maxPoints++;
    }
    public void ScorePoint()
    {
        currentPoints++;
        addScore(POINT_SCORE);     
    }
    public void PowerUp()
    {
        powerUpTimeLeft = POWER_UP_TIME;
        pacmanPoweredUp = true;
        PacmanPowerUpEvent?.Invoke(pacmanPoweredUp);
    }
    public bool GetPacPowerupState()
    {
        return pacmanPoweredUp;
    }
}
