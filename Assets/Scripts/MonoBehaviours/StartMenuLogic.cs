using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StartMenuLogic : MonoBehaviour
{
    private int counter = 0;

    public GameObject[] objectsToActivate;
    public CanvasGroup toFadeCanvas;

    public TMP_InputField inputField;
    public ToggleGroup toggleGroup;
    
    public GameObject grayCanvas;
    public GameObject paperCanvas;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            counter++;
            
            if (counter == objectsToActivate.Length)
            {
                StartCoroutine(StartGameRoutine());
            }
            else
            {
                if (counter == 3)
                {
                    grayCanvas.SetActive(false);
                    paperCanvas.SetActive(true);
                }
                
                if (counter == 7)
                {
                    grayCanvas.SetActive(true);
                    paperCanvas.SetActive(false);
                }
                
                objectsToActivate[counter-1].SetActive(false);
                objectsToActivate[counter].SetActive(true);
            }
        }
    }

    IEnumerator StartGameRoutine()
    {
        
        while(toFadeCanvas.alpha < 1)
        {
            toFadeCanvas.alpha += Time.deltaTime / 2;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        
        SceneManager.LoadScene("Game");
    }

    public void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetString("ChannelName", inputField.text);

        int votingSpeed = int.Parse(toggleGroup.ActiveToggles().First().name);

        PlayerPrefs.SetInt("VotingSpeed", votingSpeed);
    }
}
