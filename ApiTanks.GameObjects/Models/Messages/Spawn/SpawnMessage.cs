namespace ApiTanks.GameObjects.Models.Messages
{
    public abstract class SpawnMessage : Message
    {
        protected SpawnMessage(string teamId, string tankId)
        {
            TeamId = teamId;
            TankId = tankId;
        }
        public string TeamId {get; set;}
        public string TankId {get; set;}
    }

}