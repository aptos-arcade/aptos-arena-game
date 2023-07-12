using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;

namespace AptosIntegration
{
    public class TransactionHandler: MonoBehaviour
    {
        
        public static TransactionHandler Instance;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            } else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        [DllImport("__Internal")]
        private static extern void OnTransactionRequest(string function, string args, string typeArgs);
        
        public delegate void TransactionRequestAction(string function, string[] args, string[] typeArgs);
        public static event TransactionRequestAction OnTransactionRequestEvent;
        
        public delegate void TransactionResultAction(bool success);
        public static event TransactionResultAction OnTransactionResult;

        public void RequestTransaction(string function, string[] args, string[] typeArgs)
        {
            OnTransactionRequestEvent?.Invoke(function, args, typeArgs);
            #if UNITY_WEBGL == true && UNITY_EDITOR == false        
                OnTransactionRequest(function, StringArrayToString(args), StringArrayToString(typeArgs));
            #else
                StartCoroutine(SendResultCoroutine(0));
            #endif
        }

        private IEnumerator SendResultCoroutine(int result)
        {
            yield return new WaitForSeconds(2f);
            SendTransactionResult(result);
        }

        public void SendTransactionResult(int success)
        {
            Debug.Log("Sending transaction result: " + success);
            OnTransactionResult?.Invoke(success > 0);
        }
        
        // write a helper function to turn an array of strings into a comma-separated single string
        // this is needed because we can't pass arrays of strings to JS
        public static string StringArrayToString(string[] stringArray)
        {
            var stringArrayString = "";
            for (var i = 0; i < stringArray.Length; i++)
            {
                stringArrayString += stringArray[i];
                if (i < stringArray.Length - 1)
                {
                    stringArrayString += ",";
                }
            }
            return stringArrayString;
        }
    }
}