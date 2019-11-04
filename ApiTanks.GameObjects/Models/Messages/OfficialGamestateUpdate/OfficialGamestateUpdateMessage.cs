using System;

namespace ApiTanks.GameObjects.Models.Messages
{
    public class OfficialGamestateUpdateMessage : Message
    {
        public long GameLoopNumber {get; set;}

        public OfficialGamestateUpdateMessage (long currentGameLoop)
        {
            GameLoopNumber = currentGameLoop;
        }
    }

}