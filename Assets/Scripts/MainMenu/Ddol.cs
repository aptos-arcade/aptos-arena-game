using Aptos.Unity.Rest;
using UnityEngine;

namespace MainMenu
{
    public class Ddol : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            RestClient.Instance.SetEndPoint(Constants.MAINNET_BASE_URL);
        }
    }
}