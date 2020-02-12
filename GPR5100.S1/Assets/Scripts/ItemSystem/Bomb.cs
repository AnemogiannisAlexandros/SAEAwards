using UnityEngine;

using Random = UnityEngine.Random;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

public class Bomb : MonoBehaviour
{

    private bool isDestroyed;
    private PhotonView photonView;
    private new Rigidbody rigidbody;

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody>();
        
    }
    public void Update()
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
        if (collision.gameObject.CompareTag("Wall"))
        {
            DestroyItemGlobaly();
        }
        else if (collision.gameObject.CompareTag("Player")) 
        {
            if (photonView.IsMine) 
            {
                collision.gameObject.GetComponent<PhotonView>().RPC("ApplyDamage", RpcTarget.All);
                DestroyItemGlobaly();
            }
        }
    }

    private void DestroyItemGlobaly() 
    {
        isDestroyed = true;
        PhotonNetwork.Destroy(gameObject);
    }
    private void DestroyItemLocaly() 
    {
        isDestroyed = true;
        GetComponent<Renderer>().enabled = false;
    }
}
