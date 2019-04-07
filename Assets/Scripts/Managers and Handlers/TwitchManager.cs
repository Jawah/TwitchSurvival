using UnityEngine;
using TwitchChatter;

public class TwitchManager : MonoBehaviour {
    
    [SerializeField] private string _pollChannelName;

    private void Start()
    {
        if (TwitchChatClient.singleton != null)
        {
            TwitchChatClient.singleton.AddChatListener(OnChatMessage);
        }

        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("ChannelName")))
        {
            TwitchChatClient.singleton.JoinChannel(PlayerPrefs.GetString("ChannelName"));
        }
        else
        {
            Debug.LogWarning("No channel name entered for poll! Enter a channel name and restart the scene.", this);
        }
    }

    private void OnDestroy()
    {
        if (TwitchChatClient.singleton != null)
        {
            TwitchChatClient.singleton.RemoveChatListener(OnChatMessage);
        }
    }

    private void OnChatMessage(ref TwitchChatMessage msg)
    {
        Debug.Log(msg.chatMessagePlainText);
        
        if (GameManager.Instance.pollHandler.gatherVotes)
        {
            if (!GameManager.Instance.pollHandler.votersList.Contains(msg.userName))
            {
                bool isValidVote = false;

                for (int i = 0; i < GameManager.Instance.pollHandler.currentPossibleAnswers.Count; i++)
                {
                    if (msg.chatMessagePlainText.ToLower().Equals(GameManager.Instance.pollHandler.currentPossibleAnswers[i].ToLower()))
                    {
                        isValidVote = true;

                        GameManager.Instance.answerCount++;
                        GameManager.Instance.interfaceHandler.counterText.text = GameManager.Instance.answerCount.ToString();
                        
                        //Debug.Log(msg.chatMessagePlainText);

                        GameManager.Instance.pollHandler.pollAnswers.Add(msg.chatMessagePlainText.ToLower());
                    }
                }

                if (isValidVote)
                {
                    GameManager.Instance.pollHandler.votersList.Add(msg.userName);
                }
            }
        }
    }
}
