using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
//using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const int POINT_SCORE = 5;
    const int POWERUP_SCORE = 95;
    const float ONE_POINT_TIME = 0.5f;
    const float ADDITIONAL_POINT_TIME = 0.1f;
    const float POWER_UP_TIME = 5.0f;
    const float GAME_TIME = 150f;
    [SerializeField]
    GameData gameData;
    //for end of round detection
    private float maxPoints;
    private float currentPoints;

    public float gameTimeLeft;
    public float notConvertedTime;
    private float powerUpTimeLeft;

    private bool pacmanPoweredUp;

    public int NumberOfPlayers {
        get {
            int numberOfPlayers = 0;
            foreach (string player in gameData.RemoteCharacterIDs)
            {
                if (player != "none")
                {
                    numberOfPlayers++;
                }
            }
            if (gameData.LocalCharacterID != "none")
                numberOfPlayers++;
            return numberOfPlayers;
        }
    }
    List<Spawner> pacmanSpawners;
    List<Spawner> ghostSpawners;
    Player localPlayer;
    List<Player> remotePlayers;
    MapGen mapGen; 
    int killStreak = 0;
    //Prafabs to instatiate
    [SerializeField]
    GameObject pacmanPrefab;
    [SerializeField]
    GameObject ghostPrefab;
    [SerializeField]
    GameObject localControllerPrefab;
    [SerializeField]
    GameObject remoteControllerPrefab;
    //SceneLoading
    [SerializeField]
    public string gameplaySceneName = "none";
    [SerializeField]
    public string endSceneName = "none";
    //Singleton pattern
    public static GameManager Instance { get { return instance; } private set { instance = value; } }
    private static GameManager instance = null;
    //Function calls
    public delegate void ScoreChangeDelegate(int newScore);
    public event ScoreChangeDelegate ScoreChangeEvent;
    public delegate void ChaseChangeDelegate(bool isPacmanPoweredUp);
    public event ChaseChangeDelegate PacmanPowerUpEvent;
    public delegate void TimeLeftChangeDelegate(float timeLeft);
    public event TimeLeftChangeDelegate TimeLeftChangeEvent;
    private int score;
    public void GameReset()
    {
        score = 0;
        gameData.ResetData();
        RoundReset();
    }
    public void RoundReset()
    {
        maxPoints = 0;
        currentPoints = 0;
        pacmanPoweredUp = false;
        notConvertedTime = 0;
        gameTimeLeft = GAME_TIME;
        pacmanSpawners = new List<Spawner>();
        ghostSpawners = new List<Spawner>();
        localPlayer= null;
        remotePlayers= new List<Player>();
    }

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
        if (localPlayer == null)
            return;
        if (gameTimeLeft > 0f)
        {
            gameTimeLeft -= Time.deltaTime;
            TimeLeftChangeEvent?.Invoke(gameTimeLeft);
            //if(gameTimeLeft < 0)
            //RoundReset();
            notConvertedTime += Time.deltaTime;
            int additionalScore = (int)(notConvertedTime / ONE_POINT_TIME);
            notConvertedTime -= additionalScore * ONE_POINT_TIME;
            score += additionalScore;
            ScoreChangeEvent?.Invoke(score);
        }
        else
        {
            RoundEnd();
        }
        if (powerUpTimeLeft > 0f)
        {
            powerUpTimeLeft -= Time.deltaTime;
            if (powerUpTimeLeft <= 0f)
            {
                killStreak = 0;
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
            SceneManager.LoadScene(endSceneName);
        }
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
        if(currentPoints== maxPoints)
            RoundEnd();
        addScore(POINT_SCORE);     
    }
    public void ScoreKill(Player player)
    {
        if (player.gameObject.activeSelf == false)
            return;
        player.gameObject.SetActive(false);
        killStreak++;
        addScore(200 * killStreak);
        StartCoroutine(PutOnDeathTimer(player));
    }
    IEnumerator PutOnDeathTimer(Player player)
    {
        yield return new WaitForSeconds(5);
        Respawn(player);
    }
    public void Respawn(Player player)
    {
        StartCoroutine(delayedRespawn(player));
    }
    IEnumerator delayedRespawn(Player player)
    {
        bool playerfound = true;
        Vector2 spawnPosition = Vector2.zero;
        while (playerfound)
        {
            if(player.gameObject.tag=="pacman")
            {
                int rand = (int)(Mathf.Round((pacmanSpawners.Count - 1) * Random.value));
                Debug.Log($"random value {rand}");
                spawnPosition = pacmanSpawners[rand].transform.position;
                break;
            }
            {
                Vector2 randomSpawnerPosition = ghostSpawners[(int)(Mathf.Round((ghostSpawners.Count - 1) * Random.value))].transform.position;
                Collider2D[] colliders = Physics2D.OverlapAreaAll(randomSpawnerPosition - Vector2.one * 0.5f, randomSpawnerPosition + Vector2.one * 0.5f);
                foreach (Collider2D collider in colliders)
                {
                    Player foundPlayer = collider.GetComponent<Player>();

                    if (foundPlayer == null)
                    {
                        playerfound = false;
                        spawnPosition = randomSpawnerPosition;
                        break;
                    }
                }
            }
            if (playerfound == false)
                break;
            foreach (Spawner spawner in ghostSpawners)
            {
                Vector2 someSpawnerPosition = spawner.transform.position;
                Collider2D[] colliders = Physics2D.OverlapAreaAll(someSpawnerPosition - Vector2.one * 0.5f, someSpawnerPosition + Vector2.one * 0.5f);
                foreach (Collider2D collider in colliders)
                {
                    Player foundPlayer = collider.GetComponent<Player>();

                    if (foundPlayer == null)
                    {
                        playerfound = false;
                        spawnPosition = someSpawnerPosition;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        player.transform.position = spawnPosition;
        player.gameObject.SetActive(true);
    }
    public void PowerUp()
    {
        addScore(POWERUP_SCORE);
        powerUpTimeLeft = POWER_UP_TIME;
        pacmanPoweredUp = true;
        PacmanPowerUpEvent?.Invoke(pacmanPoweredUp);
    }
    public bool GetPacPowerupState()
    {
        return pacmanPoweredUp;
    }
    public void RegisterPacManSpawner(Spawner spawner)
    {
        pacmanSpawners.Add(spawner);
    }
    public void RegisterGhostSpawner(Spawner spawner)
    {
        ghostSpawners.Add(spawner);
    }

    public void RegisterMapGen(MapGen mapGen)
    {
        this.mapGen= mapGen;
        mapGen.CreateMap();
        StartCoroutine(lateGameStart());
    }
    IEnumerator lateGameStart()
    {
        yield return new WaitForSeconds(1.1f);
        Controller localC = Instantiate(localControllerPrefab).GetComponent<Controller>();
        if (gameData.LocalCharacterID == "pac")
        {
            localPlayer = Instantiate(pacmanPrefab).GetComponent<Player>();
        }
        else
        {
            localPlayer = Instantiate(ghostPrefab).GetComponent<Player>();
        }
        localC.characterID = gameData.LocalCharacterID;
        localC.SetPlayer(localPlayer);
        localPlayer.gameObject.SetActive(false);
        Respawn(localPlayer);


        for (int i = 0; i < 4; i++)
        {
            Controller remoteC = Instantiate(remoteControllerPrefab).GetComponent<Controller>();
            Player remotePlayer;
            if (gameData.RemoteCharacterIDs[i] == "pac")
            {
                remotePlayer = Instantiate(pacmanPrefab).GetComponent<Player>();
            }
            else if (gameData.RemoteCharacterIDs[i] == "gh1" || gameData.RemoteCharacterIDs[i] == "gh2" || gameData.RemoteCharacterIDs[i] == "gh3" || gameData.RemoteCharacterIDs[i] == "gh4")
            {
                remotePlayer = Instantiate(ghostPrefab).GetComponent<Player>();
            }
            else
                continue;
            remoteC.characterID = gameData.RemoteCharacterIDs[i];
            remoteC.SetPlayer(remotePlayer);
            remotePlayers.Add(remotePlayer);
            remotePlayer.gameObject.SetActive(false);
            Respawn(remotePlayer);
        }
    }
    public void RoundEnd() //dodac game end serwerowy
    {
        if(gameTimeLeft>0f)
            score+=(int)(gameTimeLeft / ONE_POINT_TIME)*2;
        gameData.Endscore += score;
        //GameReset();
        SceneManager.LoadScene(endSceneName);
    }
}
