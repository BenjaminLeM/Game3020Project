using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Button Button1;
    [SerializeField]
    Button Button2;
    [SerializeField]
    Button Button3;
    [SerializeField]
    Button Button4;
    [SerializeField]
    Button Button5;
    [SerializeField]
    Button Button6;
    [SerializeField]
    Text TitleText;
    [SerializeField]
    Text InstructionText;
    [SerializeField]
    Text ControlsText;
    private void Awake()
    {
        Button1.onClick.AddListener(delegate { StartGame(); });
        Button2.onClick.AddListener(delegate { SwitchMenuToInstruction(); });
        Button3.onClick.AddListener(delegate { Quit(); });
        Button4.onClick.AddListener(delegate { SwitchMenuToMainMenu(); });
        Button5.onClick.AddListener(delegate { OpenInstructions(); });
        Button6.onClick.AddListener(delegate { OpenControls(); });
    }

    void StartGame() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    #region

    void SwitchActiveButtons() 
    {
        Button1.gameObject.SetActive(!Button1.gameObject.active);
        Button2.gameObject.SetActive(!Button2.gameObject.active);
        Button3.gameObject.SetActive(!Button3.gameObject.active);
        Button4.gameObject.SetActive(!Button4.gameObject.active);
        Button5.gameObject.SetActive(!Button5.gameObject.active);
        Button6.gameObject.SetActive(!Button6.gameObject.active);
    }
    void SwitchMenuToInstruction() 
    {
        SwitchActiveButtons();
        TitleText.gameObject.SetActive(!TitleText.gameObject.active);
    }

    void SwitchMenuToMainMenu() 
    {
        SwitchActiveButtons();
        TitleText.gameObject.SetActive(!TitleText.gameObject.active);
        if(InstructionText.gameObject.active)
            InstructionText.gameObject.SetActive(false);
        else if(ControlsText.gameObject.active)
            ControlsText.gameObject.SetActive(false);
    }

    void OpenInstructions() 
    {
        InstructionText.gameObject.SetActive(true);
        if (ControlsText.gameObject.active) 
        {
            ControlsText.gameObject.SetActive(false);
        }
    }

    void OpenControls() 
    {
        ControlsText.gameObject.SetActive(true);
        if (InstructionText.gameObject.active)
        {
            InstructionText.gameObject.SetActive(false);
        }
    }

    #endregion
    void Quit() 
    {
        Application.Quit();
    }
}
