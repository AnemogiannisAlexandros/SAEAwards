using UnityEngine;
using Photon.Pun;
/// <summary>
/// Bomb Attributes
/// </summary>
public class Bomb : MonoBehaviour
{

    private bool isDestroyed;
    private PhotonView photonView;

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    public void Update()
    {
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (isDestroyed) 
        {
            return;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            DestroyItemLocaly();
        }
        else if (collision.gameObject.CompareTag("Player")) 
        {
            if (photonView.IsMine) 
            {
                collision.gameObject.GetComponent<PhotonView>().RPC("ApplyDamage", RpcTarget.All);
            }
            DestroyItemLocaly();
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
        Destroy(gameObject);
    }
}
