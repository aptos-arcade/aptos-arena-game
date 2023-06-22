using System;
using System.Linq;
using Aptos.Unity.Rest;
using Aptos.Unity.Rest.Model;
using UnityEngine;

namespace AptosIntegration
{
    public class AptosArenaFunctions : MonoBehaviour
    {
        private void Start()
        {
            RestClient.Instance.SetEndPoint(Constants.MAINNET_BASE_URL);
            var bytes = StringToByteArray("c09622c20bdd49b2b83b7e05c264a62cfedeb45eaf5c629d0f0174917d801aefd000000000000000025d74b9dfc5e930db7fd9530675e06a0bb52800cb5bf7c038a6f886aa3c00381d07736372697074730f65717569705f6368617261637465720004207fef9d50cd1a2ee2068617b086b98ec434f1728d7cadcc7088c402df4585ce411110506f6e74656d204461726b20416765730f0e4461726b2041676573202331383008000000000000000000000000000000000000000000000000492e936400000000010020c02d96ae8b38b207e6463dc4f3bcfa21883388a96f2f5730ea6cd0253482e21740a164471debb486ee332cf972a6e5facf8cc2269df1e4bb9a9e103af0600964ea5d2e7ee6bd51efd9fb73437e67c27a4352186d19dede630385f93273ddfe1204");
            Debug.Log(bytes[0]);
            Debug.Log(bytes[1]);
            Debug.Log(bytes.Length);
            StartCoroutine(RestClient.Instance.SubmitSignedTransaction(
                SubmitCallback,
                bytes
            ));
        }

        private void SubmitCallback(Transaction transaction, ResponseInfo responseInfo)
        {
            Debug.Log(responseInfo.message);
        }
        
        public static byte[] StringToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }
    }
}
