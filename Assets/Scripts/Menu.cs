using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Slider  SfxVolume, MusicVolume, MasterVolume;
    void Start()
    {
        SfxVolume.value = PlayerPrefs.GetFloat("SfxVolume");
        MusicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
        MasterVolume.value = PlayerPrefs.GetFloat("MasterVolume");
    }

   
  
}
