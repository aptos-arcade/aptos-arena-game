using System;
using Photon.Pun;
using UnityEngine;

namespace Weapons
{
    public class Weapon: MonoBehaviourPun
    {
        private AudioSource audioSource;
        
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip audioClip)
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
