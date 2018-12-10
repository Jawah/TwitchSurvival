using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/MedPackEvent")]
public class MedPackEvent : Event {
    
    public override void Instantiate()
    {
        GameManager.Instance.m_CountDownValue = m_EventLength;
        GameManager.Instance.m_QuestionText.text = "Should " + GameManager.Instance.currentCharacter.m_CharacterName + " get a MedPack?";

        base.Instantiate();
    }

    public override void Execute(List<List<string>> dividedList, CharacterManager characterV)
    {
        if (dividedList[0].Count > dividedList[1].Count)
        {
            //MED PACK BEHAVIOUR
        }
    }
}
