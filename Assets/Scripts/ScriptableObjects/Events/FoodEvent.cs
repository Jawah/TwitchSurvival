using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/FoodEvent")]
public class FoodEvent : Event {

	public override void Execute()
    {
        Debug.Log("lol");
    }
}
