using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class EarthPlatformController : MonoBehaviour
{

    // private AudioSource movingSFX;
    public GameObject soundObj;


    [SerializeField] private  float moveSpeed = 5f;
    [SerializeField] private int pushDistance=15;
    [SerializeField] private int platformDistance = 3;
    [SerializeField] private bool isPulling;
    [SerializeField] private bool isPushing;
    public bool isMoving;

    private Vector3 targetPosition;
    public Rigidbody platformRb;

    private Vector3 targetDir;
   


    private void Start()
    {
        platformRb = GetComponent<Rigidbody>();
      
       
       
    }



    private void Update()
    {



        if (!isPulling && !isPushing)
        {
            platformRb.velocity = Vector3.zero;
           


        }

        if (isPulling || isPushing)
        {
            soundObj.SetActive(true);
        }
        else { soundObj.SetActive(false); }
               
        


    }

    private void FixedUpdate()
    {   
      /*  if (isPulling)
        {
            MoveToPosition();
        }

        if (isPushing)
        {
            MoveToPosition();
        }
      */
        MoveToPosition();
    }
    public void PullingPlatform(Vector3 newPosition)
    {
        
        isPushing = false;
        targetPosition = new Vector3(newPosition.x, transform.position.y, newPosition.z);
        targetDir = targetPosition-transform.position;
        
        targetDir=targetDir.normalized;
        
        isPulling = true;
        isMoving = true;
        
       
    }

    void MoveToPosition()
    {
        if ((Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)) && isMoving)
        {


            // transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
             platformRb.velocity = targetDir * moveSpeed;
            
           // isMoving = true;

        }

        else
        {
            
            isPulling = false;
            isPushing = false;
            isMoving = false;
           
        }

    }
    public void PushingPlatform(Vector3 direction)
    {
        isPulling = false;
        direction.y = 0;
        targetPosition = transform.position + direction.normalized * pushDistance;
        targetPosition.y = transform.position.y;
        targetDir = targetPosition-transform.position;
        targetDir=targetDir.normalized;
        isPushing = true;
        isMoving = true;
        

    }




   



}
