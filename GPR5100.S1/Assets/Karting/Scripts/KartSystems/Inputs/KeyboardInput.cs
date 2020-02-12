using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using MyMultiplayerProject;

namespace KartGame.KartSystems
{
    /// <summary>
    /// A basic keyboard implementation of the IInput interface for all the input information a kart needs.
    /// </summary>
    public class KeyboardInput : MonoBehaviour, IInput
    {
        [SerializeField]
        private KeyCode forwardButton,leftButton,rightButton,backwardButton,hopButton,boostButton,fireButton,itemButton;
        private PhotonView photonView;
        private KartManager manager;
        public float Acceleration
        {
            get { return m_Acceleration; }
        }
        public float Steering
        {
            get { return m_Steering; }
        }
        public bool BoostPressed
        {
            get { return m_BoostPressed; }
        }
        public bool FirePressed
        {
            get { return m_FirePressed; }
        }
        public bool HopPressed
        {
            get { return m_HopPressed; }
        }
        public bool HopHeld
        {
            get { return m_HopHeld; }
        }

        float m_Acceleration;
        float m_Steering;
        bool m_HopPressed;
        bool m_HopHeld;
        bool m_BoostPressed;
        bool m_FirePressed;

        bool m_FixedUpdateHappened;
        public void Start()
        {
            photonView = GetComponent<PhotonView>();
            manager = GetComponent<KartManager>();
        }
        void Update ()
        {
            if (!photonView.IsMine) 
            {
                return;
            }
            UserInput();
        }
        void FixedUpdate ()
        {
            if (!photonView.IsMine)
            {
                return;
            }
            if (!manager.IsControllable()) 
            {
                return;
            }
            m_FixedUpdateHappened = true;
        }
        void UserInput() 
        {
            if (manager.IsControllable())
            {
                if (Input.GetKey(forwardButton))
                    m_Acceleration = 1f;
                else if (Input.GetKey(backwardButton))
                    m_Acceleration = -1f;
                else
                    m_Acceleration = 0f;

                if (Input.GetKey(leftButton) && !Input.GetKey(rightButton))
                    m_Steering = -1f;
                else if (!Input.GetKey(leftButton) && Input.GetKey(rightButton))
                    m_Steering = 1f;
                else
                    m_Steering = 0f;
                if (Input.GetKeyDown(itemButton))
                {
                    photonView.RPC("UseItem", RpcTarget.AllViaServer, transform.position, transform.rotation);
                }
                m_HopHeld = Input.GetKey(hopButton);

                if (m_FixedUpdateHappened)
                {
                    m_FixedUpdateHappened = false;

                    m_HopPressed = false;
                    m_BoostPressed = false;
                    m_FirePressed = false;
                }

                m_HopPressed |= Input.GetKeyDown(hopButton);
                m_BoostPressed |= Input.GetKeyDown(boostButton);
                m_FirePressed |= Input.GetKeyDown(fireButton);
            }
            else
            {
                m_Acceleration = 0;
                m_Steering = 0;
                if (m_FixedUpdateHappened)
                {
                    m_FixedUpdateHappened = false;

                    m_HopPressed = false;
                    m_BoostPressed = false;
                    m_FirePressed = false;
                }

                m_HopPressed = false;
                m_BoostPressed = false;
                m_FirePressed = false;
            }

            
        }
    }
}