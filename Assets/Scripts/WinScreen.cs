using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField]
    Button Button1;
    [SerializeField]
    Button Button2;
    [SerializeField]
    Button Button3;
    [SerializeField]
    TextMeshProUGUI FinalScore;
    [SerializeField]
    TextMeshProUGUI FinalGrade;
    private void Awake()
    {
        Button1.onClick.AddListener(delegate { Retry(); });
        Button2.onClick.AddListener(delegate { MainMenu(); });
        Button3.onClick.AddListener(delegate { Quit(); });
    }

    void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
    void MainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }
    void Quit()
    {
        Application.Quit();
    }

    public void SetWinScreenScores(float score, string grade) 
    {
        FinalGrade.text = grade;
        FinalScore.text = "Score: " + score;
    }
}
