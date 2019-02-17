using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/StormyEvent")]
public class StormyEvent : Event {

	public override void Instantiate()
	{
		GameManager.Instance.interfaceHandler.questionText.text = "Soll jemand auf das Dach gehen und das Loch stopfen?";

		base.Instantiate();
	}
	
	// Update is called once per frame
	public override void Execute(List<List<string>> dividedList)
	{
		if (dividedList[0].Count > 0 || dividedList[1].Count > 0)
		{
			if (dividedList[0].Count > dividedList[1].Count)
			{
				GameManager.Instance.pollHandler.chosenAnswer = dividedList[0][0]; 
			}
			else if(dividedList[1].Count > dividedList[0].Count)
			{
				GameManager.Instance.pollHandler.chosenAnswer = dividedList[1][0];
			}
			else
			{
				GameManager.Instance.pollHandler.chosenAnswer = "Gleichstand";
			}
		}
		else
		{
			GameManager.Instance.pollHandler.chosenAnswer = "Keine Antworten";
		} 
	}
}
