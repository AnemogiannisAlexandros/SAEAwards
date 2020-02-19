using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

namespace MyMultiplayerProject
{
    public class PlayerUiPanel : MonoBehaviourPunCallbacks
    {
        public GameObject playerEntryUiPrefab;

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
                entry.transform.GetChild(1).GetComponent<Text>().text = string.Format("{0}", targetPlayer.GetScore());
                entry.transform.GetChild(2).GetComponent<Image>().sprite = GameManager.GetItemTexture(targetPlayer.GetPlayerNumber());
            }
        }
        #endregion
    }
}
