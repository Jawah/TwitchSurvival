using UnityEngine;

public class DataResetter : MonoBehaviour
{
    public ResettableScriptableObject[] resettableScriptableObjects;
    
    private void Awake()
    {
        foreach (var scriptableObject in resettableScriptableObjects)
        {
            scriptableObject.Reset();
        }
    }
}