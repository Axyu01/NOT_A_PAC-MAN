using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ButtonConnect : MonoBehaviour
{
    [SerializeField]
    Text textField;

    public void OnConnectedToServer()
    {
        Debug.Log("Pressed!");
        if(NetworkManager.Instance!=null && textField !=null)
        {
            NetworkManager.Instance.StartListening(textField.text);
            NetworkManager.Instance.SendMsg("Witaj Craksys, zolnierek zhackowal ci serwer, oddaj v-bucksy bambiku");
            Thread.Sleep(1000);
            NetworkManager.Instance.SendMsg("Witaj Craksys, zolnierek zhackowal ci serwer, oddaj v-bucksy bambiku");
        }
    }
}
