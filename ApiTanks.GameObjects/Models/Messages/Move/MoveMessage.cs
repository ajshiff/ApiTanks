namespace ApiTanks.GameObjects.Models.Messages
{
    public abstract class MoveMessage : Message
    {
        protected MoveMessage (string teamId, string entityId)
        {
            TeamId = teamId;
            TankId = entityId;
        }
        public string TeamId {get; set;}
        public string TankId {get; set;}
    }

}