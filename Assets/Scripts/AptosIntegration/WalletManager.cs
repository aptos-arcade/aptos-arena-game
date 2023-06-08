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
