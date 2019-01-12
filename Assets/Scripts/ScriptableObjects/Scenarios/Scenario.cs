using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scenario : ScriptableObject
{    
    public List<Event> scenarioEvents = new List<Event>();
    
    public virtual IEnumerator ExecuteScenario(){ Debug.LogWarning("No override ExecuteScenario function declared"); yield return null; }
}
