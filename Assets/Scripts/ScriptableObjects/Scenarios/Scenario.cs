using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scenario : ResettableScriptableObject
{    
    public List<Event> scenarioEvents = new List<Event>();
    public List<AudioClip> audioClips = new List<AudioClip>();

    public bool wasUsed = false;

    public virtual IEnumerator ExecuteScenario(){ Debug.LogWarning("No override ExecuteScenario function declared"); yield return null; }

    public override void Reset()
    {
        wasUsed = false;
    }
}
