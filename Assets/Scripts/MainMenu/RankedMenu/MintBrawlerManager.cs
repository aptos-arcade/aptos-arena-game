using System;
using AptosIntegration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.RankedMenu
{
    public class MintBrawlerManager : MonoBehaviour
    {
        [SerializeField] private Button mintButton;
        
        [SerializeField] private ModalManager modalManager;
        
        private void Start()
        {
            mintButton.onClick.AddListener(RankedTransactions.CreateBrawler);
        }

        private void OnEnable()
        {
            TransactionHandler.OnTransactionRequestEvent += OnTransactionRequest;
            TransactionHandler.OnTransactionResult += OnTransactionResult;
        }
        
        private void OnDisable()
        {
            TransactionHandler.OnTransactionRequestEvent -= OnTransactionRequest;
            TransactionHandler.OnTransactionResult -= OnTransactionResult;
        }

        private void OnTransactionRequest(string _, string[] __, string[] ___)
        {
            modalManager.ShowLoading(true);
        }
        
        private void OnTransactionResult(bool success)
        {
            modalManager.ShowLoading(false);
        }
    }
}