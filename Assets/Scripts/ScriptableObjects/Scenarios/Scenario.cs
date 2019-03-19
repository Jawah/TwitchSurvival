using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scenario : ResettableScriptableObject
{    
    public List<Event> scenarioEvents = new List<Event>();
    public List<AudioClip> audioClips = new List<AudioClip>();
    public Sprite scenarioSprite;

    public bool wasUsed = false;

    public virtual IEnumerator ExecuteScenario()
    {
        if (scenarioSprite != null)
        {
            GameManager.Instance.interfaceHandler.scenarioImageHolder.sprite = scenarioSprite;
        }
        else
        {
            GameManager.Instance.interfaceHandler.scenarioImageHolder.sprite = GameManager.Instance.interfaceHandler.scenarioStandardSprite;
        }
        
        yield return null;
    }

    public override void Reset()
    {
        wasUsed = false;
    }
}
