using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ButtonConnect : MonoBehaviour
{
    [SerializeField]
    GameData gameData;
    [SerializeField]
    Text ipText;
    [SerializeField]
    Text portText;
    [SerializeField]
    Text nickText;
    [SerializeField]
    GameObject connectMenu;
    [SerializeField]
    GameObject chooseCharacterMenu;
    [SerializeField]
    GameObject serverInfo;
    [SerializeField]
    Text serverInfoText;
    public void Start()
    {
        NetworkManager.Instance.RemoteMsgEvent += getMsg;
    }
    void getMsg(string message)
    {
        if (RemoteFunction.GetFunctionName(message)=="StartCharacterLobby")//start character picking
        {
            chooseCharacterMenu.SetActive(true);
            connectMenu.SetActive(false);
        }
        else if (RemoteFunction.GetFunctionName(message) == "BadNickname")//for nickname repeat
        {
            serverInfoText.text = RemoteFunction.GetFunctionArguments(message)[0];
            serverInfo.SetActive(true);
        }
    }

    public void OnConnectedToServer()
    {
        Debug.Log("Pressed!");
        try
        {
            if (NetworkManager.Instance != null && ipText != null)
            {
                NetworkManager.Instance.ResetManager();
                NetworkManager.Instance.StartListening(ipText.text, int.Parse(portText.text));
                NetworkManager.Instance.SendMsg($"SetMyNickname({nickText.text});");
                gameData.ResetData();//redudant becouse of function bollow that contains it
                GameManager.Instance.GameReset();
                gameData.LocalNick= nickText.text;
                //Thread.Sleep(1000);
                //NetworkManager.Instance.SendMsg("Witaj Craksys, zolnierek zhackowal ci serwer, oddaj v-bucksy bambiku");
            }
        }
        catch
        {
            Debug.LogWarning("Error with server connection!");
        }
    }
}
