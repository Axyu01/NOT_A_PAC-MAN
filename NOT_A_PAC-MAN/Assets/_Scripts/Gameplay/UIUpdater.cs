using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    const float PRESURE_TIME=10f;
    [SerializeField]
    Text score;
    [SerializeField]
    Text timeLeft;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ScoreChangeEvent+=reactToScoreChange;
        GameManager.Instance.TimeLeftChangeEvent += reactToTimeChange;
    }
    void reactToTimeChange(float timeLeft)
    {
        if(timeLeft>PRESURE_TIME)
            this.timeLeft.text = timeLeft.ToString("0.0");
        else
            this.timeLeft.text = timeLeft.ToString("0.00");
    }
    void reactToScoreChange(int score)
    {
        this.score.text = $"Score:{score}";
    }
}
