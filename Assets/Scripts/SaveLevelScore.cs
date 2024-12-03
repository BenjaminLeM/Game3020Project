using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHighScore 
{
    public float HighScore;
    public string LevelGrade;
    public LevelHighScore(float highScore, string grade) 
    {
        HighScore = highScore;
        LevelGrade = grade;
    }
}
public class SaveLevelScore : MonoBehaviour
{
    public static void SaveLevelHighScore(float score, string grade) 
    {
        string destination = "SaveData/" + SceneManager.GetActiveScene().name + ".json";
        LevelHighScore previousHighScore = null;
        if (tryLoadData(out previousHighScore, SceneManager.GetActiveScene().name))
        {
            if (previousHighScore.HighScore < score)
            {
                LevelHighScore newHighscore = new LevelHighScore(score, grade);
                string savedata = JsonUtility.ToJson(newHighscore);
                File.WriteAllText(destination, savedata);
            }
        }
        else
        {
            LevelHighScore newHighscore = new LevelHighScore(score, grade);
            string savedata = JsonUtility.ToJson(newHighscore);
            File.WriteAllText(destination, savedata);
        }
    }

    public static bool tryLoadData(out LevelHighScore data, string LevelName)
    {
        string destination = "SaveData/";
        LevelHighScore loadedData = null;
        DirectoryInfo path = new DirectoryInfo(destination);
        FileInfo[] files = path.GetFiles("*.json");
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name == LevelName + ".json")
            {
                string dataAdress = File.ReadAllText(destination + files[i].Name);
                loadedData = JsonUtility.FromJson<LevelHighScore>(dataAdress);
            }
        }
        if (loadedData != null)
        {
            data = loadedData;
            return true;
        }
        data = null;
        return false;
    }
}
