using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSelector : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        QueueSong();
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying) 
        {
            QueueSong();
        }
    }
    public void QueueSong() 
    {
        source.PlayOneShot(clips[Random.Range(0, clips.Length - 1)]);
    }
}
