using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

public class TNT : MonoBehaviour
{
    private bool isDestroyed;
    private PhotonView photonView;
    private new Rigidbody rigidbody;

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        } 
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (isDestroyed)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
        {

            if (photonView.IsMine)
            {
                collision.gameObject.GetComponent<PhotonView>().RPC("ApplyDamage", RpcTarget.All);
                DestroyItemLocally();
            }
        }
    }

    private void DestroyItemGlobally()
    {
        isDestroyed = true;
        PhotonNetwork.Destroy(gameObject);
    }

    private void DestroyItemLocally()
    {
        isDestroyed = true;
        Destroy(gameObject);
    }
}
