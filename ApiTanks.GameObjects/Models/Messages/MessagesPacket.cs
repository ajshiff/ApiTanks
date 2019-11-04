using System.Collections.Generic;

namespace ApiTanks.GameObjects.Models.Messages
{
    public class MessagesPacket
    {
        
        public List<ServerSpawnTankMessage> ServerSpawnTankMessages {get; set;} 
            = new List<ServerSpawnTankMessage>();
        public List<ServerSpawnBulletMessage> ServerSpawnBulletMessages {get; set;} 
            = new List<ServerSpawnBulletMessage>();
        public List<ServerMoveTankMessage> ServerMoveTankMessages {get; set;} 
            = new List<ServerMoveTankMessage>();
        public List<ServerMoveBulletMessage> ServerMoveBulletMessages {get; set;} 
            = new List<ServerMoveBulletMessage>();
        public List<ServerDestroyTankMessage> ServerDestroyTankMessages {get; set;} 
            = new List<ServerDestroyTankMessage>();
        public List<ServerDestroyBulletMessage> ServerDestroyBulletMessages {get; set;} 
            = new List<ServerDestroyBulletMessage>();
        public List<UpdateScoreMessage> UpdateScoreMessages {get; set;} 
            = new List<UpdateScoreMessage>();
        public OfficialGamestateUpdateMessage OfficialGamestateUpdateMessage {get; set;} 
            = new OfficialGamestateUpdateMessage(0);

        
    }

}