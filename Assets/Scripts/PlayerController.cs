using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [Header("Movement")]
    public float speed = 0f;
    public float runSpeed = 10f;
    public float acceleration = 5f;
    public float deceleration = 10f;
    public float maxSpeed = 20f;
    public float jumpForce = 5f;
    public float GroundCheckDistance = 0.2f;

    [Header("References")]
    public Rigidbody rb;
    // public Animator anim;
    public Transform groundCheck;


    [Header("Turn Heading")]
    public Transform playerCamera;
    public float mouseSensitivity = 100f;
    public float minPitch = -90f;
    public float maxPitch = 90f;

    private float currentSpeed;
    private Vector3 movementInput;
    private float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // anim=GetComponent<Animator>();
        currentSpeed = speed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void TurnHeading()
    {   
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical look (camera only)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minPitch, maxPitch);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal look (player body)
        transform.Rotate(Vector3.up * mouseX);
    }
    public void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movementInput = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
        // anim.SetBool("isMoving", movementInput.magnitude > 0);

        // anim.SetFloat("MoveX", moveHorizontal);
        // anim.SetFloat("MoveZ", moveVertical);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = Mathf.Lerp(currentSpeed, runSpeed, acceleration * Time.deltaTime);
            // anim.SetBool("isRunning", true);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, speed, deceleration * Time.deltaTime);
            // anim.SetBool("isRunning", false);
        }
        Vector3 movement =transform.TransformDirection(movementInput) * currentSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            // anim.SetTrigger("Jump");
            // anim.SetFloat("MoveY", rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, GroundCheckDistance, LayerMask.GetMask("Ground"));
    }

    // Update is called once per frame
    void Update()
    {
        TurnHeading();
        Move();
        Jump();
    }
}
