using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Scene scene = Scene.MainMenu;
    private Player player;
    private MainMenu mainMenu;
    private Timer timer;
    private RandomCubes cubes;
    private float timerDuration = 35;
    private GameOver gameOverDialog;

    private Dialog endSceneDialog;
    private const int numOfScenes = 4;

    private float totalTime;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadScene();
    }

    public int NumOfScenes
    {
        get { return numOfScenes; }
    }
    public Scene GameScene
    {
        get { return scene; }
        set { scene = value; }
    }
    public Player Player_
    {
        get { return player; }
        set { player = value; }
    }

    public Dialog EndSceneDialog
    {
        get { return endSceneDialog; }
        set { endSceneDialog = value; }
    }
    public MainMenu Menu
    {
        get { return mainMenu; }
        set { mainMenu = value; }
    }

    public RandomCubes Cubes
    {
        get { return cubes; }
        set { cubes = value; }
    }

    public GameOver GameOverDialog
    {
        get { return gameOverDialog; }
        set { gameOverDialog = value; }
    }

    public Timer Timer_
    {
        get { return timer; }
        set
        {
            timer = value;
            if (timer != null)
            {
                timer.OnTimerDone += HandleTimerDone;
            }
        }
    }

    public void LoadScene()
    {
        if (GameScene != Scene.MainMenu)
        {
            player.EnableInteraction(true);
            player.ResetToStartPosition();
            timer.StartTimer(timerDuration);
            cubes.RespawnCubes();
        }
        else
        {
            totalTime = 0f;
            player.EnableInteraction(false);
        }
        SceneManager.LoadScene(scene.ToString());
    }

    public void HandleAllCubesDestroyed(float duration)
    {
        player.Sound.StopRunning();
        totalTime += duration;
        if (GameScene == Scene.GameScene3)
        {
            gameOverDialog.ShowWinDialog(totalTime);
            mainMenu.HandleScore(totalTime);
        }
    }

    private void HandleTimerDone()
    {
        player.Sound.StopRunning();
        cubes.reset();
        gameOverDialog.ShowLoseDialog();
    }
}

public enum Scene
{
    MainMenu,
    GameScene,
    GameScene2,
    GameScene3
}