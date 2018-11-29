using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager {

    //public bool m_WaitingForPoll = false;

    private List<Event> m_EventsToCall = new List<Event>();

    private void Update()
    {
        
    }

    private IEnumerator StartPoll()
    {
        

        yield return new WaitForSeconds(2f);
    }


}
