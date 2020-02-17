using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class DestroyExplosionOBJ : MonoBehaviour
{

    private void Update()
    {
        if (!GetComponent<ParticleSystem>().isPlaying)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
