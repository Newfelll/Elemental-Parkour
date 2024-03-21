using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IceBlockBehave : MonoBehaviour
{
    public ParticleSystem explosion;
    private AudioSource iceBlockSFX;
    public float timeToDestroy = 0.5f;
    public float timeToPlaySFX = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        iceBlockSFX = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
   


    private void OnCollisionEnter(Collision collision)
    {   

        
        if (collision.gameObject.CompareTag("Fire"))
        {
            
            
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fire"))
        {

           
            StartCoroutine(DestroyIceBlock());
        }
    }



    IEnumerator DestroyIceBlock()
    {
       iceBlockSFX.Play();
       iceBlockSFX.time = timeToPlaySFX;

       explosion.Play();

       this.gameObject.GetComponent<MeshRenderer>().enabled = false;
       this.gameObject.GetComponent<BoxCollider>().enabled = false;

       yield return new WaitForSeconds(timeToDestroy);
        
       Destroy(this.gameObject);
    }

}
