using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuLogic : MonoBehaviour
{
    private int counter = 0;

    public GameObject[] objectsToActivate;
    public CanvasGroup toFadeCanvas;
    
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
                objectsToActivate[counter-1].SetActive(false);
                objectsToActivate[counter].SetActive(true);
            }
        }
    }

    IEnumerator StartGameRoutine()
    {
        while(toFadeCanvas.alpha > 0)
        {
            toFadeCanvas.alpha -= Time.deltaTime / 2;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        
        SceneManager.LoadScene("Game");
    }
}
