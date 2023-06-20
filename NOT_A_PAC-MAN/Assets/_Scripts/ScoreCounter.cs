using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
  public TextMeshProUGUI scoreText;
  private int score = 0;

  void Start()
  {
    UpdateScoreText();
  }

  public void addScore(int points)
  {
    score += points;
    UpdateScoreText();
  }

  public void setScore(int newScore)
  {
    score = newScore;
    UpdateScoreText();
  }

  private void UpdateScoreText()
  {
    scoreText.text = "Score: " + score.ToString();
  }
}