using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerScript : MonoBehaviour
{
    private bool paused = false;

    void Start()
    {
        SetFromPlayerPrefs();
    }

    void SetFromPlayerPrefs()
    {
        //GameManager.Instance.twitchManager.SetChannel(PlayerPrefs.GetString("ChannelName"));
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!paused)
            {
                Time.timeScale = 0;
                paused = true;
            }
            else
            {
                Time.timeScale = 1;
                paused = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
