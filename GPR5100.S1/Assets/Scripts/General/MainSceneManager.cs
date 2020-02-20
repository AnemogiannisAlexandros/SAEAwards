using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using Cinemachine;
using TMPro;
using KartGame.KartSystems;

namespace MyMultiplayerProject
{
    public class MainSceneManager : MonoBehaviourPunCallbacks
    {
        public static MainSceneManager Instance = null;
        public Text InfoText;
        public GameObject[] ItemPrefabs;
        public Transform[] playerSpawnPoints;
        public Transform[] itemSpawnPoints;
        private List<GameObject> players = new List<GameObject>();

        public GameObject GetPlayer(int index) 
        {
            return players[index];
        }
        #region Unity
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else 
            {
                Destroy(this.gameObject);
            }
        }
        public override void OnEnable()
        {
            base.OnEnable();
            CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
            if (PhotonNetwork.IsMasterClient) 
            {
                PhotonNetwork.Instantiate("AudioManager", Vector3.zero, Quaternion.identity);

            }
        }
        public void Start()
        {
            InfoText.text = "Waiting for other players...";
            Hashtable props = new Hashtable
        {
            {GameManager.PLAYER_LOADED_LEVEL,true }
        };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        }
        public override void OnDisable()
        {
            base.OnDisable();

            CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
        }
        #endregion

        #region Coroutines
        public IEnumerator RespawnMysteryItem(Transform itemTaken) 
        {
            //if (PhotonNetwork.IsMasterClient)
            //{
                float timer = 0f;
                while (timer <= GameManager.MYSTERY_ITEMS_SPAWN_TIME)
                {
                    timer += Time.deltaTime;
                    //               Debug.Log(timer);
                    yield return null;
                }
                itemTaken.GetComponent<MeshRenderer>().enabled = true;
                itemTaken.GetComponent<Collider>().enabled = true;
            //}
        }
        //New Implementation
        private IEnumerator EndOfGame(string winner, int remainingLives)
        {
            float timer = 10.0f;
            while (timer > 0.0f)
            {
                InfoText.text = string.Format("Player {0} wins with {1} remaining Lives.", winner, remainingLives);

                yield return new WaitForEndOfFrame();

                timer -= Time.deltaTime;
            }
            PhotonNetwork.LoadLevel(2);
        }

        private IEnumerator ResetToLobby()
        {
            float timer = 5.0f;
            while (timer > 0.0f)
            {
                InfoText.text = string.Format("Returning to login screen in {0} seconds.", timer.ToString("n2"));

                yield return new WaitForEndOfFrame();

                timer -= Time.deltaTime;
            }
            PhotonNetwork.LeaveRoom();
        }
        #endregion

        #region PunCallbacks
        public override void OnDisconnected(DisconnectCause cause)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {

            CheckEndOfGame();
        }
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(GameManager.PLAYER_LIVES)) 
            {
                CheckEndOfGame();
                return;
            }

            if (PhotonNetwork.IsMasterClient) 
            {
                return;
            }
            if (changedProps.ContainsKey(GameManager.PLAYER_LOADED_LEVEL)) 
            {
                if (CheckAllPlayerLoadedLevel()) 
                {
                    Hashtable props = new Hashtable
                    {
                        {CountdownTimer.CountdownStartTime, (float) PhotonNetwork.Time }
                    };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                }
            }
        }
        #endregion

        private void StartGame() 
        {
            Hashtable props = new Hashtable
            {
                {GameManager.PLAYER_IS_DEAD,false }
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            GameObject go =  PhotonNetwork.Instantiate("Kart Variant", playerSpawnPoints[PhotonNetwork.LocalPlayer.GetPlayerNumber()].position, playerSpawnPoints[PhotonNetwork.LocalPlayer.GetPlayerNumber()].rotation, 0);
            players.Add(go);
            if (PhotonNetwork.LocalPlayer.IsLocal)
            {
                CinemachineVirtualCamera camera = go.transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
                camera.gameObject.SetActive(true);
                KartMovement movement = go.GetComponent<KartMovement>();
                GameObject trailsLeft = PhotonNetwork.Instantiate("DriftTrailVar", transform.position, Quaternion.identity);
                GameObject trailsRight = PhotonNetwork.Instantiate("DriftTrailVar", transform.position, Quaternion.identity);
                trailsLeft.transform.SetParent(go.transform);
                trailsRight.transform.SetParent(go.transform);
                trailsLeft.transform.localPosition = new Vector3(-0.4160004f, -0.49844f, -0.7085f);
                trailsRight.transform.localPosition = new Vector3(0.3809996f, -0.49844f, -0.7085f);
                trailsRight.transform.localRotation = Quaternion.Euler(-89.98f, 0, 0);
                trailsLeft.transform.localRotation = Quaternion.Euler(-89.98f, 0, 0);
                movement.OnDriftStarted.AddListener(trailsRight.GetComponent<ParticleSystem>().Play);
                movement.OnDriftStarted.AddListener(trailsLeft.GetComponent<ParticleSystem>().Play);
                movement.OnDriftStopped.AddListener(trailsRight.GetComponent<ParticleSystem>().Stop);
                movement.OnDriftStopped.AddListener(trailsLeft.GetComponent<ParticleSystem>().Stop);

                GameObject playerAudio = PhotonNetwork.Instantiate("KartAudio", transform.position, Quaternion.identity);
                playerAudio.transform.SetParent(go.transform);
                playerAudio.transform.localPosition = new Vector3(0, 0, 0);
                playerAudio.transform.GetChild(2).GetComponent<EngineAudio>().Init(movement);
                movement.OnDriftStarted.AddListener(playerAudio.transform.GetChild(1).GetComponent<AudioSource>().Play);
                movement.OnDriftStopped.AddListener(playerAudio.transform.GetChild(1).GetComponent<AudioSource>().Stop);
                movement.OnKartCollision.AddListener(playerAudio.transform.GetChild(0).GetComponent<AudioSource>().Play);
                // go.transform.Find("Player Canvas").GetChild(0).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.LocalPlayer.NickName;
            }
            if (PhotonNetwork.IsMasterClient) 
            {
                for (int i = 0; i < 4; i++)
                {
                    PhotonNetwork.InstantiateSceneObject("MysteryBox", itemSpawnPoints[i].position, Quaternion.identity);
                }
            } 
        }
        private bool CheckAllPlayerLoadedLevel() 
        {
            if (!PhotonNetwork.IsConnected)
            {
                return true;
            }
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object playerLoadedLevel;
                if (p.CustomProperties.TryGetValue(GameManager.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
                {
                    if ((bool)playerLoadedLevel)
                    {
                        continue;
                    }
                }
                return false;
            }
            return true;
        }
        private void CheckEndOfGame() 
        {
            foreach (Player p in PhotonNetwork.PlayerList) 
            {
                object lives;
                if (p.CustomProperties.TryGetValue(GameManager.PLAYER_LIVES, out lives)) 
                {
                    if ((int)lives <= 0)
                    {
                        p.SetCustomProperties(new Hashtable { { GameManager.PLAYER_IS_DEAD, true } });
                        break;
                    }
                }
            }
            int destroyedCounter=0;
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) 
            {
                object isDestroyed;
                PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue(GameManager.PLAYER_IS_DEAD, out isDestroyed);
                {
                    if ((bool)isDestroyed) 
                    {
                        destroyedCounter++;
                    }
                }
            }
            if (destroyedCounter>=PhotonNetwork.PlayerList.Length-1) 
            {
                if (PhotonNetwork.IsMasterClient) 
                {
                    StopAllCoroutines();
                }
                string winner = "";
                int remainingLives = 0;
                foreach (Player p in PhotonNetwork.PlayerList) 
                {
                    if (p.GetScore() > remainingLives) 
                    {
                        winner = p.NickName;
                        remainingLives = p.GetScore();
                    }
                }
               // StartCoroutine(EndOfGame(winner, remainingLives));
                StartCoroutine(ResetToLobby());

            }
        }
        public void Disconnect() 
        {
            PhotonNetwork.Disconnect();
        }
        private void OnCountdownTimerIsExpired() 
        {
            StartGame();
        }
    }
}
