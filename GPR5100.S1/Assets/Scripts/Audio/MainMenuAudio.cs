using UnityEngine;

/// <summary>
/// Holds data for the Lobby Button Audio Clips
/// Buttons Call these methods on Hover and Click Events to play audio
/// </summary>
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
