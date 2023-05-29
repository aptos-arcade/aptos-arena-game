using System;
using Photon.Pun;
using UnityEngine;

namespace Weapons
{
    public class Weapon: MonoBehaviourPun
    {
        
        private AudioSource audioSource;
        
        [SerializeField] private AudioClip audioClip;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound()
        {
            audioSource.Stop();
            audioSource.PlayOneShot(audioClip);
        }
        
        [PunRPC]
        public void Equip()
        {
            gameObject.SetActive(true);
        }

        [PunRPC]
        public void UnEquip()
        {
            gameObject.SetActive(false);
        }
    }
}
