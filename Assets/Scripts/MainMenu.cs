using System.Globalization;
using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenu : MonoBehaviour
{
    private string player_name;
    private const int numOfScores = 5;

    [SerializeField] private TMP_Text scoreTable;

    private void Start()
    {
        GameManager.Instance.Menu = this;
        PrintHighScores();
    }

    public void PlayGame()
    {
        GameManager.Instance.GameScene = Scene.GameScene;
        GameManager.Instance.LoadScene();
    }

    public void ReadString(string str)
    {
        player_name = str;
    }

    public void HandleScore(float endTime)
    {
        string formattedTime = endTime.ToString("F2");
        UnityEngine.Debug.Log(player_name + " in time of " + formattedTime);
        SavePlayerScore(player_name, endTime);
        PrintHighScores();
    }

    private void SavePlayerScore(string playerName, float time)
    {
        for (int i = 1; i <= numOfScores; i++)
        {
            float savedTime = PlayerPrefs.GetFloat("BestTime_" + i, -1f); // Use -1 as an indicator for no score.
            if (time < savedTime || savedTime == -1f)
            {
                for (int j = numOfScores; j > i; j--)
                {
                    PlayerPrefs.SetFloat("BestTime_" + j, PlayerPrefs.GetFloat("BestTime_" + (j - 1), -1f));
                    PlayerPrefs.SetString("BestName_" + j, PlayerPrefs.GetString("BestName_" + (j - 1), ""));
                }
                PlayerPrefs.SetFloat("BestTime_" + i, time);
                PlayerPrefs.SetString("BestName_" + i, playerName);
                break;
            }
        }
    }


    private void PrintHighScores()
    {
        for (int i = 1; i <= numOfScores; i++)
        {
            float time = PlayerPrefs.GetFloat("BestTime_" + i, -1f);
            string name = PlayerPrefs.GetString("BestName_" + i, "");

            if (time > 0f)
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = "(No name)";
                }
                scoreTable.text += $"{i}. {name} - {time:F2} seconds\n";
            }
        }
    }
}
