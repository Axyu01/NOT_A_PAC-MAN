using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ButtonConnect : MonoBehaviour
{
    [SerializeField]
    Text ipText;
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
            connectMenu.SetActive(false);
            chooseCharacterMenu.SetActive(true);
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
        if(NetworkManager.Instance!=null && ipText !=null)
        {
            NetworkManager.Instance.StartListening(ipText.text);
            NetworkManager.Instance.SendMsg($"SetMyNickname({nickText.text});");
            //Thread.Sleep(1000);
            //NetworkManager.Instance.SendMsg("Witaj Craksys, zolnierek zhackowal ci serwer, oddaj v-bucksy bambiku");
        }
    }
}
