using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    Button Button1;
    [SerializeField]
    Button Button2;
    [SerializeField]
    Button Button3;
    private void Awake()
    {
        Button1.onClick.AddListener(delegate { Resume(); });
        Button2.onClick.AddListener(delegate { MainMenu(); });
        Button3.onClick.AddListener(delegate { Quit(); });
    }

    void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
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
}
