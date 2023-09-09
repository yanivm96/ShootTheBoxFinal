using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    private bool isDialogActive = false;
    [SerializeField] private TMP_Text dialogText;


    void Start()
    {
        Hide();
        GameManager.Instance.GameOverDialog = this;
    }

    void Update()
    {
        if (isDialogActive)
        {
            if (Input.anyKeyDown)
            {
                Hide();
                GameManager.Instance.GameScene = Scene.MainMenu;
                GameManager.Instance.LoadScene();
            }
        }
    }

    public void ShowLoseDialog()
    {
        Show();
        dialogText.text = "Time's up... Game Over\nPress any key to go back to menu.";
    }

    public void ShowWinDialog(float score)
    {
        Show();
        dialogText.text = $"You won!!!\nYour Score is: {score} seconds\nPress any key to go back to menu.";
    }

    private void Hide()
    {
        isDialogActive = false;
        gameObject.SetActive(false);
    }

    private void Show()
    {
        GameManager.Instance.Player_.EnableInteraction(false);
        gameObject.SetActive(true);
        isDialogActive = true;
    }
}