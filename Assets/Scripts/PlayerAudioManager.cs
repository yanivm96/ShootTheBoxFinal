using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip jumping;
    [SerializeField] private AudioClip running;
    [SerializeField] private AudioClip shooting;


    private AudioSource audioSource;
    private bool isRunningSoundPlaying = false;

    private void Start()
    {
        GameManager.Instance.Player_.Sound = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRunning()
    {
        if (!isRunningSoundPlaying)
        {
            audioSource.Stop(); // Stop any other sound
            audioSource.clip = running;
            audioSource.loop = true;
            audioSource.Play();
            isRunningSoundPlaying = true;
        }
    }

    public void StopRunning()
    {
        if (isRunningSoundPlaying)
        {
            audioSource.loop = false;
            audioSource.Stop();
            isRunningSoundPlaying = false;
        }
    }

    public void PlayJumping()
    {
        audioSource.Stop(); // Stop any other sound
        audioSource.clip = jumping;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayShooting()
    {
        // Stop other sounds but allow running and jumping sounds to continue
        if (audioSource.clip != running && audioSource.clip != jumping)
        {
            audioSource.Stop();
        }

        audioSource.PlayOneShot(shooting); // Play the shooting sound once
    }
}
