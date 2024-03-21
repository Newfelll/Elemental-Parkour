using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsData : MonoBehaviour
{
    

    private static SettingsData instance;
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
    }




    public static void SetSensitivity(float x, float y)
    {
        PlayerPrefs.SetFloat("SensitivityX", x);
        PlayerPrefs.SetFloat("SensitivityY", y);

        PlayerLook.sensX = x;
        PlayerLook.sensY = y;
    }


    public static float GetSensitivityX()
    {
        return PlayerPrefs.GetFloat("SensitivityX");
    }

    public static float GetSensitivityY()
    {
        return PlayerPrefs.GetFloat("SensitivityY");
    }
}
