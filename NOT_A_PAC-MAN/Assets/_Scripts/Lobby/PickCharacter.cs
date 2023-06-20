using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickCharacter : MonoBehaviour
{
    [SerializeField]
    string characterID;
    [SerializeField]
    Text pickText;

    static bool championPicked;
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Instance.RemoteMsgEvent += setChampion;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PickChampion()
    {
        NetworkManager.Instance.SendMsg($"PickChampion({characterID});");
    }
    void setChampion(string message)
    {
        if (RemoteFunction.GetFunctionName(message)=="PickChampion")//Set Champion
        {
            string[] args=RemoteFunction.GetFunctionArguments(message);
            string nick = args[0];
            if (!args[1].Equals(characterID))
                return;
            if(nick=="")
            {
                pickText.text = "click this button to pick character!";
            }
            else
            { 
                pickText.text = $"{nick} picked this character!";
            }
        }
    }
}
