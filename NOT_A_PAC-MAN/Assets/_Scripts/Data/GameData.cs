using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameData", menuName = "Data/GameData")]
public class GameData : ScriptableObject
{
    public int Endscore = 0;
    public string LocalNick = "none";
    public string LocalCharacterID = "none";
    public string[] RemoteCharacterIDs = { "none", "none", "none", "none" };
    public string[] RemoteNicks = { "", "", "", "" };
    // Start is called before the first frame update
    public void ResetData()
    {
        Endscore = 0;
        LocalNick = "none";
        LocalCharacterID = "none";
        RemoteCharacterIDs = new string[] { "none", "none", "none", "none" };
        RemoteNicks = new string[] { "", "", "", "" };
    }
    public void AddPlayer(string nick,string characterID)
    {
        if (!(characterID == "gh1" || characterID == "gh2" || characterID == "gh3" || characterID == "gh4" || characterID == "pac"))
            return;
        if (nick == LocalNick) 
        { LocalCharacterID = characterID; }
        else
        {
            for(int i = 0; i < 4; i++)
            {
                if (RemoteCharacterIDs[i] == characterID)
                {
                    break;
                }
                if (RemoteCharacterIDs[i] == "none")
                {
                    RemoteCharacterIDs[i] = characterID;
                    RemoteNicks[i] = nick;
                    break;
                }
            }
        }
    }
    public string GetNick(string charID)
    { 
        if(charID == LocalCharacterID)
            return LocalNick;
        for(int i=0;i<4;i++)
        {
            if (charID == RemoteCharacterIDs[i])
                return RemoteNicks[i];
        }
        return "";
    }
}
