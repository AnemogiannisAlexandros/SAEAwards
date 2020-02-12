using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KartGame.KartSystems;
using MyMultiplayerProject;

public class AssignParticleListener : MonoBehaviour
{
    KartMovement movement;
    ParticleSystem system;

    public void InitializeData() 
    {
        movement = transform.root.GetComponent<KartMovement>();
        system = GetComponent<ParticleSystem>();
        Debug.Log(movement.name);
        movement.OnDriftStarted.AddListener(Play);
        movement.OnDriftStopped.AddListener(Stop);
    }
    private void Play() 
    {
        system.Play();
    }
    private void Stop() 
    {
        system.Stop();
    }
}
