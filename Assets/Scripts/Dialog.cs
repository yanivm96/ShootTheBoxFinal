using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour
{
    private Button yesBtn;
    private Button noBtn;

    private void Awake()
    {
        GameManager.Instance.EndSceneDialog = this;
        Hide();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                HandleYesButton();
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                HandleNoButton();
            }
        }
    }

    public void HandleYesButton()
    {
        Hide();
        GameManager.Instance.GameScene = (Scene)(((int)GameManager.Instance.GameScene + 1) % GameManager.Instance.NumOfScenes);
        GameManager.Instance.LoadScene();
    }

    public void HandleNoButton()
    {
        Hide();
        GameManager.Instance.GameScene = Scene.MainMenu;
        GameManager.Instance.LoadScene();
    }

    public void showDialog()
    {
        GameManager.Instance.Player_.EnableInteraction(false);
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
