using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    
    [SerializeField] public static float sensX = 80f;

    
    [SerializeField] public static float sensY = 80f;

    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;

    [SerializeField] private float mouseX;
    [SerializeField] private float mouseY;

    float multiplier = 0.01f;

    float xRotation;
    float yRotation;


    // Start is called before the first frame update
    void Start()
    {
      
        
            

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        if (!GameManager.gameOver)
        {
            if (!GameManager.gamePaused)
            {

                MouseInput();

                cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
                orientation.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            }
        }



    }


    void MouseInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    }
}