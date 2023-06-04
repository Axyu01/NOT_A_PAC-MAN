using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickCharacter : MonoBehaviour
{
    [SerializeField]
    int characterID;
    [SerializeField]
    Text pickText;
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Instance.RemoteMsgEvent += setChampion;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PickChampion(string nick)
    {
        NetworkManager.Instance.SendMsg($"PickChampion({nick},{characterID})");
    }
    void setChampion(string message)
    {
        if (RemoteFunction.GetFunctionName(message)=="SetChampion")
        {
            string[] args=RemoteFunction.GetFunctionArguments(message);
            string nick = args[0];
            if(nick=="")
            {
                pickText.text = "click this button to pick character!";
            }
            else if (int.Parse(args[1])==characterID)
            { 
                pickText.text = $"{nick} picked this character!";
            }
        }
    }
}
