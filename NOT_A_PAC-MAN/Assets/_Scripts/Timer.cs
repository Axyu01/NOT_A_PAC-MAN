using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
  public TextMeshProUGUI timerText; 
  private float _timer;           
  private bool _isRunning;         

  private void Update()
  {
    if (_isRunning)
    {
      _timer += Time.deltaTime; 
      UpdateTimerText();      
    }
  }
  
  public void SetTimer(float time)
  {
    _timer = time;
    UpdateTimerText();
  }
  
  public void ResetTimer()
  {
    _timer = 0f;
    UpdateTimerText();
  }
  
  public void StartTimer()
  {
    _isRunning = true;
  }
  
  public void StopTimer()
  {
    _isRunning = false;
  }
  
  public float GetTimer()
  {
    return _timer;
  }
  
  private void UpdateTimerText()
  {
    int minutes = Mathf.FloorToInt(_timer / 60f);
    int seconds = Mathf.FloorToInt(_timer % 60f);
    string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);
    timerText.text = timeText;
  }
}