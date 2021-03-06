﻿using Photon.Realtime;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Bomb Behaviour and Initialization Data
/// </summary>
public class BombBehaviour : MonoBehaviour, IItemBehaviour
{
    [SerializeField]
    private float force;
    private float offset;
    [SerializeField]
    private GameObject explosion;

    public Player Owner { get; private set; }


    public float GetOffset() 
    {
        return offset;
    }
    public void Awake()
    {
        offset = 4;
    }
    public void InitializeItem(Player player, float offset ,Vector3 originalDirection, float lag, PhotonMessageInfo info)
    {
        Owner = player;

        transform.forward = originalDirection;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();
        col.enabled = true;
        rigidbody.velocity = originalDirection * force;
        transform.position += originalDirection * (offset + lag);
    }

    void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Player")||collision.gameObject.CompareTag("Wall"))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(explosion.name, collision.gameObject.transform.position, Quaternion.identity);
            }
        }
    }
}
