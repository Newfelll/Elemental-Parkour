using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Gameplay")]
    static public bool gameOver = false;
    static public bool gamePaused = false;
    public GameObject crosshair;
    public TextMeshProUGUI timeScore;
    public static bool timeStarted = false;
    private float time = 0f;
    public Slider sensitivityX,sensivityY;


    [Header("Level Change")]
    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI bestTimeText;
    public GameObject finishMenu;
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        
        gameOver = false;
        if (PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name) == null)
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name, 300);
        }
        if (PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name) == 0)
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name, 300);
        }

        PlayerLook.sensX = PlayerPrefs.GetFloat("SensitivityX");
        PlayerLook.sensY = PlayerPrefs.GetFloat("SensitivityY");

        sensitivityX.value = PlayerLook.sensX;
        sensivityY.value = PlayerLook.sensY;

        Resume();
        gameOver = false;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {   
               

                if (!gamePaused)
                {
                    Pause();
                }
                else { Resume(); }
            }




            if (timeStarted)
            {
                time += Time.deltaTime;
            }


            timeScore.text = time.ToString("F2");
        }
    }



    public void FinishLevel()
    {
        crosshair.SetActive(false);
        finishMenu.active = true;
        currentTimeText.text = "Current Time: " + time.ToString("F2");
        if (time < PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name))
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name, time);
        }
        bestTimeText.text = "Best Time: " + PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name).ToString("F2");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;


    }


    public void OpenNextLevel(int levelID)
    {
        string levelName = "Level_" + (SceneManager.GetActiveScene().buildIndex + 1);


        SceneManager.LoadScene(levelName);
       
    }
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Resume()
    {   
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gamePaused = false;
        Time.timeScale = 1; pauseMenu.SetActive(false); crosshair.SetActive(true);

        SettingsData.SetSensitivity(sensitivityX.value, sensivityY.value);
        

    }
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gamePaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        crosshair.SetActive(false);
    }

}