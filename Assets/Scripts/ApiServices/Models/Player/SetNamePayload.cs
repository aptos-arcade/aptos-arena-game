using System;

namespace ApiServices.Models.Player
{
    [Serializable]
    public class SetNamePayload
    {
        public SetNamePayload(string name, string playerId)
        {
            this.name = name;
            this.playerId = playerId;
        }
        
        public string name;
        public string playerId;
    }
}