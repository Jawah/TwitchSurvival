using UnityEngine;

public class PrototypeScript : MonoBehaviour
{

    public ScriptableObject so;

    void Start()
    {
        Debug.Log(so.GetType().ToString());
    }
    
	public void TimeScaleSetter(float value)
    {
        Time.timeScale = value;
    }
}
