using System;
using System.Collections;
using System.Collections.Generic;
using Aptos.Accounts;
using Aptos.Unity.Rest;
using Aptos.Unity.Rest.Model;
using UnityEngine;

public class WalletManager : MonoBehaviour
{

    public static WalletManager Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    // private void Start()
    // {
    //     RestClient.Instance.SetEndPoint(Constants.MAINNET_BASE_URL);
    //     Debug.Log("AAAA");
    //     var viewRequest = new ViewRequest()
    //     {
    //         Function = "0x1::coin::balance",
    //         TypeArguments = new[] { Constants.APTOS_ASSET_TYPE },
    //         Arguments = new[] { "0xc09622c20bdd49b2b83b7e05c264a62cfedeb45eaf5c629d0f0174917d801aef" }
    //     };
    //     StartCoroutine(RestClient.Instance.View((res, resInfo) => {
    //         Debug.Log(res[0]);
    //         Debug.Log(resInfo);
    //     }, viewRequest));
    // }
    

    public string Address { get; private set; }
    public bool IsLoggedIn => Address != null;
    public string AddressEllipsized => Ellipsize(Address);
        
    private const string AccountAddressKey = "AccountAddressKey";
    
    public delegate void WalletConnectedAction();
    public static event WalletConnectedAction OnConnect;
    
    public void SetAccountAddress(string accountAddress)
    {
        Address = accountAddress;
        PlayerPrefs.SetString(AccountAddressKey, accountAddress);
        OnConnect?.Invoke();
    }

    private string Ellipsize(string str, int length = 6)
    {
        return str[..length] + "..." + str[^length..];
    }

}
