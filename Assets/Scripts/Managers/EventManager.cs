using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    //public bool m_WaitingForPoll = false;

    enum EventEnum {Food, MedPack};
    EventEnum m_EventEnum; 

    private List<Event> m_EventsToCall = new List<Event>();

    private void Update()
    {
        
    }

    private IEnumerator StartPoll()
    {
        

        yield return new WaitForSeconds(2f);
    }


}
