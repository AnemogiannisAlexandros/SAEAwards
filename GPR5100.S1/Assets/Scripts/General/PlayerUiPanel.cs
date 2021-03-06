﻿using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

/// <summary>
/// Main Scene UI Manager
/// Updates Graphics on the Canvas According to player Stats(Health,Icon)
/// </summary>
namespace MyMultiplayerProject
{
    public class PlayerUiPanel : MonoBehaviourPunCallbacks
    {
        public GameObject playerEntryUiPrefab;
        public Sprite defaultSpriteMask;
        private Dictionary<int, GameObject> playerListEntries;


        #region Unity
        public void Awake()
        {
            playerListEntries = new Dictionary<int, GameObject>();

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                GameObject entry = Instantiate(playerEntryUiPrefab);
                entry.transform.SetParent(gameObject.transform);
                entry.transform.localPosition = Vector3.one;
                entry.transform.localScale = new Vector3(0.5f,0.5f);
                entry.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.GetTexutre(p.GetPlayerNumber());
                entry.transform.GetChild(1).GetComponent<Text>().text = string.Format("{0}", p.GetScore());
                entry.transform.GetChild(2).GetComponent<Image>().sprite = defaultSpriteMask;
                playerListEntries.Add(p.ActorNumber, entry);
            }
        }
        #endregion

        #region PunCallBacks
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
            playerListEntries.Remove(otherPlayer.ActorNumber);
        }
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            GameObject entry;
            if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry)) 
            {
                entry.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.GetTexutre(targetPlayer.GetPlayerNumber());
                entry.transform.GetChild(1).GetComponent<Text>().text = string.Format("{0}", targetPlayer.GetScore());
                object item;
                if (targetPlayer.CustomProperties.TryGetValue(GameManager.PLAYER_CURRENT_ITEM, out item))
                {
                    if ((int)item >= 0)
                    {
                        entry.transform.GetChild(2).GetComponent<Image>().sprite = GameManager.GetItemTexture((int)item);
                    }
                    else 
                    {
                        entry.transform.GetChild(2).GetComponent<Image>().sprite = defaultSpriteMask;
                    }
                }
            }
        }
        #endregion
    }
}
