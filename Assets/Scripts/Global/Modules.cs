using Aptos.Unity.Rest.Model;

namespace Global
{
    public static class Modules
    {
        private const string PackageAddress = "0xa063aa74aeb7aac297161df445de42d99e2e9ac0d560af9500b2db29f2b8c4d6";
        private static readonly string ScriptsModule = $"{PackageAddress}::scripts";
        private static readonly string PlayerModule = $"{PackageAddress}::brawler";
        private static readonly string AptosArenaModule = $"{PackageAddress}::aptos_arena";
        
        public static ViewRequest PlayerViewPayload(string functionName, string[] arguments, string[] typeArguments)
        {
            return new ViewRequest()
            {
                Function = $"{PlayerModule}::{functionName}",
                Arguments = arguments,
                TypeArguments = typeArguments
            };
        }
        
        public static ViewRequest AptosArenaViewPayload(string functionName, string[] arguments, string[] typeArguments)
        {
            return new ViewRequest()
            {
                Function = $"{AptosArenaModule}::{functionName}",
                Arguments = arguments,
                TypeArguments = typeArguments
            };
        }
        
        public static string ScriptFunctionAddress(string functionName)
        {
            return $"{ScriptsModule}::{functionName}";
        }
    }
}