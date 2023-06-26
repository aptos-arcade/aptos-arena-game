using System.Collections;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class KillTextManager : MonoBehaviour
    {
        
        [SerializeField] private TMP_Text killText;

        public void OnKill()
        {
            StartCoroutine(KillCoroutine());
        }

        private IEnumerator KillCoroutine()
        {
            killText.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            killText.gameObject.SetActive(false);
        }
    }
}
