using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ChatText : ScriptableObject {

    [TextArea]
    public string text;

    void Execute()
    {
        //GameManager.Instance.interfaceHandler.
    }
}
