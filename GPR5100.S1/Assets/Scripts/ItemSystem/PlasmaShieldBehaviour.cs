﻿using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;

public class PlasmaShieldBehaviour : MonoBehaviour, IItemBehaviour
{ 

    private float offset;
    [SerializeField]
    private GameObject explosion;
    private bool isDestroyed;
    public Player Owner { get; private set; }

    private PhotonView photonView;


    public float GetOffset()
    {
        return offset;
    }
    public void Awake()
    {
        offset = 4;
        photonView = GetComponent<PhotonView>();
    }
    public void InitializeItem(Player player, float offset, Vector3 originalDirection, float lag, PhotonMessageInfo info)
    {
        Owner = player;

        transform.parent = info.photonView.transform;
        transform.localPosition = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDestroyed)
        {
            return;
        }
        if (other.gameObject.CompareTag("Shootable"))
        {
            Destroy(other.gameObject);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(explosion.name, transform.position, Quaternion.identity);
            }
            DestroyItemLocaly();
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(explosion.name, other.gameObject.transform.position, Quaternion.identity);
            }
            if (photonView.IsMine)
            {
                other.gameObject.GetComponent<PhotonView>().RPC("ApplyDamage", RpcTarget.All);
                DestroyItemLocaly();
            }
        }
    }
    private void DestroyItemLocaly()
    {
        isDestroyed = true;
        Destroy(gameObject);
    }
}