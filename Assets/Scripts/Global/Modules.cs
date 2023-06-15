using Aptos.Unity.Rest.Model;

namespace Global
{
    public class Modules
    {
        private const string ModuleAddress = "0xd71b4784f28cd0b0f6d629a0042b88e9e2faad13abc8e85389c48f9445745983";
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