using System.Collections.Generic;
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
}
