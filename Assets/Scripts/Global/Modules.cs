using Aptos.Unity.Rest.Model;

namespace Global
{
    public class Modules
    {
        private const string ModuleAddress = "0x6dea44e79ce16ac82a77e0be74b26953295ccf04a5ff2dcac7738001581d8722";
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