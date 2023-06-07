using System;
using System.Collections;
using Aptos.Unity.Rest;
using Characters;
using Global;
using Photon;
using Photon.Pun;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class CustomPlayerManager : MonoBehaviour
    {

        [SerializeField] private GameObject characterRig;
        [SerializeField] private Button continueToRoomSelectButton;
        [SerializeField] private TMP_Text messageText;
        
        [SerializeField] private GameObject itemsPanel;
        [SerializeField] private Image characterImage;
        [SerializeField] private Image swordImage;
        [SerializeField] private Image gunImage;
        
        [SerializeField] private CharacterImages characterImages;
        [SerializeField] private SwordImages swordImages;
        [SerializeField] private GunImages gunImages;
        
        private GameObject curCharacterRig;
        
        private bool loadSuccessful;

        private void OnEnable()
        {
            messageText.gameObject.SetActive(true);
            messageText.text = "Loading...";
            itemsPanel.SetActive(false);
            StartCoroutine(LoadCoroutine());
        }
        
        private void OnDisable()
        {
            if(curCharacterRig != null) Destroy(curCharacterRig);
        }

        private IEnumerator LoadCoroutine()
        {
            yield return StartCoroutine(GetPlayerMeleeWeaponCoroutine());
            yield return StartCoroutine(GetPlayerCharacterCoroutine());
            yield return StartCoroutine(GetPlayerRangedWeaponCoroutine());
            if (loadSuccessful)
            {
                curCharacterRig = Instantiate(characterRig, new Vector3(-0.66f, -4.18f, 0), Quaternion.identity);
                curCharacterRig.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                
                messageText.gameObject.SetActive(false);
                
                itemsPanel.SetActive(true);
                
                var characterIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties[PlayerPropertyKeys.CharacterKey];
                characterImage.sprite = characterImages.GetCharacterSprite(characterIndex);

                var swordIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties[PlayerPropertyKeys.SwordKey];
                swordImage.sprite = swordImages.GetSwordImage(swordIndex);
                
                var gunIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties[PlayerPropertyKeys.GunKey];
                gunImage.sprite = gunImages.GetGunImage(gunIndex);
            }
            else
            {
                messageText.text = "No player found.";
            }
            
        }
        
        private static IEnumerator GetPlayerMeleeWeaponCoroutine()
        {
            return RestClient.Instance.View((vals, responseInfo) =>
            {
                int swordIndex;
                if (vals == null || vals.Length < 2 || vals[1] == "0")
                {
                    swordIndex = 1;
                }
                else
                {
                    swordIndex = int.Parse(vals[1]);
                }
                var playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;
                playerProperties[PlayerPropertyKeys.SwordKey] = swordIndex - 1;
                PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            }, Modules.ViewPayload(
                "get_player_melee_weapon",
                new[] { WalletManager.Instance.Address },
                Array.Empty<string>()
            ));
        }
        
        private static IEnumerator GetPlayerRangedWeaponCoroutine()
        {
            return RestClient.Instance.View((vals, responseInfo) =>
            {
                int rangedIndex;
                if (vals == null || vals.Length < 2 || vals[1] == "0")
                {
                    rangedIndex = 1;
                }
                else
                {
                    rangedIndex = int.Parse(vals[1]);
                }
                var playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;
                playerProperties[PlayerPropertyKeys.GunKey] = rangedIndex - 1;
                PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            }, Modules.ViewPayload(
                "get_player_ranged_weapon",
                new[] { WalletManager.Instance.Address },
                Array.Empty<string>()
            ));
        }
        
        private IEnumerator GetPlayerCharacterCoroutine()
        {
            return RestClient.Instance.View((vals, responseInfo) =>
            {
                if (vals == null || vals.Length < 3 || vals[1] == string.Empty) return;
                var characterCollectionName = vals[1];
                var playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;
                playerProperties[PlayerPropertyKeys.CharacterKey] = Characters.Characters.GetCharacterEnum(characterCollectionName);
                PhotonNetwork.SetPlayerCustomProperties(playerProperties);
                continueToRoomSelectButton.interactable = true;
                loadSuccessful = true;
            }, Modules.ViewPayload(
                "get_player_character",
                new[] { WalletManager.Instance.Address },
                Array.Empty<string>()
            ));
        }
    }
}