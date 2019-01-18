using UnityEngine;

public class PrototypeScript : MonoBehaviour
{

    public ScriptableObject so;

    void Start()
    {
        
    }
    
	public void TimeScaleSetter(float value)
    {
        Time.timeScale = value;
    }
}
