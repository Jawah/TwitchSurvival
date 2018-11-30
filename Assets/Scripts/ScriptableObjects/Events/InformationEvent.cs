using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/InformationEvent")]
public class InformationEvent : Event {

    public void Execute()
    {
        Debug.Log("lol");
    }
}
