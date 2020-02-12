using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExplosionOBJ : MonoBehaviour
{

    private void Update()
    {
        if (!GetComponent<ParticleSystem>().isPlaying)
        {
           Destroy(this.gameObject);
        }
    }
}
