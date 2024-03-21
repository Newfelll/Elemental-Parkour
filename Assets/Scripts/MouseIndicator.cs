using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseIndicator : MonoBehaviour
{
    public Sprite[] indicatorSprites;
    public Transform cam;
   
    public LayerMask layerMask;
    public float sphereRadius = 1f;
    public Image IndicatorU�;

    private string waterTag = "Water";
    private string earthTag = "Earth";
    private string fireTag = "Fire";
    private string iceTag = "Ice";

    void Start()
    {
        ;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, ElementRaycastController.maxInteractionDistance, layerMask))
           
        {

            if (hit.collider.tag == waterTag)
            {   

                IndicatorU�.sprite = indicatorSprites[1];
            }

            else if (hit.collider.tag == earthTag)
            {
                IndicatorU�.sprite = indicatorSprites[2];
            }

            else if (hit.collider.tag == fireTag)
            {
                IndicatorU�.sprite = indicatorSprites[1];
            }

            else if (hit.collider.tag == iceTag)
            {   
                
                IndicatorU�.sprite = indicatorSprites[3];
            }
             else  IndicatorU�.sprite = indicatorSprites[0];
            


        }

        else
        {
            IndicatorU�.sprite = indicatorSprites[0];
        }


        
    }
}
