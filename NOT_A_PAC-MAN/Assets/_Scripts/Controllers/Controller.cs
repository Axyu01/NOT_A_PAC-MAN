using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    protected Player player;
    public string characterID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetPlayer(Player player)
    {
        if (this.player == null) 
        { 
            this.player = player;
        }
    }
}
