using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenLevel(int LevelID)
    {
        string LevelName = "Level_" + LevelID;
        SceneManager.LoadScene(LevelName);
    }


    public void Exit()
    {
        Application.Quit();
    }
}
