using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class WaterSling : MonoBehaviour
{
    
    private AudioSource waterSlingSFX;
    public Rigidbody rb;
    public LineRendererAnimation lr;

    public KeyCode slingKey;
    public float maxSlingDistance = 10f;
    private Vector3 slingDir;

    public float slingForce = 10f;
    private bool sling = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        waterSlingSFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void FixedUpdate()
    {
        if (sling)
        {
            sling = false;
            Sling(slingDir.normalized);
        }
    }




    public void SlingSpherecast(RaycastHit hit)
    {
        
        if(hit.collider.gameObject.GetComponent<WaterInteractableCoolDown>().isCoolingDown == false)
        {
            slingDir = hit.collider.gameObject.transform.position - transform.position;

            //Sling(slingDir.normalized);
            sling = true;
            lr.ThrowWaterHook(hit.collider.gameObject.transform.position);
            hit.collider.gameObject.GetComponent<WaterInteractableCoolDown>().CoolDown();
        }
        

    }

    void Sling(Vector3 dir)
    {   
        rb.AddForce(dir * slingForce, ForceMode.Impulse);
        PlayerMovement.canDash = true;
        waterSlingSFX.PlayOneShot(waterSlingSFX.clip);
      
    }
}
