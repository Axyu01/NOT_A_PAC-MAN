using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    // Start is called before the first frame update
    bool ready = false;
    [SerializeField]
    GameObject readyTick;
    [SerializeField]
    GameObject serverInfo;
    [SerializeField]
    Text serverInfoText;
    [SerializeField]
    Text playerCountText;

    bool loadScene = false;
    void Start()
    {
        NetworkManager.Instance.RemoteMsgEvent += readReady;
        NetworkManager.Instance.RemoteMsgEvent += serverMesssage;
    }

    // Update is called once per frame
    void Update()
    {
        if(loadScene)
            SceneManager.LoadScene("SosivoTestScene");
        readyTick.SetActive(ready);
        if (check_game_readiness == null)
            check_game_readiness = StartCoroutine(checkGameState());
    }
Coroutine check_game_readiness = null;
IEnumerator checkGameState()
{
    yield return new WaitForSeconds(0.5f);
        //NetworkManager.Instance.SendMsg($"IsReady();");
        check_game_readiness = null;
}
public void ClickReady()
    {
        //NetworkManager.Instance.SendMsg("GameStart();");
        if(ready)
            NetworkManager.Instance.SendMsg($"SetNotReady();");
        else
            NetworkManager.Instance.SendMsg($"SetReady();");
    }
    private void readReady(string message)
    {
        if (RemoteFunction.GetFunctionName(message) == "SetReady")
        {
            updateReady(true);
        }
        else if(RemoteFunction.GetFunctionName(message) == "SetNotReady")
        {
            updateReady(false);
        }
    }
    private void serverMesssage(string message)
    {
        if (RemoteFunction.GetFunctionName(message) == "ServerInfo")
        {
            serverInfoText.text = RemoteFunction.GetFunctionArguments(message)[0];
            serverInfo.SetActive(true);
        }
        else if(RemoteFunction.GetFunctionName(message) == "PlayerCount")
        {
            playerCountText.text= $"Players {RemoteFunction.GetFunctionArguments(message)[0]}/5";
        }
        else if (RemoteFunction.GetFunctionName(message) == "Started")
        {
            //StartGame();
        }
    }
    private void updateReady(bool val)
    {
        ready= val;
    }
    private void StartGame()
    {
        loadScene = true;//load scene can be called only from main thread
        Debug.Log("Game Started");
    }
}
