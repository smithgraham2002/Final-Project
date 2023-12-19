using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    public float walkSpeed;
    public Transform orientation;
    float horizInput;
    float vertInput;
    Vector3 moveDir;
    Rigidbody rb;

    
    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readytoJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    public float startYScale;
   
   public MovementState state;
    public enum MovementState {
        walking,sprinting, air, crouching
    }
    
    [Header("Key Codes")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatisGround;
    bool grounded;
    public float groundDrag;
    
    [Header("Win/Loss Conditions")]
    public bool GotGuyed;
    public bool Goal;


    void StateHandler(){
      
        if (Input.GetKeyDown(crouchKey)){
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        if (grounded && Input.GetKey(sprintKey)){
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (grounded){
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else{
            state = MovementState.air;
        }

    }

    void Start(){
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readytoJump = true;
        startYScale = transform.localScale.y;
        GotGuyed = false;
    }
    void Update(){
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatisGround);
        MyInput();
        SpeedControl();
        StateHandler();
        if (grounded){
            rb.drag = groundDrag;
        }
        else{
            rb.drag = 0;
        }
        if (GotGuyed){
            Debug.Log("NOOOOOO");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Goal){
            Debug.Log("YESSS");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Projectile")){
            GotGuyed = true;
        }
        if (collision.gameObject.name == "Goal"){
            Goal = true;
        }
    }

    void FixedUpdate(){
        movePlayer();
    }

    // Update is called once per frame
    void MyInput()
    {
        horizInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(jumpKey) && readytoJump && grounded){
            readytoJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetKeyDown(crouchKey)){
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(crouchKey)){
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }
    void movePlayer(){
        moveDir = orientation.forward * vertInput + orientation.right * horizInput;
        if (grounded){
            rb.AddForce(moveDir * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded){
            rb.AddForce(moveDir * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    void SpeedControl(){
        Vector3 flatVel = new Vector3(rb.velocity.x,0f,rb.velocity.z);

        if (flatVel.magnitude > moveSpeed){
            Vector3 limVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limVel.x, rb.velocity.y, limVel.z);
        }
    }

    void Jump(){
        rb.velocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);
        rb.AddForce(transform.up*jumpForce, ForceMode.Impulse);
    }

    void ResetJump(){
        readytoJump = true;
    }
}
