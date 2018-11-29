using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/FirewoodEvent")]
public class FirewoodEvent : Event {

    public override void Execute()
    {
        Debug.Log("lol");
    }
}
