using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earthPlatformSound : MonoBehaviour
{   private AudioSource movingSFX;
    private bool isMoving;
  

    private void Awake()
    {
        movingSFX = GetComponent<AudioSource>();
       
    }

    private void OnEnable()
    {
        movingSFX.Play();
    }
    
    private void OnDisable()
    {  
       movingSFX.Stop();
    }
}
