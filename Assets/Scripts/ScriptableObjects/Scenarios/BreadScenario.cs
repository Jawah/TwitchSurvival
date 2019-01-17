using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/BreadScenario")]
public class BreadScenario : Scenario {

	public override IEnumerator ExecuteScenario()
	{
		CharacterManager activeCharacter = GameManager.Instance.characterHandler.RandomActiveCharacter();
		
		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Beim Sortieren der Rationen fällt " + activeCharacter.characterName + " etwas sonderbares auf."
		);
		GameManager.Instance.CountDownValue = 10;
		yield return new WaitForSeconds(10);

		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Hinter einem Schrank neben einigen leeren Konservendosen liegt ein angebrochenes Stück Brot."
		);
		GameManager.Instance.CountDownValue = 10;
		yield return new WaitForSeconds(10);

		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Es scheint noch frisch zu sein und da nicht allzu lange zu liegen."
		);
		GameManager.Instance.CountDownValue = 10;
		yield return new WaitForSeconds(10);

		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Im Haus sind jedoch ganz klare Regeln definiert. Essensrationen werden immer nur gemeinsam angebrochen und fair aufgeteilt."
		);
		GameManager.Instance.CountDownValue = 10;
		yield return new WaitForSeconds(10);
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Hmm... Jemand oder etwas scheint sich an den Rationen zu vergreifen..."
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
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					activeCharacter.characterName + " hält die Augen den ganzen Tag offen und auffällig ist, dass sich " + otherCharacter.characterName + " sehr merkwürdig und zurückhaltend benimmt."
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					activeCharacter.characterName + " konfrontiert " + otherCharacter.characterName + " mit dem angeknabberten Brot, worauf unter Tränen das 'Verbrechen' gestanden wird."
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"'Ich hatte solchen Hunger. Entschuldige. Ich werde es nicht wieder tun.'"
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Trotz allem zerrt die Aktion an der Gruppenmoral.\r\n'Ohne Vertrauen haben wir nichts!'"
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
				{
					character.MoraleValue -= 1.5f;
				}
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					""
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
			}
			else
			{
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Bei genauerem Hinsehen siehst du Essenskrümel, die eindeutig rechts hinter dem Schrank langführen."
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Du folgst dieser Spur bis du zu einem Haufen in der Ecke gestauten Kartons kommst. Viele kleine Fussspuren weisen darauf hin, dass dort etwas hinterliegt."
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Bei genauerem Hinschauen findet " + activeCharacter.characterName + " ein Loch, was in die Wand führt. Ein Rattenloch!"
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);

				Item tempItem = GameManager.Instance.itemHandler.RandomItem();
				GameManager.Instance.itemHandler.InstantiateNewItem(tempItem.itemName);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					activeCharacter.characterName + " beseitigt das Rattennest und findet " + tempItem.itemName
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
			}
		}
		else
		{
			int rnd = Random.Range(0, 9);

			if (rnd <= 4)
			{
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Obwohl du an das gute im Menschen glaubst, war dein Vertrauen wohl an falscher Stelle gesetzt."
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Rationen verschwinden weiterhin zwischendurch."
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
			}
			else
			{
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Deinem Gefühl zu vertrauen, scheint das richtige gewesen zu sein. Es gab keine weiteren Vorfälle."
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Vertrauen ist das wichtigste in den diesen Zeiten."
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				
				foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
				{
					character.MoraleValue += 1.5f;
				}
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Vertrauen ist das wichtigste in den diesen Zeiten."
				);
				GameManager.Instance.CountDownValue = 3;
				yield return new WaitForSeconds(3);
			}
		}
	}
}