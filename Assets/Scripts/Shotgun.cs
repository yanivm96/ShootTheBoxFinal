using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private AudioClip shotgunSound;
    private AudioSource audioSource;
    private float bulletSpeed = 3000f;
    private float fireRate = 0.2f;
    private float nextFireTime = 0f;
    private bool canInteract;
    private int nextToShoot = 0;

    private GameObject[] bullets;
    private const int PoolSize = 5;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        bullets = new GameObject[PoolSize];
        for (int i = 0; i < PoolSize; i++)
        {
            bullets[i] = Instantiate(bulletPrefab, firePoint.transform.position, transform.rotation);
            bullets[i].SetActive(false);
            DontDestroyOnLoad(bullets[i].gameObject);
        }
    }

    private void Start()
    {
        canInteract = false;
    }

    private void Update()
    {
        if (canInteract && Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            bullets[nextToShoot].transform.position = firePoint.transform.position;
            bullets[nextToShoot].transform.rotation = firePoint.transform.rotation;
            Shoot();
        }
    }

    public void EnableInteraction(bool shouldInteract)
    {
        canInteract = shouldInteract;
    }

    private void Shoot()
    {
        bullets[nextToShoot].SetActive(true);
        bullets[nextToShoot].GetComponent<Rigidbody>().velocity = Vector3.zero;
        bullets[nextToShoot].GetComponent<Rigidbody>().AddForce(firePoint.transform.right * bulletSpeed);
        nextToShoot = (nextToShoot + 1) % PoolSize;
        PlayShotgunSound(); // Call this function to play the shotgun sound
    }

    private void PlayShotgunSound()
    {
        audioSource.clip = shotgunSound;
        audioSource.loop = false;
        audioSource.Play();
    }
}
