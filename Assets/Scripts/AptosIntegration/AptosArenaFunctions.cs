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
            var bytes = StringToByteArray("c09622c20bdd49b2b83b7e05c264a62cfedeb45eaf5c629d0f0174917d801aefd000000000000000025d74b9dfc5e930db7fd9530675e06a0bb52800cb5bf7c038a6f886aa3c00381d07736372697074730f65717569705f636861726163746572000420c46dd298b89d38314b486b2182a6163c4c955dce3509bf30751c307f5ecc2f361514506f6e74656d205370616365205069726174657312115370616365205069726174652023393933080000000000000000000000000000000000000000000000006cad946400000000010020c02d96ae8b38b207e6463dc4f3bcfa21883388a96f2f5730ea6cd0253482e2174040319c393553745be015f986f7d6c4c159743e551bbfad5d0b4d0bc32ff33b5b16c3bfccbb652412340b22bf30f7597ba733cb4e495c6e62d7e7962988505907");
            Debug.Log(bytes[0]);
            Debug.Log(bytes[1]);
            Debug.Log(bytes.Length);
            StartCoroutine(RestClient.Instance.SubmitSignedTransactionBcs(
                SubmitCallback,
                bytes
            ));
        }

        private void SubmitCallback(Transaction transaction, ResponseInfo responseInfo)
        {
            Debug.Log(responseInfo.message);
        }

        private static byte[] StringToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }
    }
}
