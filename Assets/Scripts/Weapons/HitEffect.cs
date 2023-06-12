using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Weapons
{
    public class HitEffect : MonoBehaviourPun
    {

        [SerializeField] private float destroyTime;
        
        [SerializeField] private AudioSource audioSource;
    
        private void Start()
        {
            audioSource.Play();
            if(!photonView.IsMine) return;
            StartCoroutine(DestroyEffect());
        }

        private IEnumerator DestroyEffect()
        {
            yield return new WaitForSeconds(destroyTime);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
