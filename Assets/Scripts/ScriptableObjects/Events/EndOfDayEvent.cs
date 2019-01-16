using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/EndOfDayEvent")]
public class EndOfDayEvent : Event {

    public override void Instantiate()
    {
        GameManager.Instance.interfaceHandler.questionText.text = "What should " + GameManager.Instance.characterHandler.currentCharacter.characterName + " do overnight?";

        base.Instantiate();
    }

    public override void Execute(List<List<string>> dividedList, CharacterManager characterV)
    {
        if (dividedList[0].Count > 0 || dividedList[1].Count > 0 || dividedList[2].Count > 0)
        {
            if (dividedList[0].Count > dividedList[1].Count && dividedList[0].Count > dividedList[2].Count)
            {
                characterV.playerState = CharacterManager.PlayerState.Default;
                GameManager.Instance.pollHandler.chosenAnswer = dividedList[0][0];
            }
            else if(dividedList[1].Count > dividedList[2].Count)
            {
                characterV.playerState = CharacterManager.PlayerState.Plunder;
                GameManager.Instance.pollHandler.chosenAnswer = dividedList[1][0];
            }
            else if (dividedList[2].Count > dividedList[1].Count)
            {
                characterV.playerState = CharacterManager.PlayerState.ChopWood;
                GameManager.Instance.pollHandler.chosenAnswer = dividedList[2][0];
            }
        }
        else
        {
            characterV.playerState = CharacterManager.PlayerState.Default;
            GameManager.Instance.pollHandler.chosenAnswer = "nothing";
        } 
    }
}
