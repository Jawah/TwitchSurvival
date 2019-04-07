using UnityEngine;

public class PrototypeScript : MonoBehaviour
{

    public ScriptableObject so;

    void Start()
    {
        PrintPlayerPrefs();
    }

    void PrintPlayerPrefs()
    {
	    Debug.Log(PlayerPrefs.GetString("ChannelName"));
	    Debug.Log(PlayerPrefs.GetInt("VotingSpeed"));
    }
    
	public void TimeScaleSetter(float value)
    {
        Time.timeScale = value;
    }

	public void ProKillCharacter()
	{
		GameManager.Instance.characterHandler.KillCharacter(GameManager.Instance.characterHandler.RandomActiveCharacter().characterName);
	}

	public void ProAddCharacter()
	{
		GameManager.Instance.characterHandler.InstantiateNewCharacter(GameManager.Instance.characterHandler.RandomInactiveCharacter().characterName);
	}
}
