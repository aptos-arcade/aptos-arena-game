using System.Collections;
using ApiServices.Models.Fetch;
using AptosIntegration;
using Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.RankedMenu
{
    public class YourBrawlerManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text loadingText;
        
        [SerializeField] private GameObject characterRigPrefab;
        
        [SerializeField] private Button continueToRoomSelectButton;
        
        [Header("Managers")]
        [SerializeField] private BrawlerItemsManager brawlerItemsManager;
        [SerializeField] private PlayerEloRatingManager playerEloRatingManager;

        private GameObject curCharacterRig;
        
        private void OnEnable()
        {
            StartCoroutine(LoadCoroutine());
            TransactionHandler.OnTransactionResult += OnTransactionResult;
        }
        
        private void OnDisable()
        {
            TransactionHandler.OnTransactionResult -= OnTransactionResult;
            if (curCharacterRig != null) Destroy(curCharacterRig);
        }
        
        private void ShowLoading()
        {
            if(curCharacterRig != null) Destroy(curCharacterRig);
            brawlerItemsManager.ShowItemsPanel(false);
            playerEloRatingManager.ShowEloRating(false);
            continueToRoomSelectButton.gameObject.SetActive(false);
            Debug.Log("Loading...");
            loadingText.text = "Loading...";
            loadingText.gameObject.SetActive(true);
        }

        private IEnumerator LoadCoroutine()
        {
            ShowLoading();
            yield return StartCoroutine(
                ApiServices.FetchServices.FetchBrawlerData(HandleFetchBrawlerData, WalletManager.Instance.Address));
        }

        private void HandleFetchBrawlerData(BrawlerData brawlerData)
        {
            var playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;
            if (brawlerData == null) return;
            var hasCharacter = brawlerData.Character.Collection != string.Empty;
            playerProperties[PlayerPropertyKeys.CharacterKey] = hasCharacter
                ? Characters.Characters.GetCharacterEnum(brawlerData.Character.Collection)
                : null;
            playerProperties[PlayerPropertyKeys.SwordKey] = brawlerData.MeleeWeapon.Type - 1;
            playerProperties[PlayerPropertyKeys.GunKey] = brawlerData.RangedWeapon.Type - 1;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
            
            // remove loading text
            loadingText.gameObject.SetActive(false);
            
            // create character display
            curCharacterRig = Instantiate(characterRigPrefab, new Vector3(-0.66f, -4.18f, 0), Quaternion.identity);
            curCharacterRig.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            
            // update items and elo
            brawlerItemsManager.UpdateItemsDisplay();
            playerEloRatingManager.ShowEloRating(true);
            
            // show continue button
            continueToRoomSelectButton.gameObject.SetActive(true);
            continueToRoomSelectButton.interactable = hasCharacter;
        }

        private void OnTransactionResult(bool successful)
        {
            if (!successful) return;
            StartCoroutine(LoadCoroutine());
        }
    }
}