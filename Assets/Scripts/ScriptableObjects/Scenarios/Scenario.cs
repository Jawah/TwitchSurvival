using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Scenario : ScriptableObject
{    
    public List<ScriptableObject> executionList = new List<ScriptableObject>();
    
    public IEnumerator ExecuteScenario()
    {
        foreach (ScriptableObject so in executionList)
        {
            if (so.GetType().ToString() == "ChatText")
            {
                ChatText tempText = so as ChatText;
                yield return null;
            }
            else
            {
                Event tempEvent = so as Event;
                tempEvent.Execute();
            }
        }
    }
}
