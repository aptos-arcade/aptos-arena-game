using System;
using ApiServices.Models;
using Global;

namespace AptosIntegration
{
    public static class RankedTransactions
    {
        
        public static void CreateBrawler()
        {
            TransactionHandler.Instance.RequestTransaction(Modules.ScriptFunctionAddress("init_brawler"), 
                Array.Empty<string>(), Array.Empty<string>());
        }
        
        public static void EquipCharacter(TokenData tokenData)
        {
            TransactionHandler.Instance.RequestTransaction(Modules.ScriptFunctionAddress("equip_character"),
                new[] { tokenData.Creator, tokenData.Collection, tokenData.Name, tokenData.PropertyVersion.ToString() },
                Array.Empty<string>());
        }
    }
}