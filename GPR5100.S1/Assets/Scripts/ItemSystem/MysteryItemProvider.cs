using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace MyMultiplayerProject
{
    public class MysteryItemProvider : MonoBehaviour
    {
        private int itemId;
        private MainSceneManager manager;
        private Collider col;
        private MeshRenderer rend;
        private void Awake()
        {
            manager = FindObjectOfType<MainSceneManager>();
            itemId = Random.Range(0, manager.ItemPrefabs.Length);
            col = GetComponent<Collider>();
            rend = GetComponent<MeshRenderer>();
        }

        public int GetItemId()
        {
            return itemId;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player")
            {
                return;
            }
            itemId = Random.Range(0, manager.ItemPrefabs.Length);
            other.GetComponent<PhotonView>().Owner.SetCustomProperties(new Hashtable { { GameManager.PLAYER_CURRENT_ITEM, itemId } });
            //PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { GameManager.PLAYER_CURRENT_ITEM, itemId } });
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            transform.GetChild(1).GetComponent<AudioSource>().Play();
            col.enabled = false;
            rend.enabled = false;
            MainSceneManager.Instance.StartCoroutine(MainSceneManager.Instance.RespawnMysteryItem(this.transform));
        }
    }
}
