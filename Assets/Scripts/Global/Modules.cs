using Aptos.Unity.Rest.Model;

namespace Global
{
    public class Modules
    {
        private const string ModuleAddress = "0xd0d068848da0d8b1eec5755c048142c8efecf8a2ac9e319d6f502358535f2590";
        private static readonly string PlayerModule = $"{ModuleAddress}::player";
        
        public static ViewRequest ViewPayload(string functionName, string[] arguments, string[] typeArguments)
        {
            return new ViewRequest()
            {
                Function = $"{PlayerModule}::{functionName}",
                Arguments = arguments,
                TypeArguments = typeArguments
            };
        }
    }
}