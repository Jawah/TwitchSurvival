using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/MedPackEvent")]
public class MedPackEvent : Event {
    
    public override void Instantiate()
    {
        GameManager.Instance.CountDownValue = eventLength;
        GameManager.Instance.interfaceHandler.questionText.text = "Should " + GameManager.Instance.characterHandler.currentCharacter.characterName + " get a MedPack?";

        base.Instantiate();
    }

    public override void Execute(List<List<string>> dividedList, CharacterManager characterV)
    {
        if (dividedList[0].Count > dividedList[1].Count)
        {
            characterV.healthState = CharacterManager.HealthState.Default;
            characterV.StatusChanger();
            GameManager.Instance.MedPackValue--;
            GameManager.Instance.interfaceHandler.medPackText.text = GameManager.Instance.MedPackValue.ToString() + "x";
        }
    }
}
