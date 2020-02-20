using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    AudioSource src;
    [SerializeField]
    private AudioClip[] buttonClips;

    private void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    public void PlayRandomClip() 
    {
        src.PlayOneShot(buttonClips[Random.Range(0, buttonClips.Length - 1)]);
    }

    public void PlayClip(AudioClip clip) 
    {
        src.PlayOneShot(clip);
    }
}
