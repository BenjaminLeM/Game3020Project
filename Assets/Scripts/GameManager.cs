using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float score = 0;
    float GameTime = 0;
    [SerializeField]
    TextMeshProUGUI UITimer;
    [SerializeField]
    TextMeshProUGUI Score;
    // Start is called before the first frame update
    void Start()
    {
        addScore(0);
    }

    private void Update()
    {
        UpdateGameTimer();
    }

    private void FixedUpdate()
    {
        
    }

    void UpdateGameTimer() 
    {
        GameTime += Time.deltaTime;
        UITimer.text = "Time: " + Math.Round(GameTime, 2).ToString();
    }

    void addScore(float amount) 
    {
        score += amount;
        Score.text = score.ToString();
    }
}
