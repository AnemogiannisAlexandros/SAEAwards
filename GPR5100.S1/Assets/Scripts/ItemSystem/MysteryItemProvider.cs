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

        private void Awake()
        {
            manager = FindObjectOfType<MainSceneManager>();
            itemId = Random.Range(0, manager.ItemPrefabs.Length);
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
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { GameManager.PLAYER_CURRENT_ITEM, itemId } });
        }
    }
}
