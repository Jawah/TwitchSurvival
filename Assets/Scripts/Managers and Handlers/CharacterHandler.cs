using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandler : MonoBehaviour {

    public GameObject m_CharacterCardPanel;
    public GameObject m_CharacterCardPrefab;
    public List<Character> m_AllCharacters = new List<Character>();
    public List<CharacterManager> m_ActiveCharacters = new List<CharacterManager>();

    public CharacterManager m_CurrentCharacter;

    public void InstantiateNewCharacter(string characterName)
    {
        Character characterSO;

        for (int i = 0; i < m_AllCharacters.Count; i++)
        {
            if (m_AllCharacters[i].m_CharacterName == characterName && m_AllCharacters[i].m_InUse == false && m_ActiveCharacters.Count < 6)
            {
                m_AllCharacters[i].m_InUse = true;
                characterSO = m_AllCharacters[i];

                CharacterManager tempCharacter = new CharacterManager(characterSO);

                tempCharacter.m_Instance = Instantiate(m_CharacterCardPrefab, m_CharacterCardPanel.transform) as GameObject;
                tempCharacter.Setup(m_ActiveCharacters.Count);
                m_ActiveCharacters.Add(tempCharacter);
            }
        }
    }
}
