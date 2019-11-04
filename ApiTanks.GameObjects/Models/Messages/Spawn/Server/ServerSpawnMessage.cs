namespace ApiTanks.GameObjects.Models.Messages
{
    public abstract class ServerSpawnMessage : SpawnMessage
    {
        public Location Location {get; set;}
        
        protected ServerSpawnMessage (Entity entity) 
            : base(entity.Team.Id, entity.Id) 
        {
            Location = entity.Position;
        }
    }

}