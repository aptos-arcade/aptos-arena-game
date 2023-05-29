using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Weapons
{
    public class HitEffect : MonoBehaviour
    {

        [SerializeField] private float destroyTime;
        
        [SerializeField] private AudioSource audioSource;
    
        private void Start()
        {
            audioSource.Play();
            StartCoroutine(DestroyEffect());
        }

        private IEnumerator DestroyEffect()
        {
            yield return new WaitForSeconds(destroyTime);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
