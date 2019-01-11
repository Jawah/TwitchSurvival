using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Events/SomebodyKnocksEvent")]
public class SomebodyKnocksEvent : Event {

	public override void Instantiate()
	{
		GameManager.Instance.CountDownValue = eventLength;
		GameManager.Instance.interfaceHandler.questionText.text = "Should we let them in?";

		base.Instantiate();
	}

	public override void Execute(List<List<string>> dividedList)
	{
		if (dividedList[0].Count > 0 || dividedList[1].Count > 0)
		{
			if (dividedList[0].Count > dividedList[1].Count)
			{
				GameManager.Instance.pollHandler.chosenAnswer = dividedList[0][0];
				GameManager.Instance.characterHandler.InstantiateNewCharacter(GameManager.Instance.characterHandler.allCharacters[Random.Range(0, GameManager.Instance.characterHandler.allCharacters.Count)].characterName);
			}
			else if(dividedList[1].Count > dividedList[0].Count)
			{
				GameManager.Instance.pollHandler.chosenAnswer = dividedList[1][0];
			}
		}
		else
		{
			GameManager.Instance.pollHandler.chosenAnswer = "nothing";
		} 
	}
}
