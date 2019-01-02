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
        foreach (var character in m_AllCharacters)
        {
            if (character.m_CharacterName == characterName && character.m_InUse == false && m_ActiveCharacters.Count < 6)
            {
                character.m_InUse = true;
                Character characterSO = character;

                CharacterManager tempCharacter = new CharacterManager(characterSO)
                {
                    m_Instance = Instantiate(m_CharacterCardPrefab, m_CharacterCardPanel.transform)
                };

                tempCharacter.Setup(m_ActiveCharacters.Count);
                m_ActiveCharacters.Add(tempCharacter);
            }
        }
    }
}
