using System;
using System.Collections;
using ApiServices.Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace ApiServices
{
    public static class ApiClient
    {
        private const string ProdURL = "https://www.brawl3r.com";
        private const string DevURL = "https://www.brawl3r.com";
        // private const string DevURL = "http://localhost:3000";


        public static string BaseUrl()
        {
            #if UNITY_EDITOR
                return $"{DevURL}/api";
            #else
                return $"{ProdURL}/api";
            #endif
        }
    }
}