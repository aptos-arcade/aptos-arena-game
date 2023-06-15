using System;
using System.Collections;
using Aptos.Unity.Rest;
using AptosIntegration;
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
    public class PlayerEloRatingManager : MonoBehaviour
    {
        [SerializeField] private GameObject statsDisplay;
        [SerializeField] private TMP_Text eloText;
        [SerializeField] private TMP_Text winsText;
        [SerializeField] private TMP_Text lossesText;

        private bool loadSuccessful;

        private void OnEnable()
        {
            StartCoroutine(LoadPlayerStats());
        }

        private IEnumerator LoadPlayerStats()
        {
            return RestClient.Instance.View((vals, responseInfo) =>
            {
                if (vals == null || vals.Length < 3)
                {
                    statsDisplay.SetActive(false);
                    return;
                }
                eloText.text = vals[0];
                winsText.text = vals[1];
                lossesText.text = vals[2];
                statsDisplay.SetActive(true);
            }, Modules.AptosArenaViewPayload(
                "get_player_elo_rating",
                new[] { WalletManager.Instance.Address },
                Array.Empty<string>()
            ));
        }
    }
}