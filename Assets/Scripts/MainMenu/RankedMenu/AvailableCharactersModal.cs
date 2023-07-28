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

        private void AddAvailableCharacters(TokenData[] tokens)
        {
            foreach (Transform child in availableCharactersTransform)
            {
                Destroy(child.gameObject);
            }

            foreach (var token in tokens)
            {
                var availableCharacterButton =
                    Instantiate(availableCharacterButtonPrefab, availableCharactersTransform);
                availableCharacterButton.InitializeButton(token);
            }
            
            noCharactersText.SetActive(tokens.Length == 0);
            availableCharactersDisplay.SetActive(tokens.Length != 0);
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