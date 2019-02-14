using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/SomebodyKnocksScenario")]
public class SomebodyKnocksScenario : Scenario {

	public override IEnumerator ExecuteScenario()
	{
		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);
		
		GameManager.Instance.audioManager.PlayEffect(audioClips[0]);
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Es klopft an die Tür."
		));
		
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Durch das Gucklock siehst du die Umrisse einer Person."
		));
		
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Die Person erkennt anscheinend, dass jemand auf der anderen Seite der Tür steht und fällt schreiend und bettelnd auf die Knie."
		));
		
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"'Lasst mich rein! Bitte! BITTE!'"
		));
		
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Durch ihre Kapuze lässt sich ihr Gesicht nur schwer erkennen und auch deine Fragen scheint sie nicht zu beantworten."
		));
		
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"'Mach bitte auf, ansonsten kriegen sie mich!'"
		));

		GameManager.Instance.interfaceHandler.storyPanel.SetActive(false);

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.pollHandler.DoPoll(scenarioEvents[0]));
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.AfterQuestion());

		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);
		
		if (GameManager.Instance.pollHandler.chosenAnswer == scenarioEvents[0].possibleAnswers[0]) //yes
		{
			int rnd = Random.Range(0, 10);

			if (rnd <= 4)
			{
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"'Vielen Dank, dass du mir aufgemacht hast! Seit heute morgen laufe ich vor den Banditen weg. Ich hoffe hier kriegen sie mich nicht.'"
				));

				Character tempCharacter = GameManager.Instance.characterHandler.RandomInactiveCharacter();
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					tempCharacter.characterName + " tritt deinem Haus bei."
				));
				
				GameManager.Instance.characterHandler.InstantiateNewCharacter(tempCharacter.characterName);

				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
						""
					));
			}
			else
			{
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"In dem Moment, wo du der fremden Person, die Tür öffnest, stürmen von allen Seiten Banditen in das Haus!"
				));

				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Sie rauben und plündern, was sie zwischen die Finger kriegen und so schnell wie sie in's Haus gerannt kamen, stürmen sie auch wieder raus."
				));
				
				int rnd2 = Random.Range(0, 2);

				if (rnd2 == 0)
				{
					CharacterManager toDieCharacter = GameManager.Instance.characterHandler.RandomActiveCharacter();
					
					yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
						"Sie klauen die Hälfte deiner Ressourcen, deine Items und als wenn das nicht schon schlimm genug wäre, erleidet " + toDieCharacter.characterName + " eine so schwere Kopfverletzung, die wenige Stunden später zum Tod führt."
					));
					
					GameManager.Instance.characterHandler.KillCharacter(toDieCharacter.characterName);
					
					GameManager.Instance.FoodValue -= GameManager.Instance.FoodValue / 2;
					GameManager.Instance.FirewoodValue -= GameManager.Instance.FirewoodValue / 2;
					GameManager.Instance.MedPackValue -= GameManager.Instance.MedPackValue / 2;
					
					GameManager.Instance.itemHandler.DeleteAllItems();

					yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
							""
						));
					
					foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
					{
						character.MoraleValue -= 3;
					}

					yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
							""
						));
				}
				else
				{
					yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
						"Sie klauen die Hälfte deiner Ressourcen und all deine Items. Wenigstens blieben die Bewohner weitesgehend verschont, jedoch war das ein harter Schlag für alle."
					));

					GameManager.Instance.FoodValue -= GameManager.Instance.FoodValue / 2;
					GameManager.Instance.FirewoodValue -= GameManager.Instance.FirewoodValue / 2;
					GameManager.Instance.MedPackValue -= GameManager.Instance.MedPackValue / 2;
					
					GameManager.Instance.itemHandler.DeleteAllItems();
					
					foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
					{
						character.MoraleValue -= 2;
					}
					
					yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
						""
					));
				}
			}
		}
		else
		{
			int rnd = Random.Range(0, 10);

			if (rnd <= 4)
			{
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Dein Ablehnen führt dazu, dass die Person auf der anderen Seite der Tür fluchend wegläuft."
				));
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"'Das wirst du bereuen! Pass auf, wenn du das nächste Mal rausgehst!'"
				));
				
				foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
				{
					character.MoraleValue -= 1;
				}
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					""
				));
			}
			else
			{
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Heulend bleibt die Person mit dem Kopf nach unten geneigt vor der Tür liegen als plötzlich ein Rudel Wölfe aus dem Geäst springt."
				));
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Nach einem kurzen Aufschrei, bleibt der regungslose Körper auf dem Boden liegen und wird von den Wölfen zurück hinter die nächsten Bäume gezerrt."
				));

				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
						"Das liegt schwer auf den Bewohnern des Hauses."
					));
				
				foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
				{
					character.MoraleValue -= 3;
				}

				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
						""
					));
			}
		}
        wasUsed = true;
    }    
}
