using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterInteractableCoolDown : MonoBehaviour
{   

    
    [SerializeField] private float coolDownTime = 2f;
    Color emissiveColor=new Color(0.109f,0.505f,0.749f);
    [SerializeField] float intensity = 4;
    public bool isCoolingDown = false;

    MeshRenderer waterInteractable;
    MaterialPropertyBlock mpb;
    public MaterialPropertyBlock Mpb
    {
        get
        {
            if (mpb == null)
            {
                mpb = new MaterialPropertyBlock();
            }
            return mpb;
        }
    }
    void Start()
    {
        waterInteractable = GetComponent<MeshRenderer>();
        Mpb.SetColor("_EmissionColor", emissiveColor * intensity);
        waterInteractable.SetPropertyBlock(Mpb);
    }

    // Update is called once per frame
  
    public void CoolDown()
    {
        StartCoroutine("CoolDownTimer");
    }

    IEnumerator CoolDownTimer()
    {   this.gameObject.tag = "Untagged";
        float time = 0;
        intensity = 0;
        Mpb.SetColor("_EmissionColor", emissiveColor * intensity );
        waterInteractable.SetPropertyBlock(Mpb);
        isCoolingDown = true;
        while (time < coolDownTime)
        {
            intensity += (2*Time.deltaTime); 
            Mpb.SetColor("_EmissionColor", emissiveColor * intensity);
            waterInteractable.SetPropertyBlock(Mpb);
            time += Time.deltaTime;
            yield return null;
        }
       this.gameObject.tag = "Water";
        isCoolingDown = false;
    }
}
