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
    public Animator anim;
    public Transform groundCheck;

    private float currentSpeed;
    private Vector3 movementInput;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim=GetComponent<Animator>();
        currentSpeed = speed;
    }

    public void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movementInput = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
        anim.SetBool("isMoving", movementInput.magnitude > 0);

        anim.SetFloat("MoveX", moveHorizontal);
        anim.SetFloat("MoveZ", moveVertical);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = Mathf.Lerp(currentSpeed, runSpeed, acceleration * Time.deltaTime);
            anim.SetBool("isRunning", true);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, speed, deceleration * Time.deltaTime);
            anim.SetBool("isRunning", false);
        }
        Vector3 movement =transform.TransformDirection(movementInput) * currentSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
            anim.SetFloat("MoveY", rb.velocity.y);
        }
        print(rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, GroundCheckDistance, LayerMask.GetMask("Ground"));
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }
}
