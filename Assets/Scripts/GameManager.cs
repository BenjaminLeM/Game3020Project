using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float score = 0;
    float scoreMultiplier = 1f;
    float GameTime = 0;
    [SerializeField]
    TextMeshProUGUI UITimer;
    [SerializeField]
    TextMeshProUGUI Score;
    [SerializeField]
    TextMeshProUGUI ScoreMultiplier;
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
        ScoreMultTick();
    }

    void UpdateGameTimer() 
    {
        GameTime += Time.deltaTime;
        UITimer.text = "Time: " + Math.Round(GameTime, 2).ToString();
    }

    public void addScore(float amount) 
    {
        float multi = Mathf.Round(scoreMultiplier * 100) / 100;
        score += multi * amount;
        Score.text = score.ToString();
        scoreMultiplier += amount / 500;
    }

    public float getScore() 
    {
        return score;
    }

    public void ScoreMultTick()
    {
        if (scoreMultiplier > 1f)
        {
            scoreMultiplier -= 0.05f * Time.fixedDeltaTime;
        }
        else if (scoreMultiplier < 1f) 
        {
            scoreMultiplier = 1f;
        }
        ScoreMultiplier.text = "Mulitplier: X" + Mathf.Round(scoreMultiplier * 100)/100;
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
        SaveLevelScore.SaveLevelHighScore(getScore(), "a");
    }
}
