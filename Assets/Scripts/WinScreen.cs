using System.Collections;
using System.Collections.Generic;
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
    private void Awake()
    {
        Button1.onClick.AddListener(delegate { Retry(); });
        Button2.onClick.AddListener(delegate { MainMenu(); });
        Button3.onClick.AddListener(delegate { Quit(); });
    }

    void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
