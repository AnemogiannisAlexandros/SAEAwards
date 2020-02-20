using System.Collections;

using UnityEngine;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using RPC = Photon.Pun.RPC;
using System.Collections.Generic;
using TMPro;

namespace MyMultiplayerProject
{
    /// <summary>
    /// All Implementation and RPCs of the carts
    /// </summary>
    public class KartManager : MonoBehaviour
    {
        private PhotonView photonView;

        [SerializeField]
        private MainSceneManager manager;
        private new Rigidbody rigidbody;
        private new Collider collider;
        private new List<Renderer> childRenderers = new List<Renderer>();
        private Animator anim;
        //Allow Input Controll of this Object
        private bool controllable = true;

        public bool IsControllable()
        {
            return controllable;
        }
        #region Unity
        public void Awake()
        {
            photonView = GetComponent<PhotonView>();
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
            anim = GetComponent<Animator>();
            manager = FindObjectOfType<MainSceneManager>();
        }

        public void Start()
        {
            //Setup starting data
            object item;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(GameManager.PLAYER_CURRENT_ITEM, out item))
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { GameManager.PLAYER_CURRENT_ITEM, -1 } });
            }
            PhotonNetwork.LocalPlayer.SetScore(9);
            photonView.RPC("UpdateNamesForAll", RpcTarget.AllViaServer);
            
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                childRenderers.Add(r);
                r.material.color = GameManager.GetColor(photonView.Owner.GetPlayerNumber());
            }
        }

        //private void Update()
        //{
        //    //Debug.Log(PhotonNetwork.LocalPlayer.GetScore());
        //    object item;
        //    if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(GameManager.PLAYER_CURRENT_ITEM, out item))
        //    {
        //        Debug.Log((int)item);
        //    }
        //}
        #endregion
        #region Coroutines
            //Wait some time after you got hit
            //Call the appropriate RPC when the time ends
        private IEnumerator WaitToRegainControll()
        {
            yield return new WaitForSeconds(GameManager.PLAYER_RESPAWN_TIME);

            photonView.RPC("RegainControll", RpcTarget.AllViaServer);
        }
        #endregion
        #region PunCallbacks

        //Update Names on All Clients 
        [RPC]
        public void UpdateNamesForAll(PhotonMessageInfo info)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                for (int j = 0; j < PhotonNetwork.PlayerList.Length; j++)
                {
                    if (players[i].GetComponent<PhotonView>().Owner.ActorNumber == PhotonNetwork.PlayerList[j].ActorNumber)
                    {
                        players[i].transform.Find("PlayerCanvas").GetChild(0).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[j].NickName;
                    }
                }
            }


        }

        //Deal Damage to the player that got hit
        [RPC]
        public void ApplyDamage()
        {

            collider.enabled = false;
            controllable = false;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            anim.SetTrigger("Hit");
            //foreach (Renderer r in childRenderers)
            //{
            //    r.enabled = true;
            //}


            if (photonView.IsMine)
            {
                object lives;
                if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(GameManager.PLAYER_LIVES, out lives))
                {
                    PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { GameManager.PLAYER_LIVES, ((int)lives <= 1) ? 0 : ((int)lives - 1) } });
                    PhotonNetwork.LocalPlayer.AddScore(-1);
                }
                if ((int)lives > 0)
                {
                    StartCoroutine("WaitToRegainControll");
                }
                else
                {
                    Debug.Log("Dead Bruh");
                }
            }
        }

        //Use Item of the player that sent the RPC with appropriate values
        [RPC]
        public void UseItem(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
        {
            object item;
            info.Sender.CustomProperties.TryGetValue(GameManager.PLAYER_CURRENT_ITEM, out item);

            if ((int)item > -1)
            {
                float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
                GameObject itemObj;

                itemObj = Instantiate(manager.ItemPrefabs[(int)item], position, Quaternion.identity) as GameObject;
                IItemBehaviour currentItemBehaviour = itemObj.GetComponent<IItemBehaviour>();
                currentItemBehaviour.InitializeItem(photonView.Owner, currentItemBehaviour.GetOffset(), (rotation * Vector3.forward), Mathf.Abs(lag), info);
                info.Sender.SetCustomProperties(new Hashtable { { GameManager.PLAYER_CURRENT_ITEM, -1 } });
            }
            else
            {
                Debug.LogError("No Item");
            }

        }

        //Enable colliders and renderers after the respawn timer has ended
        [RPC]
        public void RegainControll()
        {
            collider.enabled = true;
            foreach (Renderer r in childRenderers)
            {
                r.enabled = true;
            }

            controllable = true;

            //EngineTrail.SetActive(true);
            //Destruction.Stop();
        }
        #endregion
    }
}
