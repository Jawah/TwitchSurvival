using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/InformationEvent")]
public class InformationEvent : Event {

    [TextArea]
    public string m_DescriptionText;
    //public float m_EventLength;

    public override void Execute()
    {
        Debug.Log("lol");
    }
}
