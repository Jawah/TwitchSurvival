using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterHandler : MonoBehaviour {

    public GameObject characterCardPanel;
    public GameObject characterCardPrefab;
    public List<Character> allCharacters = new List<Character>();
    [HideInInspector]
    public List<CharacterManager> activeCharacters = new List<CharacterManager>();
    [HideInInspector]
    public CharacterManager currentCharacter;

    public void InstantiateNewCharacter(string characterName)
    {
        foreach (var character in allCharacters)
        {
            if (character.characterName == characterName && character.inUse == false && activeCharacters.Count < 6)
            {
                character.inUse = true;
                Character characterSO = character;

                CharacterManager tempCharacter = new CharacterManager(characterSO)
                {
                    Instance = Instantiate(characterCardPrefab, characterCardPanel.transform)
                };

                tempCharacter.Setup(activeCharacters.Count);
                activeCharacters.Add(tempCharacter);
            }
        }
    }

    public CharacterManager RandomActiveCharacter()
    {
        CharacterManager cm = activeCharacters[Random.Range(0, activeCharacters.Count)];
        return cm;
    }

    public Character RandomInactiveCharacter()
    {
        bool characterChosen = false;
        Character tempCharacter = null;
        
        while (!characterChosen)
        {
            tempCharacter = allCharacters[Random.Range(0, GameManager.Instance.characterHandler.allCharacters.Count)];
            if (tempCharacter.inUse == false)
            {
                characterChosen = true;
            }
        }

        return tempCharacter;
    }

    public void KillCharacter(string name)
    {
        foreach(CharacterManager character in activeCharacters.ToList())
        {
            if (character.characterName == name)
            {
                Destroy(character.Instance);
                GameManager.Instance.characterHandler.activeCharacters.Remove(character);
            }
        }
    }
}
