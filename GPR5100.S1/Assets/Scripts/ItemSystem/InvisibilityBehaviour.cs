using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;

namespace MyMultiplayerProject
{

    public class InvisibilityBehaviour : MonoBehaviour, IItemBehaviour
    {
        [SerializeField]
        private float invisTimer = 5;
        private List<Renderer> rends;
        public Player Owner { get; private set; }

        public float GetOffset()
        {
            //throw new System.NotImplementedException();
            return 0;
        }

        public void InitializeItem(Player player, float offset, Vector3 originalDirection, float lag,PhotonMessageInfo info)
        {
            Owner = player;
            rends = new List<Renderer>();
            GameObject playerObject = info.photonView.gameObject;
            Debug.Log(playerObject.name);
            foreach (Renderer r in playerObject.GetComponentsInChildren<Renderer>())
            {
                rends.Add(r);
                r.enabled = false;
            }
            StartCoroutine(InvisEnded());
        }

        public IEnumerator InvisEnded()
        {
            float curTimer = 0;
            while (curTimer <= invisTimer)
            {
                curTimer += Time.deltaTime;
                yield return null;
            }
            foreach (Renderer r in rends)
            {
                r.enabled = true;
            }
            Destroy(this.gameObject);
        }
    }
}
