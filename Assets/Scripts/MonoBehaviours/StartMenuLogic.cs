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

    public GameObject nextText;
    
    public GameObject grayCanvas;
    public GameObject paperCanvas;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && counter != 2)
        {
            counter++;
            
            if (counter == objectsToActivate.Length)
            {
                StartCoroutine(StartGameRoutine());
            }
            else
            {
                if (counter == 2)
                {
                    nextText.SetActive(false);
                }
                
                else if (counter == 4)
                {
                    grayCanvas.SetActive(false);
                    paperCanvas.SetActive(true);
                }
                
                else if (counter == 8)
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

        counter++;
        
        if (counter == 3)
        {
            nextText.SetActive(true);    
        }
        
        objectsToActivate[counter-1].SetActive(false);
        objectsToActivate[counter].SetActive(true);
    }
}
