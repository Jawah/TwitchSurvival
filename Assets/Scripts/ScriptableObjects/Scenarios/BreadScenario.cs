using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/BreadScenario")]
public class BreadScenario : Scenario {

	public override IEnumerator ExecuteScenario()
	{
		CharacterManager activeCharacter = GameManager.Instance.characterHandler.RandomActiveCharacter();
		
		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);
		yield return new WaitForSeconds(2f);
		
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Beim Sortieren der Rationen fällt " + activeCharacter.characterName + " etwas sonderbares auf."
			));
		
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Hinter einem Schrank neben einigen leeren Konservendosen liegt ein angebrochenes Stück Brot."
		));

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Es scheint noch frisch zu sein und da nicht allzu lange zu liegen."
		));

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Im Haus sind jedoch ganz klare Regeln definiert. Essensrationen werden immer nur gemeinsam angebrochen und fair aufgeteilt."
		));
		
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Hmm... Jemand oder etwas scheint sich an den Rationen zu vergreifen..."
		));
		
		GameManager.Instance.interfaceHandler.storyPanel.SetActive(false);

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.pollHandler.DoPoll(scenarioEvents[0]));
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.AfterQuestion());

		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);

		if (GameManager.Instance.pollHandler.chosenAnswer == scenarioEvents[0].possibleAnswers[0]) //yes
		{
			int rnd = Random.Range(0, 10);

			if (rnd <= 4 && GameManager.Instance.characterHandler.activeCharacters.Count >= 2)
			{
				bool characterChosen = false;
				CharacterManager otherCharacter = null;

				while (!characterChosen)
				{
					otherCharacter = GameManager.Instance.characterHandler.RandomActiveCharacter();

					if (otherCharacter.characterName != activeCharacter.characterName)
					{
						characterChosen = true;
					}
				}
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					activeCharacter.characterName + " hält die Augen den ganzen Tag offen und auffällig ist, dass sich " + otherCharacter.characterName + " sehr merkwürdig und zurückhaltend benimmt."
				));
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					activeCharacter.characterName + " konfrontiert " + otherCharacter.characterName + " mit dem angeknabberten Brot, worauf unter Tränen das 'Verbrechen' gestanden wird."
				));
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"'Ich hatte solchen Hunger. Entschuldige. Ich werde es nicht wieder tun.'"
				));
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Trotz allem zerrt die Aktion an der Gruppenmoral.\r\n'Ohne Vertrauen haben wir nichts!'"
				));
				
				foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
				{
					character.MoraleValue -= 1.5f;
				}
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					""
				));
			}
			else
			{
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Bei genauerem Hinsehen siehst du Essenskrümel, die eindeutig rechts hinter dem Schrank langführen."
				));
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Du folgst dieser Spur bis du zu einem Haufen in der Ecke gestauten Kartons kommst. Viele kleine Fussspuren weisen darauf hin, dass dort etwas hinterliegt."
				));
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Bei genauerem Hinschauen findet " + activeCharacter.characterName + " ein Loch, was in die Wand führt. Ein Rattenloch!"
				));

				Item tempItem = GameManager.Instance.itemHandler.RandomItem();
				GameManager.Instance.itemHandler.InstantiateNewItem(tempItem.itemName);
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					activeCharacter.characterName + " beseitigt das Rattennest und findet " + tempItem.itemName
				));
			}
		}
		else
		{
			int rnd = Random.Range(0, 9);

			if (rnd <= 4)
			{
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Obwohl du an das gute im Menschen glaubst, war dein Vertrauen wohl an falscher Stelle gesetzt."
				));
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Rationen verschwinden weiterhin zwischendurch."
				));
			}
			else
			{
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Deinem Gefühl zu vertrauen, scheint das richtige gewesen zu sein. Es gab keine weiteren Vorfälle."
				));
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Vertrauen ist das wichtigste in den diesen Zeiten."
				));
				
				
				foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
				{
					character.MoraleValue += 1.5f;
				}
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Vertrauen ist das wichtigste in den diesen Zeiten."
				));
			}
		}
        wasUsed = true;
	}
}