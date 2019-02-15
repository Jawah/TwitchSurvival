using UnityEngine;

public class EmptyChildren : MonoBehaviour
{
    private void OnDisable()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
