using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    GameData gameData;
    [SerializeField]
    Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = gameData.Endscore.ToString();
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
