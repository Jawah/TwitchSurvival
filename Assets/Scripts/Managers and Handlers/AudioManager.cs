using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private AudioSource musicAudio;
	private AudioSource effectAudio;

	private void Start()
	{
		AudioSource[] audios = GetComponents<AudioSource>();
		musicAudio = audios[0];
		effectAudio = audios[1];
	}

	public void PlayEffect(AudioClip clip)
	{
		effectAudio.clip = clip;
		effectAudio.Play();
	}
}
