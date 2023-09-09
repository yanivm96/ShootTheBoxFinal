using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float mass = 1f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] Transform cameraTransform;



    private Vector2 look;
    private CharacterController controller;
    private Vector3 velocity;
    private bool canInteract;
    private Shotgun shotgun;
    private bool hasJumped = false; // Track if the player has jumped
    private PlayerAudioManager sound;
    private Vector3 initialStartPosition;

    private GameObject Crosshair;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        canInteract = false;
        GameManager.Instance.Player_ = this;
        DontDestroyOnLoad(gameObject);
        initialStartPosition = transform.position;
        Crosshair = GameObject.Find("Crosshair");
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void EnableInteraction(bool shouldIneract)
    {
        canInteract = shouldIneract;
        shotgun = GameObject.Find("Shotgun").GetComponent<Shotgun>();
        if (shouldIneract)
        {
            shotgun.EnableInteraction(true);
            Crosshair.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Crosshair.SetActive(false);
            shotgun.EnableInteraction(false);
        }
    }

    public PlayerAudioManager Sound
    {
        get { return sound; }
        set { sound = value; }
    }

    public void ResetToStartPosition()
    {
        transform.position = initialStartPosition;
    }

    private void UpdateMovement()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        var input = new Vector3();

        input += transform.forward * y;
        input += transform.right * x;
        input = Vector3.ClampMagnitude(input, 1f);

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y += jumpSpeed;
            hasJumped = true; // Set hasJumped to true
        }

        controller.Move((input * movementSpeed + velocity) * Time.deltaTime);
    }

    private void UpdateGravity()
    {
        var gravity = Physics.gravity * mass * Time.deltaTime;
        velocity.y = controller.isGrounded ? -1f : velocity.y + gravity.y;
    }

    private void UpdateLook()
    {
        look.x += Input.GetAxis("Mouse X");
        look.y += Input.GetAxis("Mouse Y");
        look.y = Mathf.Clamp(look.y, -89f, 89f);
        cameraTransform.localRotation = Quaternion.Euler(-look.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, look.x, 0);
    }

    private void UpdateSound()
    {
        Vector3 input = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );

        if (controller.isGrounded)
        {
            if (input != Vector3.zero)
            {
                sound.PlayRunning();
            }
            else
            {
                sound.StopRunning();
            }

            if (hasJumped)
            {
                sound.PlayJumping();
                hasJumped = false; // Reset hasJumped
            }
        }
        else
        {
            sound.StopRunning();
            if (hasJumped)
            {
                sound.PlayJumping();
                hasJumped = false; // Reset hasJumped
            }
        }
    }


    void Update()
    {
        if (canInteract)
        {
            UpdateMovement();
            UpdateGravity();
            UpdateSound();
            UpdateLook();
        }
    }
}
