using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/SomebodyKnocksScenario")]
public class SomebodyKnocksScenario : Scenario {

	public override IEnumerator ExecuteScenario()
	{
		//1
		
		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Es klopft an die Tür."
		);
		GameManager.Instance.CountDownValue = 10;
		GameManager.Instance.audioManager.PlayEffect(audioClips[0]);
		yield return new WaitForSeconds(10);
        
		//2
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Durch das Gucklock siehst du die Umrisse einer Person."
		);
		GameManager.Instance.CountDownValue = 10;
		yield return new WaitForSeconds(10);
        
		//3
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Die Person erkennt anscheinend, dass jemand auf der anderen Seite der Tür steht und fällt schreiend und bettelnd auf die Knie."
		);
		GameManager.Instance.CountDownValue = 10;
		yield return new WaitForSeconds(10);
		
		//4
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"'Lasst mich rein! Bitte! BITTE!'"
		);
		GameManager.Instance.CountDownValue = 10;
		yield return new WaitForSeconds(10);
		
		//5
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Durch ihre Kapuze lässt sich ihr Gesicht nur schwer erkennen und auch deine Fragen scheint sie nicht zu beantworten."
		);
		GameManager.Instance.CountDownValue = 10;
		yield return new WaitForSeconds(10);
		
		//6
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"'Mach bitte auf, ansonsten kriegen sie mich!'"
		);
		GameManager.Instance.CountDownValue = 10;
		yield return new WaitForSeconds(10);

		GameManager.Instance.interfaceHandler.storyPanel.SetActive(false);

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.pollHandler.DoPoll(scenarioEvents[0]));
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.AfterQuestion());

		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);
		
		if (GameManager.Instance.pollHandler.chosenAnswer == scenarioEvents[0].possibleAnswers[0]) //yes
		{
			int rnd = Random.Range(0, 10);

			if (rnd <= 4)
			{
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"'Vielen Dank, dass du mir aufgemacht hast! Seit heute morgen laufe ich vor den Banditen weg. Ich hoffe hier kriegen sie mich nicht.'"
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);

				Character tempCharacter = GameManager.Instance.characterHandler.RandomInactiveCharacter();
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					tempCharacter.characterName + " tritt deinem Haus bei."
				);
				
				GameManager.Instance.characterHandler.InstantiateNewCharacter(tempCharacter.characterName);
				
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
			}
			else
			{
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"In dem Moment, wo du der fremden Person, die Tür öffnest, stürmen von allen Seiten Banditen in das Haus!"
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);

				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Sie rauben und plündern, was sie zwischen die Finger kriegen und so schnell wie sie in's Haus gerannt kamen, stürmen sie auch wieder raus."
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);

				int rnd2 = Random.Range(0, 2);

				if (rnd2 == 0)
				{
					CharacterManager toDieCharacter = GameManager.Instance.characterHandler.RandomActiveCharacter();
					
					GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
						"Sie klauen die Hälfte deiner Ressourcen, deine Items und als wenn das nicht schon schlimm genug wäre, erleidet " + toDieCharacter.characterName + " eine so schwere Kopfverletzung, die wenige Stunden später zum Tod führt."
					);
					
					GameManager.Instance.characterHandler.KillCharacter(toDieCharacter.characterName);
					
					GameManager.Instance.FoodValue -= GameManager.Instance.FoodValue / 2;
					GameManager.Instance.FirewoodValue -= GameManager.Instance.FirewoodValue / 2;
					GameManager.Instance.MedPackValue -= GameManager.Instance.MedPackValue / 2;
					
					GameManager.Instance.itemHandler.DeleteAllItems();
					
					GameManager.Instance.CountDownValue = 15;
					yield return new WaitForSeconds(15);
					
					GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
						"'Was für eine Scheisse...'"
					);
					
					foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
					{
						character.MoraleValue -= 3;
					}
					
					GameManager.Instance.CountDownValue = 10;
					yield return new WaitForSeconds(10);
				}
				else
				{
					GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
						"Sie klauen die Hälfte deiner Ressourcen und all deine Items. Wenigstens blieben die Bewohner weitesgehend verschont, jedoch war das ein harter Schlag für alle."
					);

					GameManager.Instance.FoodValue -= GameManager.Instance.FoodValue / 2;
					GameManager.Instance.FirewoodValue -= GameManager.Instance.FirewoodValue / 2;
					GameManager.Instance.MedPackValue -= GameManager.Instance.MedPackValue / 2;
					
					GameManager.Instance.itemHandler.DeleteAllItems();
					
					foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
					{
						character.MoraleValue -= 2;
					}
					GameManager.Instance.CountDownValue = 3;
					yield return new WaitForSeconds(3);	
				}
			}
		}
		else
		{
			int rnd = Random.Range(0, 10);

			if (rnd <= 4)
			{
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Dein Ablehnen führt dazu, dass die Person auf der anderen Seite der Tür fluchend wegläuft."
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"'Das wirst du bereuen! Pass auf, wenn du das nächste Mal rausgehst!'"
				);
				GameManager.Instance.CountDownValue = 10;
				
				foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
				{
					character.MoraleValue -= 1;
				}
				
				yield return new WaitForSeconds(10);
			}
			else
			{
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Heulend bleibt die Person mit dem Kopf nach unten geneigt vor der Tür liegen als plötzlich ein Rudel Wölfe aus dem Geäst springt."
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Nach einem kurzen Aufschrei, bleibt der regungslose Körper auf dem Boden liegen und wird von den Wölfen zurück hinter die nächsten Bäume gezerrt."
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Das liegt schwer auf den Bewohnern des Hauses."
				);
				GameManager.Instance.CountDownValue = 10;
				
				foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
				{
					character.MoraleValue -= 3;
				}
				
				yield return new WaitForSeconds(10);
			}
		}
	}
}
