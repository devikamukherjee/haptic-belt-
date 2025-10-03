using UnityEngine;
using TMPro; 
public class TimerScript : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public int startSeconds = 30;

    private float remainingTime;
    private bool isRunning = false;

    void Start()
    {
        remainingTime = startSeconds;
        isRunning = true;
        UpdateTimerUI();
    }

    void Update()
    {
        if (!isRunning) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            isRunning = false;
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        int seconds = Mathf.CeilToInt(remainingTime);
        timerText.text = seconds.ToString("D2");

        if (seconds <= 3)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = new Color(1f, 0.75f, 0.8f); 
        }
    }
}
