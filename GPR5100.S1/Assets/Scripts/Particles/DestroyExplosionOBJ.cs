using UnityEngine;
using Photon.Pun;

public class DestroyExplosionOBJ : MonoBehaviour
{

    private void Update()
    {
        if (!GetComponent<ParticleSystem>().isPlaying)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}
