using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuLogic : MonoBehaviour
{
    private int counter = 0;

    public GameObject[] objectsToActivate;
    public CanvasGroup toFadeCanvas;

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
                if (counter == 2)
                {
                    grayCanvas.SetActive(false);
                    paperCanvas.SetActive(true);
                }
                
                if (counter == 6)
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
}
