using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour {

    public List<Event> m_AllEvents = new List<Event>();

    [HideInInspector] public Event m_CurrentEvent;

    private IEnumerator EventRoutine()
    {



        yield return null;
    }
}
