using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/MedPackEvent")]
public class MedPackEvent : Event {
    
    public override void Instantiate()
    {
        GameManager.Instance.m_CountDownValue = m_EventLength;
        GameManager.Instance.m_QuestionText.text = "Should " + GameManager.Instance.m_CurrentCharacter.m_CharacterName + " get a MedPack?";

        base.Instantiate();
    }

    public override void Execute(List<List<string>> dividedList, CharacterManager characterV)
    {
        if (dividedList[0].Count > dividedList[1].Count)
        {
            characterV.healthState = CharacterManager.HealthState.Default;
            characterV.StatusChanger();
            GameManager.Instance.MedPackValue--;
            GameManager.Instance.m_MedPackText.text = GameManager.Instance.m_MedPackValue.ToString() + "x";
        }
    }
}
