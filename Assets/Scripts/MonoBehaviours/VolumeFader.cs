using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeFader : MonoBehaviour
{
    public AudioSource source; // Assign in editor/etc

    void FixedUpdate()
    {
        if (source.volume > 0.1f)
        {
            source.volume = source.volume - 0.002f;
        }
    }
}
