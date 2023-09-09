using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private float fadeSpeed = 3f; 
    private bool fadeOut = false;
    private Color originalColor;
    private AudioSource audioSource;

    public GameObject Cube
    {
        get { return cube; }
        set { cube = value; }
    }

    public bool IsFaded
    {
        get { return fadeOut; }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        originalColor = this.GetComponent<Renderer>().material.color;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (fadeOut)
        {
            Renderer renderer = this.GetComponent<Renderer>();
            Color objectColor = renderer.material.color;
            float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            renderer.material.color = objectColor;

            if (objectColor.a <= 0)
            {
                this.gameObject.SetActive(false);
                fadeOut = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsFaded)
        {
            if (collision.transform.name == "Bullet(Clone)")
            {
                fadeOut = true;
                GameManager.Instance.Cubes.NotifyCubeDestroyed();
                PlayAndFadeOutSound("Sounds/fadeout");
            }
        }
    }

    private void PlayAndFadeOutSound(string soundPath)
    {
        AudioClip clip = Resources.Load<AudioClip>(soundPath);
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
            StartCoroutine(FadeOut(audioSource, fadeSpeed));
        }
        else
        {
            Debug.LogError("Could not find audio clip at path: " + soundPath);
        }
    }

    private IEnumerator FadeOut(AudioSource audioSource, float fadeSpeed)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeSpeed;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public void ResetToOriginalColor()
    {
        this.GetComponent<Renderer>().material.color = originalColor;
        this.gameObject.SetActive(true);
        fadeOut = false;
    }
}
