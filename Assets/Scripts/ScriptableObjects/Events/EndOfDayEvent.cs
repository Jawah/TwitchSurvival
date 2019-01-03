using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/EndOfDayEvent")]
public class EndOfDayEvent : Event {

    public override void Instantiate()
    {
        GameManager.Instance.CountDownValue = eventLength;
        GameManager.Instance.interfaceHandler.questionText.text = "What should " + GameManager.Instance.characterHandler.currentCharacter.characterName + " do overnight?";

        base.Instantiate();
    }

    public override void Execute(List<List<string>> dividedList, CharacterManager characterV)
    {
        if (dividedList[0].Count > dividedList[1].Count && dividedList[0].Count > dividedList[2].Count)
        {
            characterV.playerState = CharacterManager.PlayerState.Default;
        }
        else if(dividedList[1].Count > dividedList[2].Count)
        {
            characterV.playerState = CharacterManager.PlayerState.Plunder;
        }
        else if (dividedList[2].Count > dividedList[1].Count)
        {
            characterV.playerState = CharacterManager.PlayerState.ChopWood;
        }
        else
        {
            characterV.playerState = CharacterManager.PlayerState.Default;
        }
    }
}
