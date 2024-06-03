using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseIndicator : MonoBehaviour
{
    public Sprite[] indicatorSprites;
    public Transform cam;
   
    public LayerMask layerMask;
    public LayerMask obstacleLayer;
    public float sphereRadius = 1f;
    public Image IndicatorUÝ;

    private string waterTag = "Water";
    private string earthTag = "Earth";
    private string fireTag = "Fire";
    private string iceTag = "Ice";

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, ElementRaycastController.maxInteractionDistance, layerMask))
           
        {
            if (!Physics.Linecast(cam.position, hit.point, obstacleLayer))
            {
                if (hit.collider.tag == waterTag)
                {

                    IndicatorUÝ.sprite = indicatorSprites[1];
                }

                else if (hit.collider.tag == earthTag)
                {
                    IndicatorUÝ.sprite = indicatorSprites[2];
                }

                else if (hit.collider.tag == fireTag)
                {
                    IndicatorUÝ.sprite = indicatorSprites[1];
                }

                else if (hit.collider.tag == iceTag)
                {

                    IndicatorUÝ.sprite = indicatorSprites[3];
                }
                else IndicatorUÝ.sprite = indicatorSprites[0];


            }
            else
            {
                IndicatorUÝ.sprite = indicatorSprites[0];
            }
        }
        else
        {
            IndicatorUÝ.sprite = indicatorSprites[0];
        }
        

        
    }
}
