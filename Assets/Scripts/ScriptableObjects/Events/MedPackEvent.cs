using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/MedPackEvent")]
public class MedPackEvent : Event {

    public void Execute()
    {
        Debug.Log("lol");
    }
}
