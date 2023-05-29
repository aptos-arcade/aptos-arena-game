using Characters;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using static Photon.PlayerPropertyKeys;

namespace Gameplay
{
    public class SpawnManager : MonoBehaviour
    {

        [SerializeField] private Button spawnButton;
        [SerializeField] private Camera sceneCamera;
    
        private string characterPrefabName;

        private void Start()
        {
            characterPrefabName = Characters.Characters.AvailableCharacters[(CharactersEnum)PhotonNetwork.LocalPlayer
                .CustomProperties[CharacterKey]].PrefabName;
            spawnButton.onClick.AddListener(SpawnPlayer);
        }
    
        private void SpawnPlayer()
        {
            gameObject.SetActive(false);
            sceneCamera.gameObject.SetActive(false);
            PhotonNetwork.Instantiate(characterPrefabName, MatchManager.Instance.SpawnPosition, Quaternion.identity);
        }
    
        public void ShowSpawnPanel()
        {
            gameObject.SetActive(true);
            sceneCamera.gameObject.SetActive(true);
        }
    }
}
