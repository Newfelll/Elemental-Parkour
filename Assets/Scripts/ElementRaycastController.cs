using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.SceneManagement;

public class ElementRaycastController : MonoBehaviour
{
    public Transform cam;
    public static float maxInteractionDistance = 30f;
    public LayerMask layerMask;
    public LayerMask obstacleLayer;
    public float sphereRadius = 1f;
    public GameObject fireball;
    public Transform fireballSpawnPoint;
    public Transform fireballSpawnRotation;
    


    public bool isPulling;
    public bool isPushing;



    [Header("References")]
    private WaterSling waterSling;
    private EarthPlatformController earthPlatform;


    Scene scene;


    void Start()
    {
        waterSling = GetComponent<WaterSling>();
        scene = SceneManager.GetActiveScene();


    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameOver)
        {

            if (Input.GetMouseButton(0) && isPulling && earthPlatform != null)
            {
                earthPlatform.PullingPlatform(transform.position);

            }
            else isPulling = false;

            if (Input.GetMouseButton(1) && isPushing && earthPlatform != null)
            {
                earthPlatform.PushingPlatform(cam.forward);


            }
            else isPushing = false;
           

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;


                if (Physics.SphereCast(cam.position, sphereRadius, cam.forward, out hit, maxInteractionDistance, layerMask))
                
                {
                    if (!Physics.Linecast(cam.position, hit.point,obstacleLayer))
                    {
                        string hitTag = hit.collider.tag;


                        if (hitTag == "Water")
                        {

                            waterSling.SlingSpherecast(hit);
                        }
                        else if (hitTag == "Earth")
                        {

                            earthPlatform = hit.collider.GetComponent<EarthPlatformController>();
                            isPulling = true;

                        }
                        else if (hitTag == "Fire")
                        {
                            GameObject fireballInstance = Instantiate(fireball, fireballSpawnPoint.position, fireballSpawnRotation.rotation);
                            FireballBehaviour fireballBehaviour = fireballInstance.GetComponent<FireballBehaviour>();
                            if (fireballBehaviour != null)
                            {
                                fireballBehaviour.DirectionCalc(cam.forward);
                            }


                        }

                    }
                }

            }

            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;

                
                if (Physics.Raycast(cam.position, cam.forward, out hit, maxInteractionDistance, layerMask))
                {

                    if (!Physics.Linecast(cam.position, hit.point,obstacleLayer))
                    {
                        string hitTag = hit.collider.tag;

                        if (hitTag == "Ice")
                        {
                            WaterFreeze waterFreeze = hit.collider.GetComponent<WaterFreeze>();
                            if (waterFreeze != null)
                            {
                                waterFreeze.StartFreeze();
                            }
                        }
                        else if (hitTag == "Earth")
                        {

                            earthPlatform = hit.collider.GetComponent<EarthPlatformController>();
                            isPushing = true;


                        }
                        else if (hitTag == "Fire")
                        {

                        }

                    }
                }







            }
        }

    }
}
