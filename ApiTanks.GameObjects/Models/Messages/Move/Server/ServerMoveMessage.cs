namespace ApiTanks.GameObjects.Models.Messages
{
    public abstract class ServerMoveMessage : MoveMessage
    {
        public Location Position {get; set;}
        protected ServerMoveMessage (Entity entity)
            : base (entity.Team.Id, entity.Id)
        {
            Position = entity.Position;
        }
        
    }

}