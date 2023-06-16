using Aptos.Unity.Rest.Model;

namespace Global
{
    public class Modules
    {
        private const string ModuleAddress = "0x5d74b9dfc5e930db7fd9530675e06a0bb52800cb5bf7c038a6f886aa3c00381d";
        private static readonly string PlayerModule = $"{ModuleAddress}::brawler";
        private static readonly string AptosArenaModule = $"{ModuleAddress}::aptos_arena";
        
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
    }
}