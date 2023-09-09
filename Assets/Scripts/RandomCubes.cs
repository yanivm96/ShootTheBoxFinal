using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCubes : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    private int totalCubes = 12;
    private int destroyedCubes = 0;
    private GameObject[] randomCubes;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        IntialCubes();
    }

    private void IntialCubes()
    {
        randomCubes = new GameObject[totalCubes];
        for (int i = 0; i < totalCubes; i++)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-16, 16), Random.Range(7, 11), Random.Range(-16, 16));
            randomCubes[i] = Instantiate(cube, randomSpawnPosition, Quaternion.identity);
            Rigidbody rb = randomCubes[i].AddComponent<Rigidbody>();
            rb.useGravity = true;
            Box boxComponent = randomCubes[i].AddComponent<Box>();
            boxComponent.Cube = randomCubes[i];
            randomCubes[i].SetActive(false);
            DontDestroyOnLoad(randomCubes[i]);
        }
    }

    private void InitailCubesPosition()
    {
        for (int i = 0; i < totalCubes; i++)
        {
            randomCubes[i].transform.position = new Vector3(Random.Range(-16, 16), Random.Range(7, 11), Random.Range(-16, 16));
            randomCubes[i].transform.rotation = Quaternion.identity;
            randomCubes[i].transform.rotation = Quaternion.identity;
            Rigidbody rb = randomCubes[i].GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void RespawnCubes()
    {
        reset();
        InitailCubesPosition();
        ResetCubeColors();
        for (int i = 0; i < totalCubes; i++)
        {
            randomCubes[i].SetActive(true);
        }
    }

    private void ResetCubeColors()
    {
        for (int i = 0; i < totalCubes; i++)
        {
            Box boxComponent = randomCubes[i].GetComponent<Box>();
            if (boxComponent != null)
            {
                boxComponent.ResetToOriginalColor();
            }
        }
    }

    void Start()
    {
        GameManager.Instance.Cubes = this;
        GameManager.Instance.Timer_.UpdateBoxesLeftText(totalCubes - destroyedCubes);
    }

    public void NotifyCubeDestroyed()
    {
        destroyedCubes++;
        GameManager.Instance.Timer_.UpdateBoxesLeftText(totalCubes - destroyedCubes);
        if (destroyedCubes == totalCubes)
        {
            if (GameManager.Instance.Timer_ != null) // Notify the Timer all cubes are destroyed
            {
                GameManager.Instance.Timer_.StopTimer();
                if (GameManager.Instance.GameScene != Scene.GameScene3)
                {
                    GameManager.Instance.EndSceneDialog.showDialog();
                }
            }
            reset();
            GameManager.Instance.Timer_.UpdateBoxesLeftText(totalCubes - destroyedCubes);
            GameManager.Instance.HandleAllCubesDestroyed(GameManager.Instance.Timer_.GetStoppedDuration());
        }
    }

    public void reset()
    {
        destroyedCubes = 0;
    }
}

