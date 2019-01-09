using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class SoDebug : ScriptableObject {

    private void OnEnable()
    {
        Debug.Log("Hallo");
    }
}
