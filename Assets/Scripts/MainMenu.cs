using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    GameObject Levels;
    [SerializeField]
    List<Button> LevelButtons = new List<Button>();
    private void Awake()
    {
        Button1.onClick.AddListener(delegate { OpenLevelUI(); });
        Button2.onClick.AddListener(delegate { SwitchMenuToInstruction(); });
        Button3.onClick.AddListener(delegate { Quit(); });
        Button4.onClick.AddListener(delegate { SwitchMenuToMainMenu(); });
        Button5.onClick.AddListener(delegate { OpenInstructions(); });
        Button6.onClick.AddListener(delegate { OpenControls(); });
        LevelButtons[0].onClick.AddListener(delegate { StartGame("FirstLevel"); });
        LevelButtons[1].onClick.AddListener(delegate { StartGame("SecondLevel"); });
        LevelButtons[2].onClick.AddListener(delegate { StartGame("ThirdLevel"); });
    }

    void OpenLevelUI() 
    {
        SwitchMainButtonState();
        List<LevelHighScore> levelData = new List<LevelHighScore>();
        LevelHighScore data;
        if (SaveLevelScore.tryLoadData(out data, "FirstLevel"))
            levelData.Add(data);
        if (SaveLevelScore.tryLoadData(out data, "SecondLevel"))
            levelData.Add(data);
        if (SaveLevelScore.tryLoadData(out data, "ThirdLevel"))
            levelData.Add(data);
        for (int i = 0; i < levelData.Count; i++) 
        {
            LevelButtons[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text += levelData[i].HighScore.ToString();
            LevelButtons[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Grade: " + levelData[i].LevelGrade;
        }

        if (levelData.Count < 2) 
        {
            LevelButtons[2].transform.GetChild(0).gameObject.SetActive(true);
            LevelButtons[2].GetComponent<Button>().enabled = false;
            if (levelData.Count < 1) 
            {
                LevelButtons[1].transform.GetChild(0).gameObject.SetActive(true);
                LevelButtons[1].GetComponent<Button>().enabled = false;
            }
        }
        Levels.SetActive(true);
        
    }
    void StartGame(string SceneName) 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneName);
    }

    #region
    void SwitchMainButtonState() 
    {
        Button1.gameObject.SetActive(!Button1.gameObject.activeSelf);
        Button2.gameObject.SetActive(!Button2.gameObject.activeSelf);
        Button3.gameObject.SetActive(!Button3.gameObject.activeSelf);
    }

    void SwitchInstructionButtonState() 
    {
        Button4.gameObject.SetActive(!Button4.gameObject.activeSelf);
        Button5.gameObject.SetActive(!Button5.gameObject.activeSelf);
        Button6.gameObject.SetActive(!Button6.gameObject.activeSelf);
    }
    void SwitchMainmenuToInstructions() 
    {
        SwitchMainButtonState();
        SwitchInstructionButtonState();
    }
    void SwitchMenuToInstruction() 
    {
        SwitchMainmenuToInstructions();
        TitleText.gameObject.SetActive(!TitleText.gameObject.activeSelf);
    }

    void SwitchMenuToMainMenu() 
    {
        SwitchMainmenuToInstructions();
        TitleText.gameObject.SetActive(!TitleText.gameObject.activeSelf);
        if(InstructionText.gameObject.activeSelf)
            InstructionText.gameObject.SetActive(false);
        else if(ControlsText.gameObject.activeSelf)
            ControlsText.gameObject.SetActive(false);
    }

    void OpenInstructions() 
    {
        InstructionText.gameObject.SetActive(true);
        if (ControlsText.gameObject.activeSelf) 
        {
            ControlsText.gameObject.SetActive(false);
        }
    }

    void OpenControls() 
    {
        ControlsText.gameObject.SetActive(true);
        if (InstructionText.gameObject.activeSelf)
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
