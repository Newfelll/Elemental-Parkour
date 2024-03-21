using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.VFX;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.Burst.CompilerServices;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    Scene scene;

   

    [Header("References")]
    private Rigidbody rb;
    [SerializeField] Transform orientation;
    public Camera playerCam;
    public VisualEffect vfxSpeedLines;
    private AudioSource playerSFX;
    EarthPlatformController platform;

    [Header("Player Movement")]

    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundMoveSpeed = 6f;
    [SerializeField] private float moveMultiplier = 10f;
    [SerializeField] private float groundDrag = 6f;
    [SerializeField] private float airDrag = 2f;
    [SerializeField] private float airMoveMultiplier = 0.4f;
    [SerializeField] private float updraftForce = 10f;
    private Vector3 updraftVector;
    [SerializeField] private float iceDrag = 1f;
    [SerializeField] private float iceMoveSpeed = 2f;
    [SerializeField] private bool onIce = false;
    [SerializeField] private float onPlatformDrag = 0f;
    [SerializeField] private bool onPlatform = false;




    [Header("Jump")]
    [SerializeField] private float canDoubleJump = 1;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool jump=false;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] Transform groundCheck;
    
    public LayerMask groundMask;
    public LayerMask grounIceMask;

    [Header("Dash")]
    [SerializeField] private float dashForce = 10f;
    [SerializeField] public static bool canDash = true;
    [SerializeField] private float maxDashDistance = 5f;
    [SerializeField] private float dashCooldown = 1.5f;
    [SerializeField] private float lastDashTime = 0f;
    [SerializeField] private int dashFov = 90;
    [SerializeField] private int originalFov = 80;
    [SerializeField] private float fovTime = 0.25f;
    [SerializeField] Vector3 dashDirection;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] AudioClip dashSFX;
    


    [Header("VFX Variables")]
    [SerializeField] private float speedLineVelocity = 10;

    

    [Header("Vectors")]
    public Vector3 moveDir;
    Vector3 slopeMoveDir;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dashKey;

    [Header("Temporary UÝ")]

      


    private float horizontalMovement;
    private float verticalMovement;


   
    
    
    

    RaycastHit slopeHit;
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2f + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else { return false; }
        }
        return false;
    }

    // Use this for initialization
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        playerSFX= GetComponent<AudioSource>();
        rb = this.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        moveSpeed = groundMoveSpeed;

       

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameOver)
        {

            
            


            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!GameManager.gamePaused && !GameManager.gameOver)
                {
                    SceneManager.LoadScene(scene.name);
                }
                
            }



            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            onIce = Physics.CheckSphere(groundCheck.position, groundDistance, grounIceMask);


            PlayerInput();
            ControlDrag();

            if (isGrounded)
            {
                canDash = true;
                canDoubleJump = 1;
            }


            if (isGrounded && Input.GetKeyDown(jumpKey))
            {
                jump = true;


            }
            else if (Input.GetKeyDown(jumpKey) && !isGrounded && canDoubleJump == 1)
            {
                jump = true;
                canDoubleJump = 0;
            }


            if (Input.GetKeyDown(dashKey) && canDash && Time.time >= lastDashTime + dashCooldown)
            {

                StartCoroutine(Dash());
            }
            slopeMoveDir = Vector3.ProjectOnPlane(moveDir, slopeHit.normal);


            if (Mathf.Abs(rb.velocity.x) > speedLineVelocity || Mathf.Abs(rb.velocity.y) > speedLineVelocity || Mathf.Abs(rb.velocity.z) > speedLineVelocity|| onPlatform)
            {
                vfxSpeedLines.enabled = true;
            }
            else
            {
               
                    vfxSpeedLines.enabled = false;
                
                
            }
            
           

            if (rb.velocity.magnitude > 2)
            {
                GameManager.timeStarted = true;
            }

            
        }
        else rb.velocity = Vector3.zero;
    }
    void FixedUpdate()
    {
        MovePlayer();

        if (jump)
        {   jump = false;
            Jump();
        }

       
    }
   


    void PlayerInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDir = orientation.transform.forward * verticalMovement + orientation.transform.right * horizontalMovement;
    }

    void ControlDrag()
    {   

        if (onIce)
        {
            rb.drag = iceDrag;
            moveSpeed = iceMoveSpeed;

        }
        else if (isGrounded&&onPlatform&&platform.isMoving)
        {
            rb.drag = onPlatformDrag;
            moveSpeed = groundMoveSpeed;

        }
        else if (isGrounded)
        {
            rb.drag = groundDrag;
            moveSpeed = groundMoveSpeed;

        }
        else
        {
            rb.drag = airDrag;
            moveSpeed = groundMoveSpeed;
        }
        
    }

    void MovePlayer()
    {   if(onPlatform&&platform.isMoving)
        {
            rb.velocity = new Vector3(platform.platformRb.velocity.x, rb.velocity.y, platform.platformRb.velocity.z);
        }
        else if (isGrounded & OnSlope())
        {
            rb.AddForce(slopeMoveDir.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
           
        }
        else if (isGrounded)
        {   
            rb.AddForce(moveDir.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
            
        }
        else 
        {
            
            rb.AddForce(moveDir.normalized * moveSpeed * moveMultiplier * airMoveMultiplier, ForceMode.Acceleration);
            
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        

    }


  

    IEnumerator Dash()
    {
        canDash = false;
        lastDashTime = Time.time;
        float startTime = Time.time;
        DoFov(dashFov);
        playerSFX.PlayOneShot(dashSFX);
       
        if (moveDir == Vector3.zero)
        {
           dashDirection = orientation.transform.forward;
        }
        else
        {
            dashDirection = moveDir.normalized;

        }
      
        
        while (Time.time < startTime + dashDuration)
        {
            Vector3 dashVelocity = dashDirection* dashForce;
            dashVelocity.y = 0; 
            rb.velocity = dashVelocity;
            yield return null;
        }

      
        DoFov(originalFov);
       
        
    }
    
    void DoFov(float targetFov)
    {
        playerCam.DOFieldOfView(targetFov, fovTime);
    }



    private void OnCollisionEnter(Collision collision)
    {
        if(isGrounded && collision.gameObject.CompareTag("Earth"))
        {
            transform.parent = collision.transform.parent;
            platform = collision.gameObject.GetComponentInParent<EarthPlatformController>(); ;
            onPlatform = true;
            
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (isGrounded && collision.gameObject.CompareTag("Earth"))
        {
            
            platform = null;
            transform.parent = null;
            onPlatform = false;
        }
    }


    private void OnCollisionStay(Collision collision)
    {   
        if (isGrounded && collision.gameObject.CompareTag("Earth"))
        {   
            

         /*   if (platform.isMoving)
            {
                rb.velocity = new Vector3(platform.platformRb.velocity.x, rb.velocity.y, platform.platformRb.velocity.z);
                
            }else
            {
                rb.drag = groundDrag;
            }
         */
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
      /*  if (other.CompareTag("Earth"))
        {
             transform.parent = other.transform;
             onPlatform = true;
        }*/

        if (other.CompareTag("Finish"))
        {
            if (!GameManager.gameOver)
            {
                FindAnyObjectByType<GameManager>().FinishLevel();
                GameManager.gameOver = true;
            }
                
        }


       

    }
  

    private void OnTriggerExit(Collider other)
    {
       /* if (other.CompareTag("Earth"))
        {
            transform.parent = null;
            onPlatform = false;

        }*/

       
       

    }



    private void OnTriggerStay(Collider other)
    {   if(other.CompareTag("Updraft"))
        {
           

            updraftVector = other.gameObject.transform.up * updraftForce;
            
            Updraft();
        }

        /*if (other.CompareTag("Earth"))
        {

            if (GetComponentInParent<EarthPlatformController>().isMoving)
            {
                rb.velocity = new Vector3(GetComponentInParent<EarthPlatformController>().platformRb.velocity.x,rb.velocity.y, GetComponentInParent<EarthPlatformController>().platformRb.velocity.z) ;
            }
            


        }*/


    }


    void Updraft()
    {
        
        rb.AddForce(updraftVector, ForceMode.Acceleration);
    }

  
}
