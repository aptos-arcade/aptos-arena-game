using ApiServices.Models.Fetch;
using AptosIntegration;
using UnityEngine;

namespace MainMenu.RankedMenu
{
    public class AvailableCharactersModal : MonoBehaviour
    {
        [SerializeField] private ModalManager modalManager;
        
        [SerializeField] private GameObject availableCharactersDisplay;
        [SerializeField] private GameObject noCharactersText;

        [SerializeField] private Transform availableCharactersTransform;

        [SerializeField] private AvailableCharacterButton availableCharacterButtonPrefab;

        private void OnEnable()
        {
            StartCoroutine(
                ApiServices.FetchServices.FetchOwnedCharacters(AddAvailableCharacters, WalletManager.Instance.Address));
            TransactionHandler.OnTransactionRequestEvent += OnTransactionRequest;
            TransactionHandler.OnTransactionResult += OnTransactionResult;
        }

        private void OnDisable()
        {
            TransactionHandler.OnTransactionRequestEvent -= OnTransactionRequest;
            TransactionHandler.OnTransactionResult -= OnTransactionResult;
        }

        private void AddAvailableCharacters(OwnedTokens ownedTokens)
        {
            foreach (Transform child in availableCharactersTransform)
            {
                Destroy(child.gameObject);
            }
            
            Debug.Log(ownedTokens.tokens.Length);

            foreach (var token in ownedTokens.tokens)
            {
                var availableCharacterButton =
                    Instantiate(availableCharacterButtonPrefab, availableCharactersTransform);
                availableCharacterButton.InitializeButton(token);
            }
            
            noCharactersText.SetActive(ownedTokens.tokens.Length == 0);
            availableCharactersDisplay.SetActive(ownedTokens.tokens.Length != 0);
        }

        private void OnTransactionRequest(string function, string[] args, string[] typeArgs)
        {
            modalManager.ShowLoading(true);
        }
        
        private void OnTransactionResult(bool success)
        {
            modalManager.CloseModals();
            if (!success)
            {
                modalManager.OpenAvailableCharactersModal();
            }
        }
    }
}