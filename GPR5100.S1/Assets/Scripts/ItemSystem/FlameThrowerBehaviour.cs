using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;

namespace MyMultiplayerProject
{

    public class FlameThrowerBehaviour : MonoBehaviour, IItemBehaviour
    {
        [SerializeField]
        private float flameTimer = 5;
        public Player Owner { get; private set; }
        private PhotonView photonView;
        private bool takeDamage = false;
        private float curTimer = 0;
        public float GetOffset()
        {
            //throw new System.NotImplementedException();
            return 0;
        }

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }

        public void InitializeItem(Player player, float offset, Vector3 originalDirection, float lag, PhotonMessageInfo info)
        {
            Owner = player;
            GameObject playerObject = info.photonView.gameObject;
            Debug.Log(playerObject.name);
            transform.parent = info.photonView.gameObject.transform;
            transform.localPosition = new Vector3(0,0,0.5f);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            StartCoroutine(UseFlameThrower());
        }

        public IEnumerator UseFlameThrower()
        {
            float curTimer = 0;
            while (curTimer <= flameTimer)
            {
                curTimer += Time.deltaTime;
                yield return null;
            }
            Destroy(this.gameObject);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (photonView.IsMine)
                {
                    takeDamage = true;
                    Debug.Log("CountDownStarted");
                }
            }
        }
        private void OnTriggerStay(Collider other)
        {
            Debug.Log("OnTriggerStayRunning");
            if (takeDamage && curTimer > 0.2f)
            {
                if (other.GetComponent<PhotonView>() != null)
                {
                    other.gameObject.GetComponent<PhotonView>().RPC("ApplyDamage", RpcTarget.All);
                    curTimer = 0;
                }
            }
            else
            {
                curTimer += Time.deltaTime;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (photonView.IsMine)
                {
                    curTimer = 0;
                    takeDamage = false;
                    Debug.Log("CountDownStopped");
                }
            }
        }
    }
}
