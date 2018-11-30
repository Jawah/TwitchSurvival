using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/EndOfDayEvent")]
public class EndOfDayEvent : Event {

    public void Execute()
    {
        Debug.Log("lol");
    }
}
