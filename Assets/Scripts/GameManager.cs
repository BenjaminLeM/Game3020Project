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
    [SerializeField]
    Canvas PlayerUI;
    [SerializeField]
    Canvas PauseMenu;
    [SerializeField]
    Canvas WinMenu;
    [SerializeField]
    TextMeshProUGUI WinScoreText;
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

    public void addScore(float amount) 
    {
        score += amount;
        Score.text = score.ToString();
    }

    public float getScore() 
    {
        return score;
    }
    public void pauseGame() 
    {
        PlayerUI.gameObject.SetActive(false);
        PauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    public void winGame() 
    {
        PlayerUI.gameObject.SetActive(false);
        WinMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        WinScoreText.text = "Score: " + getScore();
    }
}
