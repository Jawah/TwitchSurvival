using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/FoodEvent")]
public class FoodEvent : Event {

    public override void Instantiate()
    {
        GameManager.Instance.CountDownValue = m_EventLength;
        GameManager.Instance.m_InterfaceHandler.m_QuestionText.text = "Should " + GameManager.Instance.m_CharacterHandler.m_CurrentCharacter.m_CharacterName + " get something to eat?";

        base.Instantiate();
    }

    public override void Execute(List<List<string>> dividedList, CharacterManager characterV)
    {
        if (dividedList[0].Count > dividedList[1].Count)
        {
            characterV.AddFull();
            GameManager.Instance.FoodValue--;
            GameManager.Instance.m_InterfaceHandler.m_FoodText.text = GameManager.Instance.FoodValue.ToString() + "x";
        }
    }
}
