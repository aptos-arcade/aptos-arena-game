using ApiServices.Models;
using AptosIntegration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.RankedMenu
{
    public class AvailableCharacterButton : MonoBehaviour
    {

        [SerializeField] private TMP_Text buttonText;
        
        private Button button;
        private TokenData tokenData;

        public void InitializeButton(TokenData tokenDataParam)
        {
            tokenData = tokenDataParam;
            buttonText.text = tokenData.Name;
            GetComponent<Button>().onClick.AddListener(OnClick);
        }
        
        private void OnClick()
        {
            RankedTransactions.EquipCharacter(tokenData);
        }
    }
}