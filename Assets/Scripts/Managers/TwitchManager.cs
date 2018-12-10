using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TwitchChatter;

public class TwitchManager : MonoBehaviour {

    // Name of the Twitch channel to join for the poll
    public string _pollChannelName;

    private void Start()
    {
        if (TwitchChatClient.singleton != null)
        {
            TwitchChatClient.singleton.AddChatListener(OnChatMessage);
        }

        if (!string.IsNullOrEmpty(_pollChannelName))
        {
            TwitchChatClient.singleton.JoinChannel(_pollChannelName);
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
        if (GameManager.Instance.m_GatherVotes)
        {
            if (!GameManager.Instance.m_VotersList.Contains(msg.userName))
            {
                bool isValidVote = false;

                for (int i = 0; i < GameManager.Instance.m_CurrentPossibleAnswers.Count; i++)
                {
                    if (msg.chatMessagePlainText.Equals(GameManager.Instance.m_CurrentPossibleAnswers[i]))
                    {
                        isValidVote = true;

                        Debug.Log(msg.chatMessagePlainText);

                        GameManager.Instance.m_PollAnswers.Add(msg.chatMessagePlainText);
                    }
                }

                if (isValidVote)
                {
                    //GameManager.Instance.m_VotersList.Add(msg.userName);
                }
            }
        }
    }
}
