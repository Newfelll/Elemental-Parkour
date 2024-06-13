using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class SettingsData : MonoBehaviour
{
    

    private static SettingsData instance;
    public AudioMixer sfxMixer;
    public int volumeMultiplier=30;

    private void Awake()
    {
        if(instance!=null)
        {
            Destroy(gameObject);
            
        }
        else
        {
            
            instance = this;

            DontDestroyOnLoad(gameObject);
        }

        


        if (PlayerPrefs.GetFloat("SensitivityX")==0||PlayerPrefs.GetFloat("SensitivityY")==0)
        {
            PlayerLook.sensY = 0.5f;
            PlayerLook.sensX = 0.5f;
        }
    }

    private void Start()
    {
        instance.sfxMixer.SetFloat("sfx", Mathf.Log10(GetSfxVolume()) * volumeMultiplier);
        Debug.Log(Mathf.Log10(GetSfxVolume()));
        instance.sfxMixer.SetFloat("music", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * volumeMultiplier);
        Debug.Log(Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")));
        instance.sfxMixer.SetFloat("master", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * volumeMultiplier);
        Debug.Log(Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")));
    }


    public static void SetSensitivityY(float y)
    {
        
        PlayerPrefs.SetFloat("SensitivityY", y);

        
        PlayerLook.sensY = y;
    }

    public static void SetSensitivityX(float x)
    {
        PlayerPrefs.SetFloat("SensitivityX", x);
        

        PlayerLook.sensX = x;
       
    }

    public static float GetSensitivityX()
    {
        return PlayerPrefs.GetFloat("SensitivityX");
    }

    public static float GetSensitivityY()
    {
        return PlayerPrefs.GetFloat("SensitivityY");
    }

    public static float GetSfxVolume()
    {
        return PlayerPrefs.GetFloat("SfxVolume");
    }



    public void SetSfxVolume(float volume)
    {
        PlayerPrefs.SetFloat("SfxVolume", volume);
        sfxMixer.SetFloat("sfx", Mathf.Log10(volume)*volumeMultiplier);
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        sfxMixer.SetFloat("music", Mathf.Log10(volume) * volumeMultiplier);
    }


    public void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat("MasterVolume", volume);
        sfxMixer.SetFloat("master", Mathf.Log10(volume) * volumeMultiplier);
    }
}
