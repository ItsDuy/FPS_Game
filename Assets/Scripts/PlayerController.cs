using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [Header("Movement")]
    public float speed = 0f;
    public float acceleration = 5f;
    public float deceleration = 10f;
    public float maxSpeed = 20f;

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
    }

    public void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movementInput = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        anim.SetFloat("MoveX", moveHorizontal);
        anim.SetFloat("MoveZ", moveVertical);
        Vector3 movement =transform.TransformDirection(movementInput) * speed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
    }
}
