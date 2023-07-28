using Aptos.Unity.Rest;
using UnityEngine;

namespace MainMenu
{
    public class Ddol : MonoBehaviour
    {
        
        private static Ddol _instance;
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            RestClient.Instance.SetEndPoint(Constants.MAINNET_BASE_URL);
        }
    }
}